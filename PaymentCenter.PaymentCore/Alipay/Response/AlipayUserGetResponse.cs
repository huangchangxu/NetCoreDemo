using System;
using System.Xml.Serialization;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayUserGetResponse.
    /// </summary>
    public class AlipayUserGetResponse : AopResponse
    {
        /// <summary>
        /// 支付宝用户信息
        /// </summary>
        [XmlElement("alipay_user_detail")]
        public AlipayUserDetail AlipayUserDetail { get; set; }
    }
}
