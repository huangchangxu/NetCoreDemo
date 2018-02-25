using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// 集合类型扩展
    /// </summary>
    public static class ArrayIistExtenstion
    {
        /// <summary>
        /// 扩展Dictionary  使其获取不异常
        /// </summary>
        /// <typeparam name="TInt"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key">key</param>
        /// <param name="defalutVal">获取异常 默认返回的值</param>
        /// <returns></returns>
        public static TOut TryGet<TInt, TOut>(this IDictionary<TInt, TOut> dic, TInt key, TOut defalutVal = default(TOut))
        {
            var flag = dic.TryGetValue(key, out TOut result);
            if (!flag)
            {
                result = defalutVal;
            }
            return result;
        }
        /// <summary>
        /// 将IDictiona 数据转换成Http表单提交数据格式
        /// key1=value1&key2=value2
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="ignoreEmpty">是否忽略空值</param>
        /// <returns></returns>
        public static string ToHttpFormData<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,bool ignoreEmpty=true)
        {
            string result = string.Empty;
            if (dictionary != null && dictionary.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (var item in dictionary)
                {
                    if (ignoreEmpty && (item.Value.IsNull() || item.Value.ToString().IsNullOrEmpty()))
                        continue;
                    list.Add($"{item.Key}={System.Web.HttpUtility.UrlEncode(item.Value.ToString())}");
                }
                result = string.Join("&", list);
            }
            return result;
        }
        /// <summary>
        /// 将实体集合 数据转换成Http表单提交数据格式
        /// key1=value1&key2=value2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToHttFormData<T>(this IList<T> list, bool ignoreEmpty = true) where T:class,new()
        {
            string result = string.Empty;
            if (list != null && list.Count > 0)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (var item in list)
                {
                    var dic = item.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(item) == null ? "" : p.GetValue(item).ToString());
                    foreach (var child in dic)
                    {
                        if (dic.ContainsKey(child.Key))
                        {
                            if (ignoreEmpty && child.Value.IsNullOrEmpty())
                                break;
                            dictionary[child.Key] = dictionary[child.Key] + "," + child.Value;
                        }
                        else
                        {
                            if (ignoreEmpty && child.Value.IsNullOrEmpty())
                                break;
                            dictionary.Add(child.Key, child.Value);
                        }
                    }
                }
                result = dictionary.ToHttpFormData();
            }
            return result;
        }
    }
}
