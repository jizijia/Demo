using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System.Data;

namespace Demo.Core.Common
{
    public static class ListExtentions
    {
        /// <summary>
        /// 转换后, 将重新创建List(扩展)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<object> ToObjectList<T>(this IList<T> value)
        {
            List<object> result = new List<object>();
            foreach (T item in value)
            {
                result.Add(item);
            }
            if (result.Count > 0)
            {
                return result;
            }
            return null;
        } 

        /// <summary>
        /// 转成Json字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToJson(this IList<object> list)
        {
            return JsonConvert.SerializeObject(list);
        }


        public static DataTable ToDataTable<T>(IEnumerable<T> value, bool isContainData = true)
        {

            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列 
            PropertyInfo[] properties = type.GetProperties();
            PropertyInfo p;
            for (int i = 0; i < properties.Length; i++)
            {
                p = properties[i];
                if (p.PropertyType.Name != "Nullable`1")
                {
                    pList.Add(p);
                    dt.Columns.Add(p.Name, p.PropertyType);
                }
                else
                {
                    if (p.PropertyType.FullName.Contains("Int64"))
                    {
                        pList.Add(p);
                        dt.Columns.Add(p.Name, typeof(Int64));
                    }
                    else if (p.PropertyType.FullName.Contains("Decimal"))
                    {
                        pList.Add(p);
                        dt.Columns.Add(p.Name, typeof(Decimal));
                    }
                    else if (p.PropertyType.FullName.Contains("DateTime"))
                    {
                        pList.Add(p);
                        dt.Columns.Add(p.Name, typeof(DateTime));
                    }
                    else
                    {
                        dt.Columns.Add(p.Name, typeof(Object));
                    }
                }
            }
            if (isContainData)
            {
                foreach (var item in value)
                {
                    //创建一个DataRow实例
                    DataRow row = dt.NewRow();
                    //给row 赋值 
                    foreach (PropertyInfo tempP in pList)
                    {
                        row[tempP.Name] = IsNullCellValue(tempP.GetValue(item, null));
                    }
                    //加入到DataTable
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 判断Table中的Cell值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object IsNullCellValue(object obj)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }
            return obj;
        }


        /// <summary>
        /// 遍历(扩展)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            if (instance == null)
            {
                return;
            }
            foreach (T item in instance)
            {
                action(item);
            }
        }

        /// <summary>
        /// 遍历(扩展)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> instance, Action<T, int> action)
        {
            int index = 0;
            foreach (T item in instance)
                action(item, index++);
        }

    }
}
