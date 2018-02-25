using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicTemplateMessageQueryResponse.
    /// </summary>
    public class AlipayMobilePublicTemplateMessageQueryResponse : AopResponse
    {
        /// <summary>
        /// 结果值
        /// </summary>
        [XmlElement("result")]
        public string Result { get; set; }
    }
}
