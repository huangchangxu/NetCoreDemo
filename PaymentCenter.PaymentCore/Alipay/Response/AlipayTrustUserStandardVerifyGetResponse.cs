using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayTrustUserStandardVerifyGetResponse.
    /// </summary>
    public class AlipayTrustUserStandardVerifyGetResponse : AopResponse
    {
        /// <summary>
        /// 详见说明文档和代码样例
        /// </summary>
        [XmlElement("verify_info")]
        public string VerifyInfo { get; set; }
    }
}
