using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayOpenPublicLifeMsgSendResponse.
    /// </summary>
    public class AlipayOpenPublicLifeMsgSendResponse : AopResponse
    {
        /// <summary>
        /// 生活号消息唯一标识
        /// </summary>
        [XmlElement("alipay_msg_id")]
        public string AlipayMsgId { get; set; }
    }
}
