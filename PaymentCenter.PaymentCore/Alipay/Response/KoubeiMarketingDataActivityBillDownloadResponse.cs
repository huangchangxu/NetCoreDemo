using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// KoubeiMarketingDataActivityBillDownloadResponse.
    /// </summary>
    public class KoubeiMarketingDataActivityBillDownloadResponse : AopResponse
    {
        /// <summary>
        /// 账单下载地址
        /// </summary>
        [XmlElement("url")]
        public string Url { get; set; }
    }
}
