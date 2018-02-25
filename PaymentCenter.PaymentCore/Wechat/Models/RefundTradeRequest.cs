using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.Wechat.Models
{
    /// <summary>
    /// 退款申请数据
    /// </summary>
    public class RefundTradeRequest:TradeBaseData
    {
        /// <summary>
        /// 付款流水号
        /// </summary>
        public string Transaction_Id { get; set; }
        /// <summary>
        /// 交易总付款金额（分）
        /// </summary>
        public int Total_Fee { get; set; }
        /// <summary>
        /// 退款金额（分）
        /// </summary>
        public int Refund_Fee { get; set; }
        /// <summary>
        /// 退款单号
        /// </summary>
        public string Out_Fefund_No { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Op_User_Id { get; set; }
        /// <summary>
        /// 证书路径
        /// </summary>
        public string CertPath { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertPwd { get; set; }
    }
}
