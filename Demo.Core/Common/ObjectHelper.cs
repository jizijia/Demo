using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Collections.ObjectModel;

namespace Demo.Core.Common
{
    public static class ObjectHelper
    {
        private static readonly object Locker = new object();

        public static bool IsNone(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }
        public static string Clear(this string source, string defaultValue = "")
        {
            if (source.IsNone())
                return defaultValue;

            return source.Trim();
        }

        public static string GetUniqueTime()
        {
            lock (Locker)
            {
                return DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>
        ///   当obj为空或是转换错误范围默认返回值, 转换成功返回转换后的值
        /// </returns>
        public static string ToString(object obj, string defaultValue = "")
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return obj.ToString();
        }


        #region 数据转换为Int类型
        /// <summary>
        /// 将Object类型的数据转换为Double类型
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>
        ///     当obj为空或是转换错误范围默认返回值, 转换成功返回转换后的值
        /// </returns>
        public static int ToInt(object obj, int defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return ToInt(obj.ToString(), defaultValue);
        }

        /// <summary>
        /// 将String类型的数据转换为Double类型
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>
        ///     当obj为空或是转换错误范围默认返回值, 转换成功返回转换后的值
        /// </returns>
        public static int ToInt(string obj, int defaultValue = 0)
        {
            int result = 0;
            if (Int32.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return defaultValue;
        }
        #endregion

        #region 数据转换为Double类型
        /// <summary>
        /// 将Object类型的数据转换为Double类型
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>
        ///     当obj为空或是转换错误范围默认返回值, 转换成功返回转换后的值
        /// </returns>
        public static double? ToDoubleOrNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            double result = 0;
            if (Double.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        public static double ToDouble(object obj, double defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return ToDouble(obj.ToString(), defaultValue);
        }

        /// <summary>
        /// 将String类型的数据转换为Double类型
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>
        ///     当obj为空或是转换错误范围默认返回值, 转换成功返回转换后的值
        /// </returns>
        public static double ToDouble(string obj, double defaultValue = 0)
        {
            double result = 0;
            if (Double.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return defaultValue;
        }
        #endregion

        #region 数据转换为decimal类型
        /// <summary>
        /// 将Object类型的数据转换为Decimal类型
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>
        ///     当obj为空或是转换错误范围默认返回值, 转换成功返回转换后的值
        /// </returns>
        public static decimal ToDecimal(object obj, decimal defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return ToDecimal(obj.ToString(), defaultValue);
        }

        /// <summary>
        /// 将String类型的数据转换为Decimal类型
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>
        ///     当obj为空或是转换错误范围默认返回值, 转换成功返回转换后的值
        /// </returns>
        public static decimal ToDecimal(string obj, decimal defaultValue = 0)
        {
            decimal result = 0;
            if (Decimal.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return defaultValue;
        }
        #endregion


        public static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// 将枚举转换成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();
            list.Each(c =>
            {
                observableCollection.Add(c);
            });
            return observableCollection;
        }

        /// <summary>
        /// 对两个类之间的相同属性的值进行同步
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static TResult TransformTo<TResult>(this object dataSource, params string[] exceptPropertyName)
        {
            if (dataSource == null)
                return default(TResult);
            IDictionary<string, object> attrs = dataSource.ToDictionary();
            Type type = typeof(TResult);
            TResult result = System.Activator.CreateInstance<TResult>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if (attrs.ContainsKey(item.Name) && !exceptPropertyName.Contains(item.Name))
                {
                    item.SetValue(result, attrs[item.Name], null);
                }
            }
            return result;
        }


        public static TTarget TransformTo<TTarget>(this object dataSource, TTarget result, bool isExceptBaseProperty = false)
        {
            if (dataSource == null)
                return default(TTarget);
            IDictionary<string, object> attrs = dataSource.ToDictionary();
            Type type = typeof(TTarget);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo item in properties)
            {
                if (isExceptBaseProperty && ContractConst.EXCEPT_TYPE.Contains($"|{item.Name}|"))
                {
                    continue;
                }
                if (attrs.ContainsKey(item.Name))
                {
                    if (attrs[item.Name] != null && item.PropertyType.FullName != attrs[item.Name].GetType().FullName)
                    {
                        continue;
                    }
                    item.SetValue(result, attrs[item.Name], null);
                }
            }
            return result;
        }

        public static List<T> CloneList<T>(List<T> list, params string[] exceptPropertyName)
        {
            List<T> newList = new List<T>();
            foreach (var item in list)
            {
                newList.Add(item.TransformTo<T>(exceptPropertyName));
            }
            return newList;
        }


        public static DateTime DataTime2
        {
            get
            {
                return new DateTime(1753, 1, 1);
            }
        }
    }
}
