using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TBS.Core.Common;

namespace Tools.Common
{
    public class RestConnector
    {
        public static string Connect(string url, string json)
        {  
            var result = Connect(url, a =>
            {
                //伪造数据
                a.Headers.Add("Origin", "emix.com.test");
                a.UserAgent = "emix.com.test";
                //if (UserCache<Member>.IsLogin)
                //{
                //    a.Headers.Add("Secret", UserCache<Member>.Token);
                //}
            }, json);
            return result;
        }


        public static string Connect(string url, Action<HttpWebRequest> action,string json)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            action(request);
             
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
    }
}
