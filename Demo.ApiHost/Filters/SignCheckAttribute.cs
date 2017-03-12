using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApiHost.Filters
{
    /// <summary>
    /// 表示使用默认用户验证签名
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AnonymousSignAttribute : Attribute
    {
    }
    /// <summary>
    /// 表示需要验证签名
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SignCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string error;

            bool needLogin = true;
            var actAttributes = actionContext.ActionDescriptor.GetCustomAttributes<AnonymousSignAttribute>();
            needLogin = actAttributes.Count == 0;
            var a = CheckRequest(actionContext, out error, needLogin);
            if (!a)
            {
                var resp = new Result.DealResult() { Result = false, Message = error };
                actionContext.Response = actionContext.Request.CreateResponse<Result.DealResult>(HttpStatusCode.OK, resp);
            }
            base.OnActionExecuting(actionContext);
        }
        static Dictionary<string, DateTime> tokenCache = new Dictionary<string, DateTime>();
        static object lockObj = new object();
        static bool threadStart = false;
        bool CheckRequest(HttpActionContext context, out string CurrentError, bool needLogin = true)
        {
            CurrentError = "";

            LoginStatus status;
            //var context = System.Web.HttpContext.Current;
            var cookie = context.Request.Headers.GetCookies("user");//必须传USER
            string userAccount = "";
            if (cookie == null || cookie.Count == 0)
            {
                CurrentError = "接口:传入凭证Cookie user为空";
                return false;
            }
            userAccount = cookie.First().Cookies.First().Value.ToString();
            status = LoginStatusContext.CheckLogined(userAccount);
            if (status == null)
            {
                CurrentError = "接口:没有找到登录状态,请重新登录";
                return false;
            }
            int CurrentUserId;
            string CurrentAccount;
            CurrentUserId = status.UserId;
            CurrentAccount = userAccount;
            if (CurrentUserId == 0 && needLogin)
            {
                CurrentError = "接口:登录认证失败,请重新登录,使用了默认用户请求了认证接口";
                return false;
            }
            if (!Setting.CheckSign)
            {
                return true;
            }
            #region 过期TOKEN清理线程
            if (!threadStart)
            {
                threadStart = true;
                var thread = new System.Threading.Thread(() =>
                {
                    while (true)
                    {
                        var dic2 = new Dictionary<string, DateTime>(tokenCache);
                        var time2 = DateTime.Now.AddSeconds(-30);
                        var removes = dic2.Where(b => b.Value < time2).Select(b => b.Key);
                        foreach (var _key in removes)
                        {
                            tokenCache.Remove(_key);
                        }
                        System.Threading.Thread.Sleep(30 * 1000);
                    }
                });
                thread.Start();
            }
            #endregion

            string key = status.Key;
            string token = "";
            DateTime time = DateTime.Now;

            //检查消息
            var dic = new SortedDictionary<string, object>();
            if (context.Request.Method == HttpMethod.Get)
            {
                #region 按GET取参数
                foreach (var item in context.Request.GetQueryNameValuePairs())
                {
                    string name = item.Key.ToLower();
                    object value = item.Value;
                    dic.Add(name, value);
                }
                if (!dic.ContainsKey("token"))
                {
                    CurrentError = "GET 缺少参数 Token";
                    return false;
                }
                token = dic["token"].ToString();
                time = Convert.ToDateTime(dic["time"]);
                #endregion
            }
            else
            {
                #region 按POST
                var args = context.ActionArguments;
                if (args.Count == 0)
                {
                    CurrentError = "POST 对象为空";
                    return false;
                }
                var msg = args.FirstOrDefault().Value as Parame.ParameBase;
                if (msg == null)
                {
                    CurrentError = "POST 对象类型不为ParameBase";
                    return false;
                }
                token = msg.Token;
                if (string.IsNullOrEmpty(token))
                {
                    CurrentError = "POST 缺少参数 Token";
                    return false;
                }
                time = Convert.ToDateTime(msg.Time);
                var properties = msg.GetType().GetProperties();
                foreach (var item in properties)
                {
                    //集合类型不参与计算
                    //if (item.PropertyType.BaseType == typeof(System.Array))
                    //{
                    //    continue;
                    //}
                    //if (item.PropertyType.GenericTypeArguments.Length > 0)
                    //{
                    //    continue;
                    //}
                    if (item.PropertyType.IsEnumerable())
                    {
                        continue;
                    }
                    var value = item.GetValue(msg);
                    if (item.PropertyType == typeof(bool))
                    {
                        value = Convert.ToInt32(value);
                    }
                    dic.Add(item.Name.ToLower(), value);
                }
                #endregion
            }

            string tokenPath = string.Format("{0}_{1}_{2}", CurrentUserId, context.Request.RequestUri, token);
            if (tokenCache.ContainsKey(tokenPath) && CurrentUserId > 0)
            {
                CurrentError = "接口:该签名已经使用过:";
                return false;
            }
            string str = "";
            foreach (var item in dic)
            {
                if (item.Key.ToLower() == "token")
                {
                    continue;
                }
                str += item.Value + "";
            }
            string token2 = Demo.Core.Common.SecurityHelper.Md5(str + key);
            if (needLogin)
            {
                time = time.AddSeconds(status.TimeDiff);
            }
            var ts = DateTime.Now - time;
            if (ts.Minutes > 10 && needLogin)
            {
                CurrentError = "接口:数据签名超时";
                return false;
            }
            if (token.ToUpper() != token2)
            {
                CurrentError = "接口:数据签名验证失败";
                if (!CoreHelper.RequestHelper.IsRemote)
                {
                    CoreHelper.EventLog.Log("签名串:" + (str + key));
                }
                return false;
            }
            lock (lockObj)
            {
                if (!tokenCache.ContainsKey(tokenPath))
                {
                    tokenCache.Add(tokenPath, DateTime.Now);
                }
            }
            return true;
        }

    }
}
