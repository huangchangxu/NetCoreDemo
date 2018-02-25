using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicQrcodeCreateResponse.
    /// </summary>
    public class AlipayMobilePublicQrcodeCreateResponse : AopResponse
    {
        /// <summary>
        /// 返回结果码，如200，标识成功
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobilePublicQrcodeCreateResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicQrcodeCreateResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 图片地址
        /// </summary>
        [XmlElement("code_img")]
        public string CodeImg { get; set; }

        /// <summary>
        /// 码过期时间，单位：秒
        /// </summary>
        [XmlElement("expire_second")]
        public long ExpireSecond { get; set; }

        /// <summary>
        /// 结果吗描述信息
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobilePublicQrcodeCreateResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicQrcodeCreateResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
