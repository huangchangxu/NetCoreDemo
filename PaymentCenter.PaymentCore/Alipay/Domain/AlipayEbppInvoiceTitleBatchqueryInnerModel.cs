using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayEbppInvoiceTitleBatchqueryInnerModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEbppInvoiceTitleBatchqueryInnerModel : AopObject
    {
        /// <summary>
        /// 抬头所属支付宝用户id
        /// </summary>
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
