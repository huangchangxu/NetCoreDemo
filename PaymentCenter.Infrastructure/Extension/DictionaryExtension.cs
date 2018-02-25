using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// 字典扩展
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// 添加/更新字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sortedDictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate<TKey, TValue>(this SortedDictionary<TKey, TValue> sortedDictionary, TKey key, TValue value)
        {
            if (sortedDictionary.ContainsKey(key))
                sortedDictionary[key] = value;
            else
                sortedDictionary.Add(key, value);
        }
        /// <summary>
        /// 添加/更新字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sortedDictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> sortedDictionary, TKey key, TValue value)
        {
            if (sortedDictionary.ContainsKey(key))
                sortedDictionary[key] = value;
            else
                sortedDictionary.Add(key, value);
        }
       
    }
}
