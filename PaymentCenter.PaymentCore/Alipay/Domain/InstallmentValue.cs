using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// InstallmentValue Data Structure.
    /// </summary>
    [Serializable]
    public class InstallmentValue : AopObject
    {
        /// <summary>
        /// 分段值
        /// </summary>
        [XmlArray("installment_values")]
        [XmlArrayItem("installment_meta_info")]
        public List<InstallmentMetaInfo> InstallmentValues { get; set; }
    }
}
