using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// KoubeiItemExtitemExistedQueryResponse.
    /// </summary>
    public class KoubeiItemExtitemExistedQueryResponse : AopResponse
    {
        /// <summary>
        /// 商品库中存在的商品编码
        /// </summary>
        [XmlArray("existed_list")]
        [XmlArrayItem("string")]
        public List<string> ExistedList { get; set; }
    }
}
