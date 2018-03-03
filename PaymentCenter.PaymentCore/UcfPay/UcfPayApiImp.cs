using PaymentCenter.PaymentCore.UcfPay.Models;
using System;
using System.Collections.Generic;
using System.Text;
using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.Tools;
using System.Linq;

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
        /// <typeparam name="T">支付请求具体交易数据</typeparam>
        /// <param name="gataway">请求api网关</param>
        /// <param name="payData">api请求数据</param>
        /// <returns></returns>
        public static UcfApiResponseDto UcfPayApiRequest<T>(string gateway, UcfApiRequestBase<T> payData)
        {
            if (!(payData.RequestData is UcfSdkPayApiRequestDto) && !(payData.RequestData is UcfPcPayApiRequestDto))
                throw new ArgumentException("支付数据类型异常");

            if (payData.UcfService != UcfServiceEnum.MOBILE_CERTPAY_ORDER_CREATE && payData.UcfService != UcfServiceEnum.MOBILE_CERTPAY_PC_ORDER_CREATE)
                throw new ArgumentOutOfRangeException("参数[UcfService]超出支持范围");

            var strJson = JsonTool.SerializeObject(payData.RequestData, true);//将支付数据序列化为key为小驼峰命名格式json串
            //生成API请求数据
            UcfApiRequest ucfApiRequest = new UcfApiRequest(payData.UcfService.ToString(), payData.MerchantId, payData.MerRsakey, payData.Version, strJson);

            UcfApiResponseDto responseDto = new UcfApiResponseDto();//响应数据

            if (UcfServiceEnum.MOBILE_CERTPAY_ORDER_CREATE == payData.UcfService)//sdk支付
            {
                var result = HttpTool.HttpRequest(gateway, ucfApiRequest.GetRequestData(), HttpRequestMethod.POST, HttpRequestDataFormat.Form);//请求支付网关

                if (!result.IsNullOrEmpty())
                {
                    try
                    {
                        var strDecrpty = UcfUtils.AESDecrypt(result, payData.MerRsakey);
                        var obj = strDecrpty.JsonDeserialize<Dictionary<string, string>>();

                        if (obj.ContainsKey("status")
                            && !obj["status"].IsNullOrEmpty()
                            && obj["status"].Equals("00"))
                        {
                            Dictionary<string, object> dictionary = new Dictionary<string, object>
                            {
                                { "merchantId",payData.MerchantId},
                                { "outOrderId",(payData.RequestData as UcfSdkPayApiRequestDto).OutOrderId},
                                { "userId",(payData.RequestData as UcfSdkPayApiRequestDto).UserId},
                                { "sign",""}
                            };
                            var strSignData = UcfUtils.getSignData(dictionary, "sign");
                            dictionary["sign"] = UcfUtils.RSAEncrypt(UcfUtils.Md5Encrypt(strSignData).ToLower(), payData.MerRsakey).UrlEncode();

                            responseDto.RespStatus = true;
                            responseDto.RespMsg = obj["respMsg"];
                            responseDto.RespData = result;
                            responseDto.RespDecryptData = obj;
                            responseDto.ClientRespData = dictionary;
                        }
                        else
                        {
                            responseDto.RespStatus = false;
                            responseDto.RespMsg = $"{obj["respMsg"]}[{obj["status"]}]";
                        }
                    }
                    catch (Exception ex)
                    {
                        responseDto.RespStatus = false;
                        responseDto.RespMsg = $"创建预付单解密异常[{ex.ToString()}]";
                    }
                }
                else
                {
                    responseDto.RespStatus = false;
                    responseDto.RespMsg = "SDK接口请求失败,无返回信息或请求超时";
                }
            }
            else if (UcfServiceEnum.MOBILE_CERTPAY_PC_ORDER_CREATE == payData.UcfService)//pc支付
            {
                Dictionary<string, object> result = new Dictionary<string, object>{
                    {"formUrl", gateway },
                    {"formData",ucfApiRequest.GetRequestData()}
                };

                responseDto.RespStatus = true;
                responseDto.ClientRespData = result;
            }
            return responseDto;
        }
        /// <summary>
        /// 支付查询
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public static UcfApiResponseDto UcfPayQueryApiRequest(string gateway, UcfApiRequestBase<string> queryData)
        {
            var strJson = new { orderId = queryData.RequestData }.ToJson();

            UcfApiRequest ucfApiRequest = new UcfApiRequest(queryData.UcfService.ToString(), queryData.MerchantId, queryData.MerRsakey, queryData.Version, strJson);


            UcfApiResponseDto responseDto = new UcfApiResponseDto();//响应数据
            try
            {
                var result = HttpTool.HttpRequest(gateway, ucfApiRequest.GetRequestData(), HttpRequestMethod.POST, HttpRequestDataFormat.Form);
                if (!result.IsNullOrEmpty())
                {
                    var dataJson = UcfUtils.AESDecrypt(result, queryData.MerRsakey).JsonDeserialize<Dictionary<string, string>>();
                    if (dataJson.ContainsKey("errorCode") && !dataJson["errorCode"].IsNullOrEmpty())
                    {
                        responseDto.RespStatus = false;
                        responseDto.RespMsg = dataJson["errorMessage"].ToString();
                        responseDto.RespData = result;
                        responseDto.RespDecryptData = dataJson;
                    }
                    else
                    {
                        responseDto.RespStatus = true;
                        responseDto.RespData = result;
                        responseDto.RespDecryptData = dataJson;
                        responseDto.ClientRespData = result.JsonDeserialize<Dictionary<string, object>>();
                    }
                }
                else
                {
                    responseDto.RespStatus = false;
                    responseDto.RespMsg = $"请求支付查询无返回结果";
                }
            }
            catch (Exception ex)
            {
                responseDto.RespStatus = false;
                responseDto.RespMsg = $"请求支付查询或解析查询结果异常[{ex}]";
            }
            return responseDto;
        }
        /// <summary>
        /// 退款请求
        /// </summary>
        /// <param name="gateway">请求api网关</param>
        /// <param name="refundData">退款数据</param>
        /// <returns></returns>
        public static UcfApiResponseDto UcfRefundApiRequest(string gateway, UcfApiRequestBase<UcfRefundApiRequestDto> refundData)
        {
            UcfApiResponseDto responseDto = new UcfApiResponseDto();//响应数据

            var isValid = CommonTools.ModelValidation(refundData, out string errMsg);

            if (isValid)
                isValid = CommonTools.ModelValidation(refundData.RequestData, out errMsg);

            if (!isValid)
                throw new ArgumentException($"退款请求参数非法【{errMsg}】");

            var strJson = JsonTool.SerializeObject(refundData.RequestData, true);//将支付数据序列化为key为小驼峰命名格式json串
            UcfApiRequest ucfApiRequest = new UcfApiRequest(refundData.UcfService.ToString(), refundData.MerchantId, refundData.MerRsakey, refundData.Version, strJson);
            try
            {
                var result = HttpTool.HttpRequest(gateway, ucfApiRequest.GetRequestData(), HttpRequestMethod.POST, HttpRequestDataFormat.Form);
                if (!result.IsNullOrEmpty())
                {
                    var dataJson = UcfUtils.AESDecrypt(result, refundData.MerRsakey).JsonDeserialize<Dictionary<string, string>>();

                    if (dataJson.ContainsKey("result") && dataJson["result"].Equals("I", StringComparison.OrdinalIgnoreCase))
                        responseDto.RespStatus = true;//受理成功
                    else
                    {
                        responseDto.RespStatus = false;
                        var msg = dataJson.ContainsKey("message") ? dataJson["message"] : "";
                        var code = dataJson.ContainsKey("resCode") ? dataJson["resCode"] : "";
                        var codeMsg= dataJson.ContainsKey("resMessage") ? dataJson["resMessage"] : "";
                        responseDto.RespMsg = "";
                    }

                    responseDto.RespData = result;
                    responseDto.RespDecryptData = dataJson;
                }
                else
                {
                    responseDto.RespStatus = false;
                    responseDto.RespMsg = $"请求支付查询无返回结果";
                }
            }
            catch (Exception ex)
            {
                responseDto.RespStatus = false;
                responseDto.RespMsg = $"请求支付查询或解析查询结果异常[{ex}]";
            }

            return responseDto;
        }
    }
}
