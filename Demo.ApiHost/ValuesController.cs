using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.IO;
using System.Xml.Linq;

namespace Interface
{

    /******
     *  Web API 的自宿主技术,可以将API寄宿在 windows from程序，windows控制台程序，还有windowsServices里面-可以不再依赖IIS
     *  注意：这是寄宿在一个单独独立的进程.不想IIS多线程.
     *  因此:多个请求时候不是每一个单独的进程.都是同步的进程
     *  所有要实现了多进程的方式，在每个APIController 方法上面加上aynsc
     *  
     *  需要引用的package:(Microsoft.AspNet.WebApi.SelfHost) 程序集引用:System.Web.Http.SelfHost
     *  
     *  还有一个问题就是API跨域请求问题（针对ajax请求）
     *  WebApi 跨域问题解决方案：CORS
     *  需要引用的package:Microsoft.AspNet.WebApi.Cors  程序集引用: System.Web.Http.Cors 
     *  
     * *******

    /***
     *  ApiController里面的方法默认都是不支持httpGet 每个方法上面必需要要显示指定请求方式
     *  ApiController里面的方法 如果是POST的请求链接的话 带参数的话必须指定[FromBody]才能获取到参数
     ***/


    [EnableCors(origins: "http://localhost:2951,http://192.168.1.103:38559", headers: "*", methods: "GET,POST")]//跨域的处理
                                                                                                                /*origins:表示允许的请求的源地址  *：代表所有都可以  多个允许用逗号隔开*/

    /*
     * 跨域的问题面向ajax 请求
     *   [FromBody] 特性是针对ajax请求传递参数（必需是POST,才能获取参数）
     *   
     *   eg:http访问不需要带[FromBody]（最好不要设置）
     */
    /// <summary>
    ///  web APi 的接口
    /// </summary>
    public class ValuesController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Test()
        {
            Stream s = Request.Content.ReadAsStreamAsync().Result;//获取客户端请求流

            StreamReader read = new StreamReader(s, Encoding.ASCII);
            string Name = read.ReadToEnd();


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent(Name, Encoding.Unicode);
            //  HttpResponseMessage response = new HttpResponseMessage();
            return response;

            // return name;
        }

        // 

        [HttpGet]
        public string version([FromBody]XObject model)
        {
            return "1.0.1.12";
        }

        [HttpPost]
        public string getData([FromBody]XObject model)
        {
            return "fdsfdsf";
        }
    }
}
