using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicGisGetResponse.
    /// </summary>
    public class AlipayMobilePublicGisGetResponse : AopResponse
    {
        /// <summary>
        /// 精确度
        /// </summary>
        [XmlElement("accuracy")]
        public string Accuracy { get; set; }

        /// <summary>
        /// 经纬度所在位置
        /// </summary>
        [XmlElement("city")]
        public string City { get; set; }

        /// <summary>
        /// 结果码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobilePublicGisGetResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicGisGetResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 纬度信息
        /// </summary>
        [XmlElement("latitude")]
        public string Latitude { get; set; }

        /// <summary>
        /// 经度信息
        /// </summary>
        [XmlElement("longitude")]
        public string Longitude { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobilePublicGisGetResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobilePublicGisGetResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 经纬度对应位置所在的省份
        /// </summary>
        [XmlElement("province")]
        public string Province { get; set; }
    }
}
