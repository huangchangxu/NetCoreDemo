using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayMarketingExtData Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayMarketingExtData : AopObject
    {
        /// <summary>
        /// 复杂模型
        /// </summary>
        [XmlElement("lbs_info")]
        public AlipayMarketingIbsInfo LbsInfo { get; set; }

        /// <summary>
        /// 外部uid
        /// </summary>
        [XmlElement("out_user_id")]
        public string OutUserId { get; set; }
    }
}
