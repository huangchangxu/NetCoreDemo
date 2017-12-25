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
    }
}
