using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Redis
{
    /// <summary>
    /// Redis操作客户端
    /// </summary>
    public class RedisClient
    {
        #region private field
        /// <summary>
        /// redis 连接对象
        /// </summary>
        public static IConnectionMultiplexer _connMultiplexer;
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();
        /// <summary>
        /// 连接对象缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, IConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, IConnectionMultiplexer>();

        #endregion private field
        /// <summary>
        /// 默认链接串格式
        /// </summary>
        public static readonly string DefaultConnectionStringFormat = "{0},abortConnect = false,ssl = false,ConnectTimeout ={1},allowAdmin = true,connectRetry ={2}";
        /// <summary>
        /// 获取 Redis 连接对象
        /// </summary>
        /// <returns></returns>
        public static IConnectionMultiplexer GetConnectionRedisMultiplexer(string connectionString)
        {
            if (_connMultiplexer == null || !_connMultiplexer.IsConnected)
                lock (Locker)
                {
                    if (_connMultiplexer == null || !_connMultiplexer.IsConnected)
                        _connMultiplexer = ConnectionMultiplexer.Connect(connectionString);
                }

            return _connMultiplexer;
        }
        /// <summary>
        /// 获取Redis连接对象（线程安全）
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IConnectionMultiplexer GetRedisClient(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetConnectionRedisMultiplexer(connectionString);
            }
            return ConnectionCache[connectionString];
        }

        /// <summary>
        /// 获取Redis连接对象（线程安全）
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IConnectionMultiplexer GetRedisClient(RedisClientConfigurations configurations)
        {
            var connectionString = string.Format(DefaultConnectionStringFormat,
               string.Join(",", configurations.Url),
                configurations.ConnectTimeout,
                configurations.ConnectRetry);
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetConnectionRedisMultiplexer(connectionString);
            }
            return ConnectionCache[connectionString];
        }


    }
}
