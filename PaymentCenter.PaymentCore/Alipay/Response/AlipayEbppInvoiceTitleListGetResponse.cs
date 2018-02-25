using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayEbppInvoiceTitleListGetResponse.
    /// </summary>
    public class AlipayEbppInvoiceTitleListGetResponse : AopResponse
    {
        /// <summary>
        /// 抬头列表
        /// </summary>
        [XmlArray("title_list")]
        [XmlArrayItem("invoice_title_model")]
        public List<InvoiceTitleModel> TitleList { get; set; }
    }
}
