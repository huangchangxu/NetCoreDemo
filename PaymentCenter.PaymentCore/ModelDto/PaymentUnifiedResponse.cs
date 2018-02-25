using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore
{
    /// <summary>
    /// 支付统一执行结果输出
    /// </summary>
    public sealed class PaymentUnifiedResponse<T>
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 响应状态码
        /// </summary>
        public long ReturnCode { get; set; }
        /// <summary>
        /// 响应状态描述
        /// </summary>
        public string ReturnMsg { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public T ReturnData { get; set; } 
    }
}
