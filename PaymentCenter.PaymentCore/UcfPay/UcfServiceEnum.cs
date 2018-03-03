using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.UcfPay
{
    /// <summary>
    /// 服务枚举
    /// </summary>
    public enum UcfServiceEnum
    {
        /// <summary>
        /// pc认证支付创建支付订单
        /// </summary>
        MOBILE_CERTPAY_PC_ORDER_CREATE,
        /// <summary>
        /// SDK认证支付创建支付订单
        /// </summary>
        MOBILE_CERTPAY_ORDER_CREATE,
        /// <summary>
        /// 支付查询
        /// </summary>
        MOBILE_CERTPAY_QUERYORDERSTATUS
    }
}
