using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
namespace Demo.Core.Common
{
    /// <summary>
    /// 获取服务器的相关信息
    /// </summary>
    public class  Server
    {
            /// <summary>
            /// 取得网站的根目录的URL
            /// </summary>
            /// <returns></returns>
            public  string GetRootURI()
            {
                string appPath = "";
                HttpContext HttpCurrent = HttpContext.Current;
                HttpRequest Req;
                if (HttpCurrent != null)
                {
                    Req = HttpCurrent.Request;
                    string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                    if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    {
                        appPath = UrlAuthority;
                    }
                    else
                    {
                        appPath = UrlAuthority + Req.ApplicationPath;
                    }
                }
                return appPath;
            }

            /// <summary>
            /// 获取网站的根目录的物理路径
            /// </summary>
            /// <returns></returns>
            public  string GetRootPath()
            {
                string appPath = "";
                HttpContext HttpCurrent = HttpContext.Current;
                if (HttpCurrent != null)
                {
                    appPath = HttpCurrent.Server.MapPath("~");
                }
                else
                {
                    appPath = AppDomain.CurrentDomain.BaseDirectory;
                    if (Regex.Match(appPath, @"\\$", RegexOptions.Compiled).Success)
                    {
                        appPath = appPath.Substring(0, appPath.Length - 1);
                    }
                }
                return appPath;
            }

            /// <summary>
            /// 获取网站目录路径
            /// </summary>
            /// <returns></returns>
            public  string GetAppPath()
            {
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    return string.Empty;
                }
                else
                {
                    return HttpContext.Current.Request.ApplicationPath;
                }
            }

            /// <summary>
            /// 获取服务器路径
            /// </summary>
            /// <returns></returns>
            public  string GetServerPath()
            {
                return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
            }
            /// <summary>
            /// 版本信息
            /// </summary>
            /// <returns></returns>
            public  string VersionInfomation()
            {
                string vesion = ReadFileContent(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"] + "admin/vesion.ini").Trim();
                return vesion;
            }
            /// <summary>
            /// 获取文件内容
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            private  string ReadFileContent(string filePath)
            {
                return ReadFileContent(filePath, System.Text.Encoding.Default);
            }

            /// <summary>
            /// 获取文件内容
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="encoding"></param>
            /// <returns></returns>
            private  string ReadFileContent(string filePath, Encoding encoding)
            {
                try
                {
                    string fileContent = "";
                    using (StreamReader sr = new StreamReader(filePath, encoding))
                    {
                        fileContent = sr.ReadToEnd();
                    }
                    return fileContent;
                }
                catch
                {
                    return "Reed file course an unknow errer.";
                }
            }

            /// <summary>
            /// 获得当前绝对路径
            /// </summary>
            /// <param name="strPath">指定的路径</param>
            /// <returns>绝对路径</returns>
            public  string GetMapPath(string strPath)
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Server.MapPath(strPath);
                }
                else //非web程序引用
                {
                    strPath = strPath.Replace("/", "\\");
                    if (strPath.StartsWith("\\"))
                    {
                        strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                    }
                    return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
                }
            }

            /// <summary>
            /// 获取服务器的计算机名称
            /// </summary>
            /// <returns></returns>
            public  string GetMachineName()
            {
                return HttpContext.Current.Server.MachineName;
            }
            #region ".NET 服务器相关信息"
            /// <summary>
            /// 获取服务器IP地址
            /// </summary>
            /// <returns></returns>
            public  string GeLocalAddress()
            {
                return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }

            /// <summary>
            /// 获取服务器域名
            /// </summary>
            /// <returns></returns>
            public  string GetServerName()
            {
                return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            }

            /// <summary>
            /// 获取 .NET解释引擎版本
            /// 如.NET CLR 2.0.50727.1873 
            /// </summary>
            /// <returns></returns>
            public  string GetVersion()
            {
                int build, major, minor, revision;
                build = Environment.Version.Build;
                major = Environment.Version.Major;
                minor = Environment.Version.Minor;
                revision = Environment.Version.Revision;
                return ".NET CLR  " + major + "." + minor + "." + build + "." + revision;
            }

            /// <summary>
            /// 获取HTTP访问端口
            /// </summary>
            /// <returns></returns>
            public  string GetServerPost()
            {
                return HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            }
            /// <summary>
            /// 获取服务器IIS版本
            /// </summary>
            /// <returns></returns>
            public  string GetServerSoftware()
            {
                return HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
            }

            /// <summary>
            /// 获取服务器现在时间
            /// </summary>
            /// <returns></returns>
            public  string GetServerDateTime()
            {
                return DateTime.Now.ToString();
            }

            /// <summary>
            /// 获取服务端脚本执行超时
            /// </summary>
            /// <returns></returns>
            public  string GetServerScriptTimeOut()
            {
                return HttpContext.Current.Server.ScriptTimeout.ToString();
            }

            /// <summary>
            /// 获取服务器操作系统
            /// </summary>
            /// <returns></returns>
            public  string GetOSVersion()
            {
                return Environment.OSVersion.ToString();
            }
            /// <summary>
            /// 获取虚拟目录绝对路径
            /// </summary>
            /// <returns></returns>
            public  string GetApplicationPhysucakPath()
            {
                return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
            }
            /// <summary>
            /// 获取执行文件绝对路径
            /// </summary>
            /// <returns></returns>
            public  string GetApplicationTranslated()
            {
                return HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"];
            }

            /// <summary>
            /// 获取客户端语言(不确定是客户端还是服务端)
            /// </summary>
            /// <returns></returns>
            public  string GetHttpAcceptLanguage()
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            }

            /// <summary>
            /// 是否支持Https
            /// </summary>
            /// <returns></returns>
            public  string HttpsIsEnable()
            {
                return HttpContext.Current.Request.ServerVariables["HTTPS"];
            }
            #endregion

            #region "常见组件支持情况"
            /// <summary>
            /// 是否支持Access数据库
            /// </summary>
            /// <returns></returns>
            public  string AccessIsEnable()
            {
                if (CheckObject("ADODB.RecordSet"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            ///  是否支持FSO
            /// </summary>
            /// <returns></returns>
            public  string FSOIsEnable()
            {
                if (CheckObject("Scripting.FileSystemObject"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            ///  是否支持CDONTS邮件发送
            /// </summary>
            /// <returns></returns>
            public  string CDONTSIsEnable()
            {
                if (CheckObject("CDONTS.NewMail"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            ///  获取虚拟目录Session总数
            /// </summary>
            /// <returns></returns>
            public  string GetSessionCount()
            {
                return HttpContext.Current.Session.Contents.Count.ToString();
            }

            /// <summary>
            ///  获取虚拟目录Application总数
            /// </summary>
            /// <returns></returns>
            public  string GetApplicationCount()
            {
                return HttpContext.Current.Application.Contents.Count.ToString();
            }

            /// <summary>
            /// 是否支持JMail
            /// </summary>
            /// <returns></returns>
            public  string JMailIsEnable()
            {
                if (CheckObject("JMail.SmtpMail"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否支持ASPemail
            /// </summary>
            /// <returns></returns>
            public  string ASPemailIsEnable()
            {
                if (CheckObject("Persits.MailSender"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否支持SmtpMail
            /// </summary>
            /// <returns></returns>
            public  string SmtpMailIsEnable()
            {
                if (CheckObject("SmtpMail.SmtpMail.1"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否Geocel发信
            /// </summary>
            /// <returns></returns>
            public  string GeocelMailIsEnable()
            {
                if (CheckObject("Geocel.Mailer"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否支持ASPUpload文件上传
            /// </summary>
            /// <returns></returns>
            public  string ASPUploadIsEnable()
            {
                if (CheckObject("Persits.Upload.1"))
                    return "支持";
                else
                    return "不支持";
            }
            /// <summary>
            /// 是否支持ASPCN文件上传
            /// </summary>
            /// <returns></returns>
            public  string ASPCNUploadIsEnable()
            {
                if (CheckObject("aspcn.Upload"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否支持个人文件上传组件
            /// </summary>
            /// <returns></returns>
            public  string LyfUploadIsEnable()
            {
                if (CheckObject("LyfUpload.UploadFile"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否支持SoftArtisans文件管理
            /// </summary>
            /// <returns></returns>
            public  string SoftArtisansFileManagerIsEnable()
            {
                if (CheckObject("SoftArtisans.FileManager"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否支持Dimac文件上传
            /// </summary>
            /// <returns></returns>
            public  string DimacManagerIsEnable()
            {
                if (CheckObject("w3.upload"))
                    return "支持";
                else
                    return "不支持";
            }

            /// <summary>
            /// 是否支持Dimac的图像读写组件
            /// </summary>
            /// <returns></returns>
            public  string DimacImageWriteAndReadIsEnable()
            {
                if (CheckObject("W3Image.Image"))
                    return "支持";
                else
                    return "不支持";
            }
            #endregion

            #region "浏览者相关信息"
            /// <summary>
            /// 获取客户端浏览器
            /// </summary>
            /// <returns></returns>
            public  string GetClientBrowser()
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.Browser.ToString();
            }

            /// <summary>
            /// 获取客户端浏览器是否支持Cookies
            /// </summary>
            /// <returns></returns>
            public  string ClientBrowserCookieIsEnable()
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.Cookies.ToString();
            }

            /// <summary>
            /// 获取客户端浏览器是否支持Frames(分栏)
            /// </summary>
            /// <returns></returns>
            public  string ClientBrowserFramesIsEnable()
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.Frames.ToString();
            }

            /// <summary>
            /// 获取客户端浏览器是否支持JavaApplets
            /// </summary>
            /// <returns></returns>
            public  string ClientBrowserJavaAppletsIsEnable()
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.JavaApplets.ToString();
            }

            /// <summary>
            /// 获取浏览者操作系统
            /// </summary>
            /// <returns></returns>
            public  string GetClientBrowserPlatform()
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.Platform.ToString();
            }


            /// <summary>
            /// 获取浏览者浏览器是否支持VBScript
            /// </summary>
            /// <returns></returns>
            public  string ClientBrowserVBScriptIsEnable()
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.VBScript.ToString();
            }

            /// <summary>
            /// 获取浏览者浏览器版本
            /// </summary>
            /// <returns></returns>
            public  string ClientBrowserVersion()
            {
                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                return bc.Version.ToString();
            }

            /// <summary>
            /// 获取浏览者IP地址
            /// </summary>
            /// <returns></returns>
            public  string ClientIPAddress()
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            #endregion

            /// <summary>
            /// 组件支持验证代码
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            private  bool CheckObject(string obj)
            {
                object meobj = HttpContext.Current.Server.CreateObject(obj);
                return (true);
            }

            /// <summary>
            ///  服务器加法循环测试
            /// </summary>
            /// <param name="Sender"></param>
            /// <param name="e"></param>
            public string TurnCheck(int count)
            {
                DateTime ontime = DateTime.Now;
                int sum = 0;
                for (int i = 1; i <= count; i++)
                {
                    sum = sum + i;
                }
                DateTime endtime = DateTime.Now;
                return ((endtime - ontime).TotalMilliseconds).ToString() + "毫秒";
            }
            /// <summary>
            /// 自定义组件检测
            /// </summary>
            /// <param name="Sender"></param>
            /// <param name="e"></param>
            public string CheckControls(string controlName)
            {
                if (CheckObject(controlName))
                    return "检测结果：支持组件" + controlName;
                else
                    return "检测结果：不支持组件" + controlName;
            }

            public  string ServerPath = "http://ibaofashion.com";
        }
    }
