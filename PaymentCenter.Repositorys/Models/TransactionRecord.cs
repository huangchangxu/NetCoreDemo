using System;

namespace PaymentCenter.Repositorys.Models
{
    /// <summary>
    /// 交易记录
    /// </summary>
    public class TransactionRecord
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 网关关联ID
        /// </summary>
        public int GatewayId { get; set; }
        /// <summary>
        /// 商城订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 商城支付交易号
        /// </summary>
        public string TradeCode { get; set; }
        /// <summary>
        /// 交易用户标识ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 虚拟区域ID
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 虚拟区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易状态（0:未支付1:已支付)
        /// </summary>
        public int TradeStatus { get; set; }
        /// <summary>
        /// 终端（1:PC;2: 微信;3: Android;4: IOS;5:云店）
        /// </summary>
        public int Terminal { get; set; }
        /// <summary>
        /// 第三方交易号
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// 记录类型（1:交易；2：还款；3：充值）
        /// </summary>
        public int RecodeType { get; set; }
        /// <summary>
        /// 冗余网关对应的交易信息，以json格式保存，便于查询和后续退款操作
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
