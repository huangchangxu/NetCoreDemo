using PaymentCenter.Infrastructure.Tools;
using System;
using System.Collections.Generic;

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

            //DbTest.GetTest();

            //ModelValidTest.Test();

            //PaymentTest.UcfTest.UcfPayApiRequestDtoTest();

            string url = "http://creditapif.test.ds365.com/WxAssist/UpdateFinalInterest?appkey=e4d8ae3856a60b225e80c2a04e5cb295&auth_ver=1&checkStr=5359010003505c1b865626696c160f3d&nonce=1517832332&tk=9027ea84117932b47a66bffc6f1d050c&s=8f0a453d27dd90a35f5c0263a272810d";
            var data = new List<dynamic>
            {
                new { transaction_number = "dduat201801080101",repaymentId="beb87375f48b11e78d8000505690db30" ,interest=7 }
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            Infrastructure.Tools.HttpTool.HttpRequest(url, json, HttpRequestMethod.POST, HttpRequestDataFormat.Json);

            Console.ReadLine();
        }
    }
}
