using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipayMarketingDataDashboardApplyModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayMarketingDataDashboardApplyModel : AopObject
    {
        /// <summary>
        /// 仪表盘ID列表
        /// </summary>
        [XmlArray("dashboard_ids")]
        [XmlArrayItem("string")]
        public List<string> DashboardIds { get; set; }
    }
}
