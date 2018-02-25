namespace PaymentCenter.PaymentCore.Wechat.Models
{
    /// <summary>
    /// 交易请求基础数据
    /// </summary>
    public class TradeBaseData
    {
        /// <summary>
        /// 支付应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string Mch_Id { get; set; }
        /// <summary>
        /// 商户支付密钥，参考开户邮件设置（必须配置）
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 公众帐号secert（仅JSAPI支付的时候需要配置）
        /// </summary>
        public string AppSecret { get; set; }
    }
}
