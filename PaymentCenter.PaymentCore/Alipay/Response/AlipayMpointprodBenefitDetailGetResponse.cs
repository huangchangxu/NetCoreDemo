using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMpointprodBenefitDetailGetResponse.
    /// </summary>
    public class AlipayMpointprodBenefitDetailGetResponse : AopResponse
    {
        /// <summary>
        /// 权益详情列表
        /// </summary>
        [XmlArray("benefit_infos")]
        [XmlArrayItem("benefit_info")]
        public List<BenefitInfo> BenefitInfos { get; set; }

        /// <summary>
        /// 响应码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMpointprodBenefitDetailGetResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMpointprodBenefitDetailGetResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 响应描述
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMpointprodBenefitDetailGetResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMpointprodBenefitDetailGetResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
