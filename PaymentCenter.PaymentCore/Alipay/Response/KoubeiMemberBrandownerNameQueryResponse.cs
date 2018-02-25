using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// KoubeiMemberBrandownerNameQueryResponse.
    /// </summary>
    public class KoubeiMemberBrandownerNameQueryResponse : AopResponse
    {
        /// <summary>
        /// 品牌商名称
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
