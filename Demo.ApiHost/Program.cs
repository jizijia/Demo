using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Demo.ApiHost
{
    class Program
    {
        static void Main(string[] args)
        { 
            try
            {
                #region Web Api监听  

                //Assembly.Load("Interface");  //手工加载某个api程序集的controller(注意引用)

                var config = new HttpSelfHostConfiguration("http://localhost:8888");//端口号自定义配置  //服务器的本地地址

                config.Routes.MapHttpRoute("default", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });

                // config.EnableCors(new EnableCorsAttribute("*", "*", "*"));


                config.EnableCors();//启动跨域
                var server = new HttpSelfHostServer(config);
                Console.WriteLine("WebApi Host is starting.");

                server.OpenAsync().Wait(); //开启监听
                Console.ReadLine();
                #endregion
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
            }

        }
    }
}
