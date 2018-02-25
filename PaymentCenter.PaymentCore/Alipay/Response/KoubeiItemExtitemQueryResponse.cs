using System;
using System.Xml.Serialization;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// KoubeiItemExtitemQueryResponse.
    /// </summary>
    public class KoubeiItemExtitemQueryResponse : AopResponse
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        [XmlElement("extitem")]
        public ExtItem Extitem { get; set; }
    }
}
