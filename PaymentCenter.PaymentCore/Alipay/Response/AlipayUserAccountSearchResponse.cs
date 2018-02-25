using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayUserAccountSearchResponse.
    /// </summary>
    public class AlipayUserAccountSearchResponse : AopResponse
    {
        /// <summary>
        /// 支付宝用户账务明细信息
        /// </summary>
        [XmlArray("account_records")]
        [XmlArrayItem("account_record")]
        public List<AccountRecord> AccountRecords { get; set; }

        /// <summary>
        /// 总页面数
        /// </summary>
        [XmlElement("total_pages")]
        public long TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
