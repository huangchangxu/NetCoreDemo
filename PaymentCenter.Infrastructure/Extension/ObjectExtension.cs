using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// Object扩展
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 判断是否不为NULL
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return (obj != null);
        }
        /// <summary>
        /// 判断是否为NULL
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return(obj==null);
        }
        /// <summary>
        /// 扩展Dictionary  使其获取不异常
        /// </summary>
        /// <typeparam name="TInt"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key">key</param>
        /// <param name="defalutVal">获取异常 默认返回的值</param>
        /// <returns></returns>
        public static TOut TryGet<TInt, TOut>(this Dictionary<TInt, TOut> dic, TInt key, TOut defalutVal = default(TOut))
        {
            var flag = dic.TryGetValue(key, out TOut result);
            if (!flag)
            {
                result = defalutVal;
            }
            return result;
        }
    }
}
