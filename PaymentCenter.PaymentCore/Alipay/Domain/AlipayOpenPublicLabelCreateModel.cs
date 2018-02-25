using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayOpenPublicLabelCreateModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayOpenPublicLabelCreateModel : AopObject
    {
        /// <summary>
        /// 标签名
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
