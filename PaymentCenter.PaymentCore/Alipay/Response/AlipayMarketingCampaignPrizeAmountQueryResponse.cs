using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMarketingCampaignPrizeAmountQueryResponse.
    /// </summary>
    public class AlipayMarketingCampaignPrizeAmountQueryResponse : AopResponse
    {
        /// <summary>
        /// 奖品剩余数量，数值
        /// </summary>
        [XmlElement("remain_amount")]
        public string RemainAmount { get; set; }
    }
}
