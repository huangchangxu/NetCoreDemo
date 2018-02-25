using System;
using System.Xml.Serialization;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicFollowListResponse.
    /// </summary>
    public class AlipayMobilePublicFollowListResponse : AopResponse
    {
        /// <summary>
        /// 返回结果码，如200，标识成功
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobilePublicFollowListResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicFollowListResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 当前组的值
        /// </summary>
        [XmlElement("count")]
        public string Count { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        [XmlElement("data")]
        public Data Data { get; set; }

        /// <summary>
        /// 与nextUserId对应，标准Alipay UserId
        /// </summary>
        [XmlElement("next_alipay_user_id")]
        public string NextAlipayUserId { get; set; }

        /// <summary>
        /// 查询分组的userid
        /// </summary>
        [XmlElement("next_user_id")]
        public string NextUserId { get; set; }
    }
}
