using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace Demo.Core.Common
{
    /// <summary>
    /// 加密解密
    /// 2011-05-11
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private static byte[] MakeMD5(byte[] original)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }
        

        #region 自定义字符串加密/解密
        /// <summary>
        /// 自定义字符串加密  进行位移操作
        /// </summary>
        /// <param name="str">待加密数据</param>
        /// <returns>加密后的数据</returns>
        public static string EncryptString(string Input)
        {
            string _temp = "";
            int _inttemp;
            char[] _chartemp = Input.ToCharArray();
            for (int i = 0; i < _chartemp.Length; i++)
            {
                _inttemp = _chartemp[i] + 1;
                _chartemp[i] = (char)_inttemp;
                _temp += _chartemp[i];
            }
            return _temp;
        }

        /// <summary>
        /// 自定义字符串解密
        /// </summary>
        /// <param name="str">待解密数据</param>
        /// <returns>解密成功后的数据</returns>
        public static string DecryptString(string Input)
        {
            string _temp = "";
            int _inttemp;
            char[] _chartemp = Input.ToCharArray();
            for (int i = 0; i < _chartemp.Length; i++)
            {
                _inttemp = _chartemp[i] - 1;
                _chartemp[i] = (char)_inttemp;
                _temp += _chartemp[i];
            }
            return _temp;
        }
        #endregion

        /// <summary>
        /// 检测是否有Sql危险字符
        /// 判断是否存在特殊字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 清除所有脚本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string InputTextSecurity(string text)
        {
            if (text == null || text == string.Empty)
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = text.Replace("'", "''");
            return FilterScript(text);
        }

        /// <summary>
        /// 清除所有脚本
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterScript(string content)
        {
            if (content == null || content == "")
            {
                return content;
            }
            string regexstr = @"(?i)<script([^>])*>(\w|\W)*</script([^>])*>";//@"<script.*</script>";
            content = Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<script([^>])*>", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(content, "</script>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// OCM 单向加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String OcmPwdEncode(string s)
        {
            char[] hexDigits = new char[]{ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
				'a', 'b', 'c', 'd', 'e', 'f' };

            byte[] strTemp = Encoding.UTF8.GetBytes(s);
            byte[] md = MakeMD5(strTemp);
            int j = md.Length;
            char[] str = new char[j * 2];
            int k = 0;
            for (int i = 0; i < j; i++)
            {
                byte byte0 = md[i];
                str[k++] = hexDigits[byte0 >> 4 & 0xf];
                str[k++] = hexDigits[byte0 & 0xf];
            }
            string str1 = new string(str);
            string str2 = string.Empty;
            for (int i = 8; i < 24; i++)
            {
                str2 = str2 + str1.ToCharArray()[i];
            }
            return str2;
        }

        public static string ToHMAC(this string data, string key = "tohmac")
        {
            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            byte[] stringBytes = Encoding.UTF8.GetBytes(data);
            byte[] hashedValue = hmac.ComputeHash(stringBytes);
            return Convert.ToBase64String(hashedValue);
        }
        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="code">要加密的字符串。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(this string code, string key = "fd09ok43")
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(code);
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="code">要解密的以Base64</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(this string code, string key = "fd09ok43")
        {
            byte[] inputByteArray = Convert.FromBase64String(code);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="value">传入值</param>
        /// <param name="charset">编码（默认UTF-8）</param>
        /// <returns></returns>
        public static string Md5(string value, string charset = "UTF-8")
        {
            byte[] bytes;
            MD5 md5 = new MD5CryptoServiceProvider();
            bytes = md5.ComputeHash(Encoding.GetEncoding(charset).GetBytes(value));
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        /// <summary>
        /// HMAC加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Hmac(string value, string key)
        {
            byte[] bytes;
            using (var hmac = new HMACMD5(Encoding.UTF8.GetBytes(key)))
            {
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            StringBuilder result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }
    }
}
