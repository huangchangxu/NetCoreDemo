using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.Wechat
{
    /// <summary>
    /// 微信支付类型
    /// </summary>
    public enum TradeType
    {
        /// <summary>
        /// 扫码支付（模式二）
        /// </summary>
        NATIVE,
        /// <summary>
        /// APP支付
        /// </summary>
        APP,
        /// <summary>
        /// 公众号支付&小程序支付
        /// </summary>
        JSAPI,
        /// <summary>
        /// H5支付
        /// </summary>
        MWEB
    }
}
