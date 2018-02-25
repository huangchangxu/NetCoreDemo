using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobileBeaconDeviceAddResponse.
    /// </summary>
    public class AlipayMobileBeaconDeviceAddResponse : AopResponse
    {
        /// <summary>
        /// 请求操作成功与否，200为成功
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobileBeaconDeviceAddResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconDeviceAddResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 请求的处理结果
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobileBeaconDeviceAddResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconDeviceAddResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
