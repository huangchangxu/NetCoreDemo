using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// ZhimaCustomerCertificationInitializeResponse.
    /// </summary>
    public class ZhimaCustomerCertificationInitializeResponse : AopResponse
    {
        /// <summary>
        /// 本次认证的唯一标识,商户需要记录
        /// </summary>
        [XmlElement("biz_no")]
        public string BizNo { get; set; }
    }
}
