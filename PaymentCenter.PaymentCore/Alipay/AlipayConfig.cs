namespace PaymentCenter.PaymentCore.ModelDto.PaymentConfig
{
    /// <summary>
    /// 支付宝支付基础配置
    public sealed class AlipayConfig
    {
        /// <summary>
        /// 支付宝支付网关
        /// </summary>
        public static string GatewayUrl { get; set; }
        /// <summary>
        /// 支付宝支付私钥
        /// </summary>
        public static string PrivateKey { get; set; }
        /// <summary>
        /// 支付宝支付公钥
        /// </summary>
        public static string PublicKey { get; set; }
        /// <summary>
        /// AES密钥
        /// </summary>
        public static string AES { get; set; }
        /// <summary>
        /// 支付宝付款方式限制
        /// </summary>
        public static string PayChannels { get; set; }
        /// <summary>
        /// 异步通知地址
        /// </summary>
        public static string Notify_Url { get; set; }

    }
}
