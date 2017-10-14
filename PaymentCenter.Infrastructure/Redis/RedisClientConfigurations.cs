using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Redis
{
    /// <summary>
    /// Redis链接配置
    /// </summary>
    public class RedisClientConfigurations
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }
       /// <summary>
       /// 链接超时时间
       /// </summary>
        public int ConnectTimeout { get; set; }
        /// <summary>
        /// 链接尝试次数
        /// </summary>
        public int ConnectRetry { get; set; }
        /// <summary>
        /// db
        /// </summary>
        public int DefaultDatabase { get; set; }
        /// <summary>
        /// 订阅消息是否有序传递
        /// </summary>
        public bool PreserveAsyncOrder { get; set; }
        /// <summary>
        /// 默认的 Key 值（用来当作 RedisKey 的前缀）
        /// </summary>
        public string DefaultKey;
    }
}
