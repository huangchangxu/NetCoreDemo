using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicAccountAddResponse.
    /// </summary>
    public class AlipayMobilePublicAccountAddResponse : AopResponse
    {
        /// <summary>
        /// 账户添加成功，在支付宝与其对应的协议号。如果账户重复添加，接口保证幂等依然视为添加成功，返回此前该账户在支付宝对应的协议号。其他异常该字段不存在。
        /// </summary>
        [XmlElement("agreement_id")]
        public string AgreementId { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobilePublicAccountAddResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicAccountAddResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 成功
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobilePublicAccountAddResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicAccountAddResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
