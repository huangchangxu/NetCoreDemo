using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMicropayOrderFreezepayurlGetResponse.
    /// </summary>
    public class AlipayMicropayOrderFreezepayurlGetResponse : AopResponse
    {
        /// <summary>
        /// 支付冻结金的地址
        /// </summary>
        [XmlElement("pay_freeze_url")]
        public string PayFreezeUrl { get; set; }
    }
}
