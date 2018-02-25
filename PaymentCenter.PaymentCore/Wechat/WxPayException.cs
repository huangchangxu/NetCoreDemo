using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.Wechat
{
    public class WxPayException: Exception
    {
        public WxPayException(string exceptionMsg) : base(exceptionMsg)
        {

        }
    }
}
