using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayEcoMycarPromoTicketPushResponse.
    /// </summary>
    public class AlipayEcoMycarPromoTicketPushResponse : AopResponse
    {
        /// <summary>
        /// 处理结果返回码
        /// </summary>
        [XmlElement("sp_apply_no")]
        public string SpApplyNo { get; set; }
    }
}
