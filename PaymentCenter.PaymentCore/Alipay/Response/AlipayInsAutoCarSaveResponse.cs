using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayInsAutoCarSaveResponse.
    /// </summary>
    public class AlipayInsAutoCarSaveResponse : AopResponse
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [XmlElement("car_no")]
        public string CarNo { get; set; }
    }
}
