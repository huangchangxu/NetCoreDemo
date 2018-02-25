using System;
using System.Xml.Serialization;

namespace PaymentCenter.PaymentCore.Alipay.Domain
{
    /// <summary>
    /// AlipaySocialBaseRelationFriendsQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipaySocialBaseRelationFriendsQueryModel : AopObject
    {
        /// <summary>
        /// 获取类型。1=获取双向好友   2=获取双向+单向好友
        /// </summary>
        [XmlElement("get_type")]
#pragma warning disable CS0108 // “AlipaySocialBaseRelationFriendsQueryModel.GetType”隐藏继承的成员“object.GetType()”。如果是有意隐藏，请使用关键字 new。
        public long GetType { get; set; }
#pragma warning restore CS0108 // “AlipaySocialBaseRelationFriendsQueryModel.GetType”隐藏继承的成员“object.GetType()”。如果是有意隐藏，请使用关键字 new。

        /// <summary>
        /// 好友列表中是否返回自己， true=返回   false=不返回    默认false
        /// </summary>
        [XmlElement("include_self")]
        public bool IncludeSelf { get; set; }
    }
}
