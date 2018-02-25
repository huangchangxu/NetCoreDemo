using System;
using System.Xml.Serialization;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayInsSceneCouponReceiveResponse.
    /// </summary>
    public class AlipayInsSceneCouponReceiveResponse : AopResponse
    {
        /// <summary>
        /// 保单凭证号;商户生成的外部投保业务号不传时则必传
        /// </summary>
        [XmlElement("policy_no")]
        public string PolicyNo { get; set; }

        /// <summary>
        /// 保险产品
        /// </summary>
        [XmlElement("product")]
        public InsProduct Product { get; set; }
    }
}
