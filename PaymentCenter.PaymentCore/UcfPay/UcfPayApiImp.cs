using PaymentCenter.PaymentCore.UcfPay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.UcfPay
{
    /// <summary>
    /// 先锋支付相关API实现
    /// </summary>
    public class UcfPayApiImp
    {
        /// <summary>
        /// 先锋支付
        /// 发起支付
        /// </summary>
        /// <param name="gateway">支付网关</param>
        /// <param name="payType">支付类型（1：PC；2：SDK）</param>
        /// <param name="requestDto">请求数据</param>
        /// <returns></returns>
        public static string UcfPayApiRequest(string gateway,int payType, UcfPayApiRequestDto requestDto)
        {
            var strRequest = requestDto.ToString();

            return "";
        }
    }
}
