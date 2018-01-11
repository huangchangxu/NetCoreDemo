using PaymentCenter.Infrastructure.Tools;
using System;

namespace PaymentCenter.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //RedisTest.SuTest();

            //RabbitMqTest.ConsumerTest();   

            //RabbitMqTest.PublishTest();

            //LogTest.InfoTest();

            //RSATest.Test();

            //ConfigTest.ConfigCenterGetTest();

            //ESTest.Query();
            DbTest.GetTest();

            Console.ReadLine();
        }
    }
}
