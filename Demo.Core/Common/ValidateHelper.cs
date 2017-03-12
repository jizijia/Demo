using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Demo.Core.Common
{
        public class ValidateHelper
        {
            private static Regex REG_NUMBER = new Regex("^[0-9]+$");
            private static Regex REG_NUMBER_SIGN = new Regex("^[+-]?[0-9]+$");
            private static Regex REG_DECIMAL = new Regex("^[0-9]+[.]?[0-9]+$");
            private static Regex REG_DECIMAL_SIGN = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
            private static Regex REG_EMAIL = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
            private static Regex REG_CHZN = new Regex("[\u4e00-\u9fa5]");
            private static Regex REG_TELLEPHONE = new Regex("^(([0-9]{3,4}-)|[0-9]{3.4}-)?[0-9]{7,8}$");
            private static Regex REG_SEND = new Regex("[1-9]{1}([0-9]+){5}");
            private static Regex REG_URL = new Regex("^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$");
            private static Regex REG_MOBILE_PHONE = new Regex("^13|15|18[0-9]{9}$");

			

            #region "基本数据类型判断"
            /// <summary>
            /// 判断对象是否为Int32类型的数字
            /// </summary>
            /// <param name="Expression"></param>
            /// <returns></returns>
            public static bool IsNumeric(object expression)
            {
                if (expression != null)
                {
                    return IsNumeric(expression.ToString());
                }
                return false;

            }
            /// <summary>
            /// 判断一个字符串是否时间格式
            /// </summary>
            /// <param name="inputData">输入字符串</param>
            /// <returns></returns>
            public static bool IsDateTime(string inputData)
            {
                try
                {
                    Convert.ToDateTime(inputData);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 判断对象是否为Int32类型的数字
            /// </summary>
            /// <param name="Expression"></param>
            /// <returns></returns>
            public static bool IsNumeric(string expression)
            {
                if (expression != null)
                {
                    string str = expression;
                    if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                    {
                        if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        {
                            return true;
                        }
                    }
                }
                return false;

            }

            /// <summary>
            /// 是否为Double类型
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public static bool IsDouble(object expression)
            {
                if (expression != null)
                {
                    return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
                }
                return false;
            }

            /// <summary>
            /// 是否为数字类型
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static bool IsNumber(string s)
            {
                return Regex.IsMatch(s, "^\\d+$");
            }

            /// <summary>
            /// 是否数字字符串 可带正负号
            /// </summary>
            /// <param name="inputData">输入字符串</param>
            /// <returns></returns>
            public static bool IsNumberSign(string inputData)
            {
                Match m = REG_NUMBER_SIGN.Match(inputData);
                return m.Success;
            }

            /// <summary>
            /// 判断是否为base64字符串
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static bool IsBase64String(string str)
            {
                //A-Z, a-z, 0-9, +, /, =
                return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
            }
            /// <summary>
            /// 是否是浮点数
            /// </summary>
            /// <param name="inputData">输入字符串</param>
            /// <returns></returns>
            public static bool IsDecimal(string inputData)
            {
                Match m = REG_DECIMAL.Match(inputData);
                return m.Success;
            }

            /// <summary>
            /// 是否是浮点数 可带正负号
            /// </summary>
            /// <param name="inputData">输入字符串</param>
            /// <returns></returns>
            public static bool IsDecimalSign(string inputData)
            {
                Match m = REG_DECIMAL_SIGN.Match(inputData);
                return m.Success;
            }
            #endregion

            #region "中英文判断"
            /// <summary>
            /// 检测是否有中文字符
            /// </summary>
            /// <param name="inputData"></param>
            /// <returns></returns>
            public static bool IsHasCHZN(string inputData)
            {
                Match m = REG_CHZN.Match(inputData);
                return m.Success;
            }

            /// <summary>
            /// 判断是否为中文字符串
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static bool IsChinese(string s)
            {
                return Regex.IsMatch(s, @"^[\u4e00-\u9fa5]+$");
            }
            #endregion

            #region "常用格式判断"
            /// <summary>
            /// 检测是否符合email格式
            /// </summary>
            /// <param name="inputData">输入字符串</param>
            /// <returns></returns>
            public static bool IsEmail(string inputData)
            {
                Match m = REG_EMAIL.Match(inputData);
                return m.Success;
            }

            /// <summary>
            /// 验证是否是电话
            /// </summary>
            /// kevin 12.12
            /// <param name="inputDate"></param>
            /// <returns></returns>
            public static   bool IsPhone(string inputDate)
            {
                if (!(inputDate==null ||inputDate ==""))
                {
                    Match m = REG_TELLEPHONE.Match(inputDate);
                    return m.Success;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 是否是邮政编码
            /// </summary>
            /// kevin 12.12
            /// <param name="inputDate"></param>
            /// <returns></returns>
            public static bool IsPostCode(string inputDate)
            {
                if (!(inputDate==null || inputDate ==""))
                {
                    Match m = REG_SEND.Match(inputDate);
                    return m.Success;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 是否是手机号码
            /// </summary>
            /// kevin 12.12
            /// <param name="inputDate"></param>
            /// <returns></returns>
            public static bool IsMobilePhone(string inputDate)
            {
                Match m = REG_MOBILE_PHONE.Match(inputDate);
                return m.Success;
            }

            /// <summary>
            /// 检测是否是正确的Url
            /// </summary>
            /// <param name="strUrl">要验证的Url</param>
            /// <returns>判断结果</returns>
            public static bool IsURL(string strUrl)
            {
                return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
            }
            /// <summary>
            /// 验证输入字符串为传真号码
            /// </summary>
            /// <param name="P_str_fax">输入字符串</param>
            /// <returns>返回一个bool类型的值</returns>
            public static bool IsFax(string P_str_fax)
            {
                return Regex.IsMatch(P_str_fax, @"86-\d{2,3}-\d{7,8}");
            }

            /// <summary>
            /// 判断字符串是否是IP
            /// </summary>
            /// <param name="ip"></param>
            /// <returns></returns>
            public static bool IsIP(string ip)
            {
                return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

            }

            /// <summary>
            /// 验证字符串是否是图片路径
            /// </summary>
            /// <param name="Input">待检测的字符串</param>
            /// <returns>返回true 或 false</returns>
            public static bool IsImgString(string Input)
            {
                bool re_Val = false;
                if (Input != string.Empty)
                {
                    string s_input = Input.ToLower();
                    if (s_input.IndexOf(".") != -1)
                    {
                        string Ex_Name = s_input.Substring(s_input.LastIndexOf(".") + 1).ToString().ToLower();
                        if (Ex_Name == "jpg" || Ex_Name == "gif" || Ex_Name == "bmp" || Ex_Name == "png")
                        {
                            re_Val = true;
                        }
                    }
                }
                return re_Val;
            }
            #endregion

            /// <summary>
            /// 验证身份号码(15位和18位)
            /// </summary>
            /// <param name="idcard">15位和18位身份证号码</param>
            /// <returns></returns>
            public static bool IsIdcard(string idcard)
            {
                return Regex.IsMatch(idcard, @"(^\d{18}$)|(^\d{15}$)");
            }

            /// <summary>
            /// 验证一年的12月份
            /// </summary>
            /// <param name="month"></param>
            /// <returns></returns>
            public static bool IsMonth(string month)
            {
                return Regex.IsMatch(month, @"^(0?[[1-9]|1[0-2])$");
            }

            /// <summary>
            /// 验证一个月的31天
            /// </summary>
            /// <param name="day"></param>
            /// <returns></returns>
            public static bool IsDay(string day)
            {
                return Regex.IsMatch(day, @"^((0?[1-9])|((1|2)[0-9])|30|31)$");
            }
            
            /// <summary>
            /// 验证大写字母
            /// </summary>
            /// <param name="upChar"></param>
            /// <returns></returns>
            public static bool IsUpChar(string upChar)
            {
                return Regex.IsMatch(upChar, @"^[A-Z]+$");
            }

            /// <summary>
            /// 验证小写字母
            /// </summary>
            /// <param name="upChar"></param>
            /// <returns></returns>
            public static bool IsLowChar(string upChar)
            {
                return Regex.IsMatch(upChar, @"^[a-z]+$");
            }

            /// <summary>
            /// 验证车牌号码
            /// </summary>
            /// <param name="license"></param>
            /// <returns></returns>
            public static bool IsLicense(string license)
            {
                return Regex.IsMatch(license, @"^([\u4e00-\u9fa5]|[A-Z]){1,2}[A-Za-z0-9]{1,2}-[0-9A-Za-z]{5}$");
            }
        }
    }

