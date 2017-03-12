using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace Demo.Core.Common
{
    public static class DataTableExtension
    {

        public static string ToJson(this DataTable dataTable)
        {
            return JsonHelper.ToJson(dataTable);
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt)
            where T : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取T的类型实例 反射的入口
            Type t = typeof(T);
            //获得T 的所有的Public 属性 并找出T属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p =>
            {
                if (dt.Columns.IndexOf(p.Name) != -1)
                    prlist.Add(p);
            });
            //创建返回的集合
            List<T> oblist = new List<T>();
            T ob = null;
            Type valType = null;
            foreach (DataRow row in dt.Rows)
            {
                //创建T的实例
                ob = new T();
                //找到对应的数据 并赋值
                foreach (PropertyInfo p in prlist)
                {
                    if (row[p.Name] != DBNull.Value &&
                       IsExcept(row[p.Name].GetType()))
                    {
                        if (p.CanWrite && p.Name != "EntityKey")
                        {
                            if (ObjectHelper.IsNullableType(p.PropertyType))
                            {
                                valType = Nullable.GetUnderlyingType(p.PropertyType);
                                if (valType.Name == "Decimal")
                                {
                                    p.SetValue(ob, ObjectHelper.ToDecimal(row[p.Name]), null);
                                }
                                else
                                {
                                    p.SetValue(ob, row[p.Name], null);
                                }
                            }
                            else
                            {
                                p.SetValue(ob, row[p.Name], null);
                            }
                        }
                    }
                }
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
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
        /// 是否排除
        /// </summary>
        /// <returns></returns>
        public static bool IsExcept(Type type)
        {
            if (ContractConst.EXCEPT_TYPE.Contains("|" + type.Name + "|"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否排除
        /// </summary>
        /// <returns></returns>
        private static bool IsContain(string typeName)
        {
            if (ContractConst.EXCEPT_TYPE.Contains("|" + typeName + "|"))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 转换DataTable格式，以支持序列化
        /// 解决 “序列化类型为“System.Reflection.RuntimeModule”的对象时检测到循环引用”的异常
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SerializeEnable(this DataTable dataTable)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dataTable.Rows)//每一行信息，新建一个Dictionary<string,object>,将该行的每列信息加入到字典
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dataTable.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return list;
        }
    }
}
