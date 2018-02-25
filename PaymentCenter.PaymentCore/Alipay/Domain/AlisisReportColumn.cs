using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlisisReportColumn Data Structure.
    /// </summary>
    [Serializable]
    public class AlisisReportColumn : AopObject
    {
        /// <summary>
        /// 列别名
        /// </summary>
        [XmlElement("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// 列值
        /// </summary>
        [XmlElement("data")]
        public string Data { get; set; }
    }
}
