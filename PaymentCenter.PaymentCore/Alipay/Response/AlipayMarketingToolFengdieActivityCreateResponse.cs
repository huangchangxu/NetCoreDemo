using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMarketingToolFengdieActivityCreateResponse.
    /// </summary>
    public class AlipayMarketingToolFengdieActivityCreateResponse : AopResponse
    {
        /// <summary>
        /// H5应用的唯一id
        /// </summary>
        [XmlElement("activity_id")]
        public long ActivityId { get; set; }
    }
}
