using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobileBeaconDeviceDeleteResponse.
    /// </summary>
    public class AlipayMobileBeaconDeviceDeleteResponse : AopResponse
    {
        /// <summary>
        /// 操作返回码
        /// </summary>
        [XmlElement("code")]
#pragma warning disable CS0108 // “AlipayMobileBeaconDeviceDeleteResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。
        public string Code { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconDeviceDeleteResponse.Code”隐藏继承的成员“AopResponse.Code”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 请求处理结果
        /// </summary>
        [XmlElement("msg")]
#pragma warning disable CS0108 // “AlipayMobileBeaconDeviceDeleteResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
        public string Msg { get; set; }
#pragma warning restore CS0108 // “AlipayMobileBeaconDeviceDeleteResponse.Msg”隐藏继承的成员“AopResponse.Msg”。如果是有意隐藏，请使用关键字 new。
    }
}
