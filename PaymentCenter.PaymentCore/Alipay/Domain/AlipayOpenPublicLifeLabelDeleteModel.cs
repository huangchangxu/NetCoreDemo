using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayOpenPublicLifeLabelDeleteModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayOpenPublicLifeLabelDeleteModel : AopObject
    {
        /// <summary>
        /// 标签id
        /// </summary>
        [XmlElement("label_id")]
        public string LabelId { get; set; }
    }
}
