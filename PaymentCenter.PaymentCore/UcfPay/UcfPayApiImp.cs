using PaymentCenter.PaymentCore.UcfPay.Models;
using System;
using System.Collections.Generic;
using System.Text;
using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.Tools;

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
        public static UcfPayApiResponseDto UcfPayApiRequest(string gateway,int payType, UcfPayApiRequestDto requestDto)
        {
            UcfPayApiResponseDto ucfPay = new UcfPayApiResponseDto();

            var strJson = requestDto.ToString();
            var objJson = strJson.JsonDeserialize<Dictionary<string, object>>();

            if (payType == 2)
            {
                var result = HttpTool.HttpRequest(gateway, objJson, HttpRequestMethod.POST, HttpRequestDataFormat.Form);

                var dicResult = new Dictionary<string, string>();
                try
                {
                    //解密先锋支付返回数据
                    var strDecrypty = UcfUtils.AESDecrypt(result, requestDto.GetMerRsakey());
                    dicResult = strDecrypty.JsonDeserialize<Dictionary<string, string>>();

                    ucfPay.RespData = strDecrypty;
                    ucfPay.RespDecryptData = dicResult;
                    ucfPay.RespStatus = (dicResult.ContainsKey("status") && !dicResult["status"].IsNullOrEmpty() && dicResult["status"] == "00");


                }
                catch (Exception ex)
                { }


            }

            return ucfPay;
        }
    }
}
