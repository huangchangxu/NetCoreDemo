namespace PaymentCenter.Infrastructure.Redis
{
    /// <summary>
    /// Redis链接配置
    /// </summary>
    public class RedisClientConfigurations
    {
        private string _url;
        private int _connectTimeout;
        private int _connectRetry;

        /// <summary>
        /// 服务器地址{IP}:{Port}
        /// </summary>
        public string Url { get { return string.IsNullOrEmpty(_url) ? "192.168.2.8:7379" : _url; } set => _url = value; }
        /// <summary>
        /// 链接超时时间
        /// </summary>
        public int ConnectTimeout { get { return _connectTimeout == 0 ? 5000 : _connectTimeout; } set => _connectTimeout = value; }
        /// <summary>
        /// 链接尝试次数
        /// </summary>
        public int ConnectRetry { get { return 5; } }
        /// <summary>
        /// 订阅消息是否有序传递
        /// </summary>
        public bool PreserveAsyncOrder { get => true;}
    }
}
