using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.UcfPay
{
    /// <summary>
    /// 先锋支付基础配置
    /// </summary>
    public class UcfPayBase
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public UcfServiceEnum Service { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string Mer_Id { get; set; }
        /// <summary>
        /// RSA公钥
        /// </summary>
        public string Mer_RsaKey { get; set; }
        /// <summary>
        /// 网关地址
        /// </summary>
        public string UCF_Gateway_Url { get; set; } 
        /// <summary>
        /// 回跳URl
        /// </summary>
        public static String Return_Url { get; set; }
        /// <summary>
        /// 异步回调URL
        /// </summary>
        public static String Notice_Url { get; set; }
        /// <summary>
        /// 先锋退款异步通知
        /// </summary>
        public static string Refund_Notice_Url { get; set; }

    }
}
