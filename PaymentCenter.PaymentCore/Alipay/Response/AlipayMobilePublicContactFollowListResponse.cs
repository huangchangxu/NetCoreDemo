using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicContactFollowListResponse.
    /// </summary>
    public class AlipayMobilePublicContactFollowListResponse : AopResponse
    {
        /// <summary>
        /// 返回结果码，如200，标识成功
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobilePublicContactFollowListResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicContactFollowListResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 联系人关注者列表
        /// </summary>
        [XmlArray("contact_follow_list")]
        [XmlArrayItem("string")]
        public List<string> ContactFollowList { get; set; }
    }
}
