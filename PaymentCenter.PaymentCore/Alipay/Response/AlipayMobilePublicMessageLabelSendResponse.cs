using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicMessageLabelSendResponse.
    /// </summary>
    public class AlipayMobilePublicMessageLabelSendResponse : AopResponse
    {
        /// <summary>
        /// 结果码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobilePublicMessageLabelSendResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicMessageLabelSendResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 结果信息
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobilePublicMessageLabelSendResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicMessageLabelSendResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 消息ID
        /// </summary>
        [XmlElement("msg_id")]
        public string MsgId { get; set; }
    }
}
