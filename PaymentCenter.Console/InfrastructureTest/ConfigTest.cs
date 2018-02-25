using PaymentCenter.Infrastructure.ConfigCenter;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.ConsoleApp
{
    /// <summary>
    /// 配置中心TEST
    /// </summary>
    public class ConfigTest
    {
        public static void ConfigRedisTest()
        {
            var url = "192.168.2.8:7379";
            var chanle = "ConfigManager.Redis.Channel";
            ConfigCenterRedisHelper.RedisSub(url, chanle, (c, m) =>
            {
                Console.WriteLine("订阅【{0}】频道，消息为【{1}】", c, m);
            });
        }

        public static void ConfigFileTest()
        {
            var value = ApplicationConfigHelper.AppSettings;
            Console.WriteLine(value);
        }

        public static void ConfigCenterInitTest()
        {
            ConfigCenterHelper.GetInstance();
        }

        public static void ConfigCenterGetTest()
        {
            Query:
            Console.WriteLine("请输入你想获取的配置中心的key");
            var key = Console.ReadLine();
            Console.WriteLine("[{0}]的值为：[{1}]", key, ConfigCenterHelper.GetInstance().Get(key));
            goto Query;
        }


    }
}
