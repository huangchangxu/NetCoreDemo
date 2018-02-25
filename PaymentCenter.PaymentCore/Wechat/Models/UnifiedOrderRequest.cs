using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.Wechat.Models
{
    /// <summary>
    /// 统一下单基础请求数据
    /// </summary>
    public class UnifiedOrderRequest : TradeBaseData
    {
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string Attach { get; set; }
        /// <summary>
        /// 商城订单号
        /// </summary>
        public string Out_Trade_No { get; set; }
        /// <summary>
        /// 付款金额（分）
        /// </summary>
        public int Total_Fee { get; set; }
        /// <summary>
        /// 交易起始时间
        /// </summary>
        public string Time_Start { get; set; }
        /// <summary>
        /// 交易失效视角
        /// </summary>
        public string Time_Expire { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public TradeType Trade_Type { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string Product_Id { get; set; }
        /// <summary>
        /// 终端IP
        /// APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP
        /// </summary>
        public string Spbill_Create_Ip { get; set; }
        /// <summary>
        /// 用户标识
        /// trade_type=JSAPI时（即公众号支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识。
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string Notify_Url { get; set; }
        /// <summary>
        /// 是否可以使用信用卡支付
        /// </summary>
        public bool IsUseCredit { get; set; }
        /// <summary>
        /// 场景信息
        /// 该字段用于上报支付的场景信息,针对H5支付有以下三种场景,请根据对应场景上报,H5支付不建议在APP端使用，针对场景1，2请接入APP支付，不然可能会出现兼容性问题
        /// </summary>
        public string Scene_Info { get; set; }
    }
}
