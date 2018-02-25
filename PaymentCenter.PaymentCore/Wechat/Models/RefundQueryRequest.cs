using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.Wechat.Models
{
    /// <summary>
    /// 退款查询
    /// </summary>
    public class RefundQueryRequest:TradeBaseData
    {
        /// <summary>
        /// 原付款商城交易号
        /// </summary>
        public string Out_Trade_No { get; set; }
        /// <summary>
        /// 原付款交易流水号
        /// </summary>
        public string Transaction_Id { get; set; }
        /// <summary>
        /// 商城退款单号
        /// </summary>
        public string Out_Refund_No { get; set; }
    }
}
