using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayMarketingCdpAdvertiseReportQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayMarketingCdpAdvertiseReportQueryModel : AopObject
    {
        /// <summary>
        /// 广告Id，唯一标识一条广告
        /// </summary>
        [XmlElement("ad_id")]
        public string AdId { get; set; }
    }
}
