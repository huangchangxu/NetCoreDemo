using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayEcapiprodDataPutResponse.
    /// </summary>
    public class AlipayEcapiprodDataPutResponse : AopResponse
    {
        /// <summary>
        /// 数据版本
        /// </summary>
        [XmlElement("data_version")]
        public string DataVersion { get; set; }
    }
}
