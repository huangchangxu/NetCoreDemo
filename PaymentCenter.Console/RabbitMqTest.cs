using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.RabbitMq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PaymentCenter.ConsoleApp
{
    /// <summary>
    /// 消息队列测试
    /// </summary>
    public class RabbitMqTest
    {
        public static void PublishTest()
        {
            var rabbitMqProxy = new RabbitMqService(new MqConfig
            {
                AutomaticRecoveryEnabled = true,
                HeartBeat = 60,
                NetworkRecoveryInterval = new TimeSpan(60),
                Host = "192.168.2.8",
                UserName = "payment",
                Password = "payment",
                VirtualHost= "PaymentVHost"
            });
            while (true)
            {
                Thread.Sleep(1000);
                var input = Guid.NewGuid().ToString();
                rabbitMqProxy.Publish(new MessageModel
                {
                    Msg = input,
                    CreateDateTime = DateTime.Now
                });
            }
            Console.ReadLine();
        }

        public static void ConsumerTest()
        {
            var rabbitMqProxy = new RabbitMqService(new MqConfig
            {
                AutomaticRecoveryEnabled = true,
                HeartBeat = 60,
                NetworkRecoveryInterval = new TimeSpan(60),
                Host = "192.168.2.8",
                UserName = "payment",
                Password = "payment",
                VirtualHost = "PaymentVHost"
            });

            rabbitMqProxy.Subscribe<MessageModel>((item) =>
            {
                Console.WriteLine("接受消息：{0}", item.ToJson());
                return true;
            });
        }
    }

    [RabbitMq("payment.QueueName", ExchangeName = "payment.ExchangeName", IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
