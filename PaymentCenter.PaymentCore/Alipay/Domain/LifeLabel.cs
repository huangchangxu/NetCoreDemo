using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// LifeLabel Data Structure.
    /// </summary>
    [Serializable]
    public class LifeLabel : AopObject
    {
        /// <summary>
        /// 标签值类型
        /// </summary>
        [XmlElement("data_type")]
        public string DataType { get; set; }

        /// <summary>
        /// 标签id
        /// </summary>
        [XmlElement("label_id")]
        public long LabelId { get; set; }

        /// <summary>
        /// 标签名
        /// </summary>
        [XmlElement("label_name")]
        public string LabelName { get; set; }
    }
}
