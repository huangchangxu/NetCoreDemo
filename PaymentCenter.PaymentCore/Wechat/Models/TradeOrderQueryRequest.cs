using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.Wechat.Models
{
    /// <summary>
    /// 微信交易查询
    /// </summary>
    public class TradeOrderQueryRequest: TradeBaseData
    {
        /// <summary>
        /// 商城交易单号
        /// </summary>
        public string Out_Trade_No { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string Transaction_Id { get; set; }
    }
}
