using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AliTrustScore Data Structure.
    /// </summary>
    [Serializable]
    public class AliTrustScore : AopObject
    {
        /// <summary>
        /// 芝麻分
        /// </summary>
        [XmlElement("score")]
        public long Score { get; set; }
    }
}
