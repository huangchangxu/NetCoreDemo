using System;
using System.Xml.Serialization;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMicropayOrderGetResponse.
    /// </summary>
    public class AlipayMicropayOrderGetResponse : AopResponse
    {
        /// <summary>
        /// 冻结订单详情
        /// </summary>
        [XmlElement("micro_pay_order_detail")]
        public MicroPayOrderDetail MicroPayOrderDetail { get; set; }
    }
}
