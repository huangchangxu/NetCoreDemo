using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayInsSceneApplicationCancelModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayInsSceneApplicationCancelModel : AopObject
    {
        /// <summary>
        /// 投保订单号
        /// </summary>
        [XmlElement("application_no")]
        public string ApplicationNo { get; set; }
    }
}
