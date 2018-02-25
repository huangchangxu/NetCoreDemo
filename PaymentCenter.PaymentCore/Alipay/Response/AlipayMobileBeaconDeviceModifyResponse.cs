using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobileBeaconDeviceModifyResponse.
    /// </summary>
    public class AlipayMobileBeaconDeviceModifyResponse : AopResponse
    {
        /// <summary>
        /// 返回的操作码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobileBeaconDeviceModifyResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconDeviceModifyResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 操作结果说明
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobileBeaconDeviceModifyResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconDeviceModifyResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
