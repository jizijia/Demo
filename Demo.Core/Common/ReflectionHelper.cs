using System;
using System.Collections.Generic;
using System.Reflection;
using Demo.Core.Caching;

namespace Demo.Core.Common
{
    public static class ReflectionHelper
    {
        private readonly static string PropertyCollectionPreKey = "Entity_Properties_";
        //private readonly static Type _includeBaseAttributeType = typeof (SerializeIncludingBaseAttribute);
        private static readonly Type _nonSerializableAttributeType = typeof(NonSerializedAttribute);
        private static readonly Type _nullableType = typeof(Nullable<>);
        private const int _cacheMinuters = 180;

        public static List<PropertyInfo> GetSerializableFields(Type type)
        {
            var fields = new List<PropertyInfo>(10);
            //fields.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
            fields.AddRange(type.GetProperties());
            //RemoveNonSerializableFields(fields);
            //if (type.BaseType != null && type.GetCustomAttributes(_includeBaseAttributeType, false).Length > 0)
            //{
            //    fields.AddRange(GetSerializableFields(type.BaseType));
            //}
            return fields;
        }

        //private static void RemoveNonSerializableFields(IList<PropertyInfo> fields)
        //{
        //    for(int i = 0; i < fields.Count; ++i)
        //    {
        //        if (!ShouldSerializeField(fields[i]))
        //        {
        //            fields.RemoveAt(i--);
        //        }
        //    }
        //}

        public static bool ShouldSerializeField(ICustomAttributeProvider field)
        {
            return field.GetCustomAttributes(_nonSerializableAttributeType, true).Length <= 0;
        }

        public static PropertyInfo FindProp(Type type, string name)
        {
            var field = FindFieldThroughoutHierarchy(type, name);
            if (field == null)
            {
                throw new ArgumentException(type.FullName + " doesn't have a field named: " + name);
            }
            return field;
        }
        public static PropertyInfo FindFieldThroughoutHierarchy(Type type, string name)
        {
            //var field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var field = type.GetProperty(name);
            //if (field == null && type.GetCustomAttributes(_includeBaseAttributeType, false).Length > 0)
            //{
            //    field = FindFieldThroughoutHierarchy(type.BaseType, name);
            //}
            return field;
        }

        //private static string makeFileName(string fieldName)
        //{
        //    if (fieldName[0] != '<')
        //    {
        //        return ('<' + fieldName + ">k__BackingField");
        //    }
        //    return fieldName;
        //}

        public static object GetValue(PropertyInfo field, object @object)
        {
            var value = field.GetValue(@object, null);
            //过滤掉null，目前只过滤string类型
            if (field.PropertyType.FullName.Equals("System.String"))
            {
                if (value == null)
                {
                    value = "";
                }
            }
            var isEnum = field.PropertyType.IsEnum;
            if (field.PropertyType.IsGenericType && field.PropertyType.GetGenericTypeDefinition() == _nullableType && value != null)
            {
                isEnum = Nullable.GetUnderlyingType(field.PropertyType).IsEnum;
            }
            return isEnum && value != null ? int.Parse(((Enum)value).ToString("d")) : value;
        }
        public static PropertyInfo GetProperty(Type type, string propertyName)
        {
            MemoryCacheManager memoryCacheManager = new MemoryCacheManager();
            Dictionary<string, PropertyInfo> propertyDictionary = null;
            string cachingKey = PropertyCollectionPreKey + type.Name;
            if (!memoryCacheManager.IsSet(cachingKey))
            {
                propertyDictionary = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (PropertyInfo property in propertyInfos)
                {
                    propertyDictionary.Add(property.Name, property);
                }
                memoryCacheManager.Set(cachingKey, propertyDictionary, 60);
            }
            propertyDictionary = memoryCacheManager.Get<Dictionary<string, PropertyInfo>>(cachingKey);

            if (propertyDictionary.ContainsKey(propertyName))
            {
                return propertyDictionary[propertyName];
            }
            return null;
        }
        /// <summary>
        /// 获取对象的属性对象
        /// 本方法提供缓存功能, 提高性能
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<TEntity>(string propertyName)
        {
            MemoryCacheManager memoryCacheManager = new MemoryCacheManager();
            Dictionary<string, PropertyInfo> propertyDictionary = null;
            string cachingKey = PropertyCollectionPreKey + typeof(TEntity).Name;
            if (!memoryCacheManager.IsSet(cachingKey))
            {
                propertyDictionary = new Dictionary<string, PropertyInfo>();
                Type type = typeof(TEntity);
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (PropertyInfo property in propertyInfos)
                {
                    propertyDictionary.Add(property.Name, property);
                }
                memoryCacheManager.Set(cachingKey, propertyDictionary, 60);
            }
            propertyDictionary = memoryCacheManager.Get<Dictionary<string, PropertyInfo>>(cachingKey);

            if (propertyDictionary.ContainsKey(propertyName))
            {
                return propertyDictionary[propertyName];
            }
            return null;
        }

        /// <summary>
        /// 获取类型属性
        /// 增加缓存机制
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertys(object target)
        {
            MemoryCacheManager memoryCacheManager = new MemoryCacheManager(); 
            string cachingKey = PropertyCollectionPreKey + target.GetType().Name;
            if (!memoryCacheManager.IsSet(cachingKey))
            { 
                Type type = target.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties();
                memoryCacheManager.Set(cachingKey, propertyInfos, _cacheMinuters);
            }
            return memoryCacheManager.Get<PropertyInfo[]>(cachingKey); 
        }

        /// <summary>
        /// 将对象实例的属性和值放入IDictionary对象中 
        /// </summary>
        /// <param name="data">要被转换的对象</param>
        /// <returns>被转换后的IDictionary,存储这各个属性和值</returns>
        public static IDictionary<string, object> ToDictionary(this object data)
        {
            if (data is IDictionary<string, object>)
                return data as IDictionary<string, object>;

            var attr = BindingFlags.Public | BindingFlags.Instance;
            var dict = new Dictionary<string, object>();
            foreach (var property in data.GetType().GetProperties(attr))
            {
                if (property.CanRead)
                {
                    dict.Add(property.Name, property.GetValue(data, null));
                }
            }
            return dict;
        }
    }
}
