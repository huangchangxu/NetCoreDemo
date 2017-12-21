using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using PaymentCenter.Infrastructure.Redis;
using PaymentCenter.Infrastructure.Extension;

namespace PaymentCenter.Infrastructure.ConfigCenter
{
    /// <summary>
    /// 配置中心Redis帮助类
    /// </summary>
    public class ConfigCenterRedisHelper
    {
        private static ISubscriber GetSubscriber(string url)
        {

            RedisClientConfigurations configurations = new RedisClientConfigurations
            {
                Url = url
            };
            var client = RedisClient.GetRedisClient(configurations);
            client.PreserveAsyncOrder = configurations.PreserveAsyncOrder;
            return client.GetSubscriber();
        }
        #region Redis发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        public static void RedisSub(string url,string subChannel, Action<string, string> onMessage = null)
        {
            var _sub = GetSubscriber(url);
            _sub.Subscribe(subChannel, (channel, message) =>
            {
                onMessage?.Invoke(channel, message);
            });
        }
        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static long RedisPub<T>(string url,string channel, T msg)
        {
            var _sub = GetSubscriber(url);
            return _sub.Publish(channel, msg.ToJson());
        }
        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public static void Unsubscribe(string url,string channel)
        {
            var _sub = GetSubscriber(url);
            _sub.Unsubscribe(channel);
        }
        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public static void UnsubscribeAll(string url)
        {
            var _sub = GetSubscriber(url);
            _sub.UnsubscribeAll();
        }
        #endregion
    }
}
