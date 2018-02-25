using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayPointBudgetGetResponse.
    /// </summary>
    public class AlipayPointBudgetGetResponse : AopResponse
    {
        /// <summary>
        /// 还可以发放的集分宝个数
        /// </summary>
        [XmlElement("budget_amount")]
        public long BudgetAmount { get; set; }
    }
}
