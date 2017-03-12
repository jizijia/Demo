using System;

namespace Demo.Core.Caching
{
    /// <summary>
    /// �������ӿ�
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary> 
        /// ��ȡ��������ָ���ļ��������ֵ��
        /// </summary>
        /// <typeparam name="T">����.</typeparam>
        /// <param name="key">��.</param>
        /// <returns>��ָ���ļ�������ֵ.</returns>
        T Get<T>(string key);

        /// <summary> 
        /// ��ָ���ļ��Ͷ�����ӵ������С�
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="data">����ʵ��</param>
        /// <param name="cacheTime">����ʱ��(����)</param>
        void Set(string key, object data, int cacheTime);
        
        /// <summary>
        /// ��ȡһ��ֵ����ֵָʾ�Ƿ���ָ���ļ�������ֵ.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        bool IsSet(string key);

        /// <summary>
        /// �ӻ������Ƴ�ָ���ļ���ֵ
        /// </summary>
        /// <param name="key">/key</param>
        void Remove(string key);

        /// <summary>
        /// ͨ��ƥ���Ƴ�����
        /// </summary>
        /// <param name="pattern">pattern</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// ��ջ�������
        /// </summary>
        void Clear();
    }
}
