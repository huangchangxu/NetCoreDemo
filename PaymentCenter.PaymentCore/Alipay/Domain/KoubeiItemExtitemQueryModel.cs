using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// KoubeiItemExtitemQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class KoubeiItemExtitemQueryModel : AopObject
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        [XmlElement("item_code")]
        public string ItemCode { get; set; }
    }
}
