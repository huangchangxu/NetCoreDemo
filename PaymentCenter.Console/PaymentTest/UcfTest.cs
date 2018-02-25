using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PaymentCenter.ConsoleApp.PaymentTest
{
    public class UcfTest
    {
        public static void UcfPayApiRequestDtoTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            PaymentCore.UcfPay.Models.UcfPayApiRequestDto dto = new PaymentCore.UcfPay.Models.UcfPayApiRequestDto("MOBILE_CERTPAY_PC_ORDER_CREATE"
                , "M200004205"
                , "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCL4MhqkB71TuaOlsnDmQKFuPNLcSi1cmRV4tdoPAuiZb5F1OdQ2DjYHVActHx3wR/r8HKYnxoWToXDfx7aNEjKkr+PXZEvz4hzrZdaVC7To88W+K8XVonjaTxxcLYYvyJtdqwnMmNyL1FEugL7Vc9gpMnGy8kyldmBYntNEFxnTwIDAQAB"
                ,"4.0.0",new { a=1,b=2});
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            //dto.InitData();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            var json=  dto.ToString();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(json);
        }
    }
}
