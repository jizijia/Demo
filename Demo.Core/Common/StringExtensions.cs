using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace Demo.Core.Common
{
    public static class StringExtensions
    {


        /// <summary>
        /// 确保字符串不超过最大允许长度(扩展)
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="postfix">当不为空的时候, 将用postfix补足到醉倒长度</param>
        /// <returns></returns>
        public static string EnsureMaximumLength(this string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 确保字符串为字符都为阿拉伯数字(扩展)
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>如果有非阿拉伯字符, 将被过滤掉</returns>
        public static string EnsureNumericOnly(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// 确保字符串不为空,当为空的时候赋值string.Empty(扩展)
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(this string str)
        {
            if (str == null)
                return string.Empty;
            return str;
        }

        /// <summary>
        /// 判断字符串是否为空(扩展)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOfEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        #region 字符串截取

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len">截取的长度</param>
        /// <returns></returns>
        public static string SubString(this string str, int len)
        {
            string result = str;
            if (str.Length >= len)
            {
                result = str.Substring(0, len);
            }
            return result;
        }

        /// <summary>
        /// 截取字符串并且在尾部添加需要的内容
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="appendCode"></param>
        /// <returns></returns>
        public static string SubStringAndAppend(this string str, int length, string appendCode)
        {
            return SubString(str, length) + appendCode;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string SubString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex = startIndex - length;
                }

                if (startIndex > str.Length)
                    throw new Exception("The start index is bigger then string lenght!");
            }
            else
            {
                if (length < 0)
                    throw new Exception("Length is error!");
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                        return "";
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 取指定长度的字符串
        /// 注开始长度大于字符串长度将暴异常
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">指定长度</param>
        /// <param name="tailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string SubString(this string str, int startIndex, int length, string tailString)
        {
            string myResult = str;

            Byte[] bComments = Encoding.UTF8.GetBytes(str);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //当截取的起始位置超出字段串长度时
                    if (startIndex >= str.Length)
                    {
                        throw new Exception("The start index is bigger then string lenght!");
                    }
                    else
                    {
                        return str.Substring(startIndex, ((length + startIndex) > str.Length) ? (str.Length - startIndex) : length);
                    }
                }
            }


            if (length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(str);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > startIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (startIndex + length))
                    {
                        p_EndIndex = length + startIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        length = bsSrcString.Length - startIndex;
                        tailString = "";
                    }



                    int nRealLength = length;
                    int[] anResultFlag = new int[length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = startIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[length - 1] == 1))
                    {
                        nRealLength = length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, startIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + tailString;
                }
            }

            return myResult;
        }
        #endregion

        #region 分割字符串

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(this  string strContent, string strSplit)
        {
            if (!strContent.IsNullOfEmpty())
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(this string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }


        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int ByteLength(this string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        #endregion

        #region "处理数组"
        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        private static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                        return i;
                }
                else if (strSearch == stringArray[i])
                    return i;
            }
            return -1;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        private static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string stringArray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringArray, strsplit), caseInsensetive);
        }
        #endregion

        #region 字符串数组
        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <param name="maxElementLength">字符串数组中单个元素的最大长度</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(this string[] strArray, int maxElementLength)
        {
            Hashtable h = new Hashtable();

            foreach (string s in strArray)
            {
                string k = s;
                if (maxElementLength > 0 && k.Length > maxElementLength)
                {
                    k = k.Substring(0, maxElementLength);
                }
                h[k.Trim()] = s;
            }

            string[] result = new string[h.Count];

            h.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(this string[] strArray)
        {
            return DistinctStringArray(strArray, 0);
        }

        #endregion


        public static bool LeftLike(this string text, string value)
        {
            if (text.Length < value.Length)
                return false;
            return (text.Substring(0, value.Length) == value);
        }
        public static bool LeftLike(this string text, string value, bool isContainSelf)
        {
            if (!isContainSelf)
            {
                if (text.Length == value.Length)
                    return false;
            }
            if (text.Length < value.Length)
                return false;
            return (text.Substring(0, value.Length) == value);
        }
    }
}
