using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicMenuUserQueryResponse.
    /// </summary>
    public class AlipayMobilePublicMenuUserQueryResponse : AopResponse
    {
        /// <summary>
        /// 结果码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobilePublicMenuUserQueryResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicMenuUserQueryResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 菜单唯一标识
        /// </summary>
        [XmlElement("menu_key")]
        public string MenuKey { get; set; }

        /// <summary>
        /// 结果码描述
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobilePublicMenuUserQueryResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicMenuUserQueryResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
