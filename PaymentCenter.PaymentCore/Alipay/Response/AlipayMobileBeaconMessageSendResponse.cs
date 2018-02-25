using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobileBeaconMessageSendResponse.
    /// </summary>
    public class AlipayMobileBeaconMessageSendResponse : AopResponse
    {
        /// <summary>
        /// 操作返回码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobileBeaconMessageSendResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconMessageSendResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 操作提示文案
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobileBeaconMessageSendResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconMessageSendResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
