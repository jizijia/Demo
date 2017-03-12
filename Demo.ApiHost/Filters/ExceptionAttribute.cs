using Demo.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Demo.ApiHost.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //这里可以加入Log部分，记录异常日志
            var resp = new BaseResult()
            {
                Code = MessageCode.SERVER_ERROR_500,
                Data = null,
                Message = actionExecutedContext.Exception.Message,
                IsSucceed = false
            };
             
            string parame = "";
            var args = actionExecutedContext.ActionContext.ActionArguments;
            if (actionExecutedContext.Request.Method == HttpMethod.Post)
            {
                foreach (var item in args)
                {
                    parame += string.Format("\r\n[POST]:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
                }
            }
            //CoreHelper.EventLog.Error(actionExecutedContext.Exception.ToString() + parame);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse<BaseResult>(HttpStatusCode.OK, resp);
            base.OnException(actionExecutedContext);
        }
    }
}
