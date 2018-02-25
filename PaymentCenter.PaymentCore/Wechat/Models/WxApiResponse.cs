using System;
using System.Collections.Generic;
using System.Text;
using PaymentCenter.Infrastructure.Extension;

namespace PaymentCenter.PaymentCore.Wechat.Models
{
    /// <summary>
    /// 微信支付所有API响应统一处理
    /// </summary>
    public class WxApiResponse
    {
        /// <summary>
        /// 响应原始数据
        /// </summary>
        public readonly WechatPaymentData ApiResponse;
        /// <summary>
        /// 请求响应码
        /// </summary>
        public readonly string Return_Code;
        /// <summary>
        /// 业务结果
        /// </summary>
        public readonly string Result_Code;
        /// <summary>
        /// 错误描述
        /// </summary>
        public readonly string ErrorMsg;

        public WxApiResponse(WechatPaymentData response)
        {
            ApiResponse = response;

            if (ApiResponse.IsNotNull())
            {
                if (ApiResponse.IsSet("return_code"))
                    this.Return_Code = ApiResponse.GetValue("return_code").ToString();

                if (ApiResponse.IsSet("result_code"))
                    this.Result_Code = ApiResponse.GetValue("result_code").ToString();

                if (ApiResponse.IsSet("return_msg"))
                    this.ErrorMsg = ApiResponse.GetValue("return_msg").ToString();

                if (ApiResponse.IsSet("err_code"))
                {
                    if (ApiResponse.IsSet("err_code_des"))
                        this.ErrorMsg = $"{ApiResponse.GetValue("err_code_des")}[{ApiResponse.GetValue("err_code")}]";
                    else
                        this.ErrorMsg = $"{ApiResponse.GetValue("err_code")}";
                }
            }
        }
        
        /// <summary>
        /// 响应结果{returnCode和ResultCode同时为success}
        /// </summary>
        public bool IsSuccess {
            get
            {
                if (Return_Code.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase)
                    && Result_Code.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                    return false;
            }
        }
        /// <summary>
        /// 响应数据签名是否正确
        /// </summary>
        public bool IsSign
        {
            get
            {
                var config = WechatPaymentConfig.GetTradeBaseData(ApiResponse.GetValue("appid").ToString());
                var b= ApiResponse.CheckSign(config.Key);
                return b;
            }
        }
    }
}
