using System;

namespace Demo.Core.Caching
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary> 
        /// 获取或设置与指定的键相关联的值。
        /// </summary>
        /// <typeparam name="T">泛型.</typeparam>
        /// <param name="key">键.</param>
        /// <returns>与指定的键关联的值.</returns>
        T Get<T>(string key);

        /// <summary> 
        /// 将指定的键和对象添加到缓存中。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">对象实例</param>
        /// <param name="cacheTime">缓存时长(分钟)</param>
        void Set(string key, object data, int cacheTime);
        
        /// <summary>
        /// 获取一个值，该值指示是否与指定的键关联的值.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        bool IsSet(string key);

        /// <summary>
        /// 从缓存中移除指定的键的值
        /// </summary>
        /// <param name="key">/key</param>
        void Remove(string key);

        /// <summary>
        /// 通过匹配移除缓存
        /// </summary>
        /// <param name="pattern">pattern</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清空缓存数据
        /// </summary>
        void Clear();
    }
}
