using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayCommerceCityfacilitatorCityQueryResponse.
    /// </summary>
    public class AlipayCommerceCityfacilitatorCityQueryResponse : AopResponse
    {
        /// <summary>
        /// 城市列表
        /// </summary>
        [XmlArray("citys")]
        [XmlArrayItem("city_function")]
        public List<CityFunction> Citys { get; set; }
    }
}
