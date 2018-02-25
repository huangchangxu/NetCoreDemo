using System;
using System.Xml.Serialization;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMicropayOrderFreezeResponse.
    /// </summary>
    public class AlipayMicropayOrderFreezeResponse : AopResponse
    {
        /// <summary>
        /// 冻结订单详情
        /// </summary>
        [XmlElement("micro_pay_order_detail")]
        public MicroPayOrderDetail MicroPayOrderDetail { get; set; }
    }
}
