using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Demo.Core.Common
{
    public static class DomainValidations
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
        public static ValidationResult IsNumeric(object expression)
        {
            if (expression != null)
            {
                string str = expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            return new ValidationResult("不是Int32类型.");
        }

        /// <summary>
        /// 判断一个字符串是否时间格式
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static ValidationResult IsDateTime(string inputData)
        {
            try
            {
                Convert.ToDateTime(inputData);
                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult("不是日期类型.");
            }
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ValidationResult IsDouble(object expression)
        {
            if (expression != null)
            {
                if (Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$"))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("不是Double类型.");
        }

        /// <summary>
        /// 是否为整数类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ValidationResult IsNumber(string s)
        {
            if (Regex.IsMatch(s, "^\\d+$"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("不是整数.");
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static ValidationResult IsNumberSign(string inputData)
        {
            Match m = REG_NUMBER_SIGN.Match(inputData);
            if (m.Success)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("不是正/负数字.");
        }
        #endregion

        #region "中英文判断"
        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static ValidationResult IsContentCHZN(string inputData)
        {
            Match m = REG_CHZN.Match(inputData);
            if (m.Success)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("包含中文字符.");
        }

        /// <summary>
        /// 判断是否为中文字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ValidationResult IsChinese(string s)
        {
            if (Regex.IsMatch(s, @"^[\u4e00-\u9fa5]+$"))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("不是中文字符串.");
        }
        #endregion

        #region "常用格式判断"
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static ValidationResult IsEmail(string inputData)
        {
            Match m = REG_EMAIL.Match(inputData);
            if (m.Success)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("不是Email格式.");
        }

        /// <summary>
        /// 验证是否是电话
        /// </summary>
        /// kevin 12.12
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public static ValidationResult IsPhone(string inputDate)
        {
            if (!(inputDate == null || inputDate == ""))
            {
                Match m = REG_TELLEPHONE.Match(inputDate);
                if (m.Success)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("不是电话号码格式.");
        }
        #endregion
    }
}

