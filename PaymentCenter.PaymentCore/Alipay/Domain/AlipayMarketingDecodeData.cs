using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayMarketingDecodeData Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayMarketingDecodeData : AopObject
    {
        /// <summary>
        /// 钱包二维码码值
        /// </summary>
        [XmlElement("code")]
        public string Code { get; set; }
    }
}
