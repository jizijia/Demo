using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace Demo.Core.Common
{
    public class NetworkHelper
    {
        //判断网络连接
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(
        ref int dwFlag,
        int dwReserved);
        ///<summary>
        /// 检测本机的网络连接
        ///</summary>

        public static bool ConnectedState()
        {
            int i = 0;
            if (InternetGetConnectedState(ref i, 0))
                return true;
            else
                return false;
        }

        public static string Connect(string url, Action<HttpWebRequest> action, object data = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            action(request);

            var json = JsonHelper.ToJson(data == null ? new { } : data);
            var bytes = Encoding.UTF8.GetBytes(json);
            request.ContentLength = bytes.Length;
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            using (WebResponse wr = request.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                var result = reader.ReadToEnd();
                return result;
            }
        }
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            var context = OperationContext.Current;
            var messageProperties = context.IncomingMessageProperties;
            var endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpointProperty.Address;
        }
        /// <summary>
        /// 获取来源地址
        /// </summary>
        /// <returns></returns>
        public static string ReferrerUrl()
        {
            return  WebOperationContext.Current.IncomingRequest.Headers.Keys[0];
            //return HttpContext.Current.Request.Url.AbsoluteUri;
        }
        /// <summary>
        /// 获取访问路径
        /// </summary>
        /// <returns></returns>
        public static string PageUrl()
        {
            return "";
        }
    }
}
