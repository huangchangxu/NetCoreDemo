using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayPlatformOpenidGetResponse.
    /// </summary>
    public class AlipayPlatformOpenidGetResponse : AopResponse
    {
        /// <summary>
        /// 结果码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayPlatformOpenidGetResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayPlatformOpenidGetResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// id字典，key为userId和老的openId，value为新的openId
        /// </summary>
        [XmlElement("dict")]
        public string Dict { get; set; }

        /// <summary>
        /// 结果描述
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayPlatformOpenidGetResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayPlatformOpenidGetResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
