using Demo.ApiHost.ActionResults;
using Demo.Core.Base;
using Demo.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Demo.ApiHost
{
    public class BaseApiController : ApiController
    {

        /// <summary>
        /// 获取流参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T GetParams<T>(T value)
        {
            var post = "{}";
            try
            {
                Stream s = Request.Content.ReadAsStreamAsync().Result;//获取客户端请求流
                StreamReader read = new StreamReader(s, Encoding.ASCII);
                string Name = read.ReadToEnd();
            }
            catch (Exception)
            {
                throw new Exception("Failed to receive data.");
            }
            return JsonHelper.ToClass(post, value);
        }

        //public Stream Json(Func<object> func)
        //{

        //}

        ///// <summary>
        ///// 跨域设置
        ///// </summary>
        //private void SetContext()
        //{
        //    var referer = _current.IncomingRequest.Headers["Origin"];

        //    _current.OutgoingResponse.ContentType = "application/json";
        //    _current.OutgoingResponse.Headers.Add("Access-Control-Allow-Credentials", "true");
        //    _current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "origin, secret,content-type, accept,cookie");
        //    _current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET,POST,DELETE,PUT,OPTIONS");
        //    _current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", referer);
        //    _current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
        //}

        /// <summary>
        /// 设置secret
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public string SetSecret(UserTicket ticket)
        {
            ticket.Ip = NetworkHelper.GetClientIP();
            var temp = JsonHelper.ToJson(ticket).Encrypt();
            return string.Format("{0}|{1}", temp, temp.ToHMAC());
        }

        #region Custom Error Action Results
        protected static NotFoundActionResult NotFound(HttpRequestMessage request, string message)
        {
            return new NotFoundActionResult(request, message);
        }
        #endregion

        public DealResult DealResult(bool result, string message = "", object data = null)
        {
            DealResult obj = new DealResult() { Result = result, Message = message, Data = data };
            return obj;
        }
    }
}
