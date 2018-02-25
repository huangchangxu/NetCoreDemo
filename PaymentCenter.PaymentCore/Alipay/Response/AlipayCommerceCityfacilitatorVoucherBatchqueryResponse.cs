using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayCommerceCityfacilitatorVoucherBatchqueryResponse.
    /// </summary>
    public class AlipayCommerceCityfacilitatorVoucherBatchqueryResponse : AopResponse
    {
        /// <summary>
        /// 查询到的订单信息列表
        /// </summary>
        [XmlArray("tickets")]
        [XmlArrayItem("ticket_detail_info")]
        public List<TicketDetailInfo> Tickets { get; set; }
    }
}
