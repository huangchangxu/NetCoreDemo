using System;
using System.Collections.Generic;
using System.Text;
using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.Redis;

namespace PaymentCenter.ConsoleApp
{
    public class RedisTest
    {
        public static void StringTest()
        {
            RedisHelper.StringSet("test", DateTime.Now.ToString());

            var value = RedisHelper.StringGet("test");

            Console.WriteLine(value);

            RedisClientConfigurations configurations = new RedisClientConfigurations
            {
                Url = "127.0.0.1:6379",
            };
            RedisHelper.StringSet("Model_test", configurations);
            var json = RedisHelper.StringGet<RedisClientConfigurations>("Model_test");
            Console.WriteLine(json);
        }

        public static void SuTest()
        {
            RedisHelper.RedisPub("test","开始运行——发布订阅");
            System.Threading.Thread.Sleep(5000);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                RedisHelper.RedisSub("test", (c, m) =>
                {
                    Console.WriteLine("订阅【{0}】频道，消息为【{1}】", c, m);
                });
            });
            while (true)
            {
                RedisHelper.RedisPub("test", DateTime.Now.ToString());
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
