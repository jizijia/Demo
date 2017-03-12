using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Common
{
    /// <summary>
    /// 枚举的扩展属性
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; protected set; }
        public DescriptionAttribute(string value)
        {
            this.Description = value;
        }
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举的字符串标识的值(扩展)
        /// [StringValue("")]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            if (fieldInfo == null) return string.Empty;
            DescriptionAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attribs.Length > 0 ? attribs[0].Description : null;

        }


        /// <summary>
        /// 通过实参获取StringValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionByActualParameter<T>(T value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            if (fieldInfo == null) return string.Empty;
            DescriptionAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attribs.Length > 0 ? attribs[0].Description : "";
        }

        /// <summary>
        /// 通过Int参数获取String Value值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionByIntValue<T>(int value)
        {
            T t = (T)Enum.ToObject(typeof(T), value);
            return GetDescriptionByActualParameter<T>(t);
        }

        /// <summary>
        /// 通过字符串获取String Value 值,没有StringValue属性的将回返字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDescription<T>(string str)
        {
            return GetDescriptionExceptNull<T>(str);
        }

        /// <summary>
        /// 通过字符串获取String Value 值, 如果没有StringValue属性的将返回空字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDescriptionExceptNull<T>(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            Type objType = typeof(T);
            string s = string.Empty;
            FieldInfo fileInfo = objType.GetField(str);
            if (fileInfo == null)
            {
                return "";
            }
            DescriptionAttribute[] objStringAttributeAttribute = (DescriptionAttribute[])fileInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objStringAttributeAttribute != null && objStringAttributeAttribute.Length == 1)
            {
                s = objStringAttributeAttribute[0].Description;
            }
            return s;
        }

        /// <summary>
        /// 通过Int值获取枚举实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumByInt<T>(int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static List<EnumEntity> ToList<T>()
        {
            List<EnumEntity> collection = new List<EnumEntity>();
            EnumEntity model = null;
            Type typeOfEnum = typeof(T);
            foreach (string s in Enum.GetNames(typeOfEnum))
            {
                model = new EnumEntity();
                DescriptionAttribute[] objStringAttributeAttribute =
                    (DescriptionAttribute[])typeOfEnum.GetField(s).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objStringAttributeAttribute != null && objStringAttributeAttribute.Length == 1)
                {
                    model.Description = objStringAttributeAttribute[0].Description;
                    model.IntValue = Convert.ToInt32(Enum.Format(typeOfEnum, Enum.Parse(typeOfEnum, s), "d"));
                    model.StringValue = s;
                }
                collection.Add(model);
            }
            return collection;
        }
    }

    public class EnumEntity
    {
        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public string Description { get; set; }
    }

}
