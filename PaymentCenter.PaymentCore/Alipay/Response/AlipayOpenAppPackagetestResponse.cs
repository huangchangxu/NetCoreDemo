using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayOpenAppPackagetestResponse.
    /// </summary>
    public class AlipayOpenAppPackagetestResponse : AopResponse
    {
        /// <summary>
        /// testtest
        /// </summary>
        [XmlElement("testtesttesttest")]
        public string Testtesttesttest { get; set; }
    }
}
