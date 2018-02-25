using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// KoubeiRetailShopitemBatchqueryResponse.
    /// </summary>
    public class KoubeiRetailShopitemBatchqueryResponse : AopResponse
    {
        /// <summary>
        /// 店铺商品集合
        /// </summary>
        [XmlArray("shopitemlist")]
        [XmlArrayItem("ext_shop_item")]
        public List<ExtShopItem> Shopitemlist { get; set; }
    }
}
