using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.Tools;
using PaymentCenter.PaymentCore.Wechat.Models;
using System;

namespace PaymentCenter.PaymentCore.Wechat
{
    /// <summary>
    /// 微信支付各类支付方式和相关功能实现
    /// </summary>
    public sealed class WechatPaymentApiImp
    {
        /// <summary>
        ///  统一下单
        /// </summary>
        /// <param name="inputObj">提交给统一下单API的参数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>成功时返回，其他抛异常</returns>
        public static WxApiResponse UnifiedOrder(UnifiedOrderRequest orderRequest, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            WechatPaymentData inputObj = new WechatPaymentData();

            inputObj.SetValue("appid", orderRequest.AppId);//公众账号ID
            inputObj.SetValue("mch_id", orderRequest.Mch_Id);//商户号 	    
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("body", orderRequest.Body);//商品描述
            inputObj.SetValue("attach", orderRequest.Attach);//附加数据
            inputObj.SetValue("out_trade_no", orderRequest.Out_Trade_No);//商城订单号
            inputObj.SetValue("total_fee", orderRequest.Total_Fee);//总金额
            inputObj.SetValue("time_start", orderRequest.Time_Start);//交易起始时间
            inputObj.SetValue("time_expire",orderRequest.Time_Expire);//交易结束时间
            inputObj.SetValue("trade_type", orderRequest.Trade_Type.ToString());//交易类型
            inputObj.SetValue("spbill_create_ip", orderRequest.Spbill_Create_Ip);//终端ip


            if (!orderRequest.Product_Id.IsNullOrEmpty())
                inputObj.SetValue("product_id", orderRequest.Product_Id);//商品ID
            
            if (orderRequest.Trade_Type == TradeType.JSAPI)
                inputObj.SetValue("openid", orderRequest.OpenId);
            else if (orderRequest.Trade_Type == TradeType.MWEB)
                inputObj.SetValue("scene_info", orderRequest.Scene_Info);

            
            if (!orderRequest.IsUseCredit)
                inputObj.SetValue("limit_pay", "no_credit");

            //签名
            inputObj.SetValue("sign", inputObj.MakeSign(orderRequest.Key));
            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            string response = HttpTool.HttpRequest(url, xml, HttpRequestMethod.POST, HttpRequestDataFormat.Json, timeOut: timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WechatPaymentData result = new WechatPaymentData();
            result.FromXml(response);


            return new WxApiResponse(result);
        }
        /// <summary>
        /// 交易查询
        /// </summary>
        /// <param name="orderQueryRequest"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static WxApiResponse TradeOrderQuery(TradeOrderQueryRequest orderQueryRequest, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            WechatPaymentData inputObj = new WechatPaymentData();
            inputObj.SetValue("appid", orderQueryRequest.AppId);
            inputObj.SetValue("mch_id", orderQueryRequest.Mch_Id);
            inputObj.SetValue("nonce_str", GenerateNonceStr());
            if (!orderQueryRequest.Transaction_Id.IsNullOrEmpty())
                inputObj.SetValue("transaction_id", orderQueryRequest.Transaction_Id);
            if (!orderQueryRequest.Out_Trade_No.IsNullOrEmpty())
                inputObj.SetValue("out_trade_no", orderQueryRequest.Out_Trade_No);
            inputObj.SetValue("sign", inputObj.MakeSign(orderQueryRequest.Key));

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            string response = HttpTool.HttpRequest(url, xml, HttpRequestMethod.POST, HttpRequestDataFormat.Json, timeOut: timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WechatPaymentData result = new WechatPaymentData();
            result.FromXml(response);

            return new WxApiResponse(result);
        }
        /// <summary>
        /// 发起退款
        /// </summary>
        /// <param name="refundTradeRequest"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static WxApiResponse WxRefundTrade(RefundTradeRequest refundTradeRequest, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";

            WechatPaymentData inputObj = new WechatPaymentData();
            inputObj.SetValue("appid", refundTradeRequest.AppId);
            inputObj.SetValue("transaction_id", refundTradeRequest.Transaction_Id);
            inputObj.SetValue("total_fee", refundTradeRequest.Total_Fee);//订单总金额
            inputObj.SetValue("refund_fee", refundTradeRequest.Refund_Fee);//退款金额
            inputObj.SetValue("out_refund_no", refundTradeRequest.Out_Fefund_No);//随机生成商户退款单号
            inputObj.SetValue("op_user_id", refundTradeRequest.Op_User_Id);//操作员，默认为商户号

            inputObj.SetValue("mch_id", refundTradeRequest.Mch_Id);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign(refundTradeRequest.Key));//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            string response = HttpTool.HttpRequest(url, xml, HttpRequestMethod.POST, HttpRequestDataFormat.Json, true, refundTradeRequest.CertPath, refundTradeRequest.CertPwd, timeOut: timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WechatPaymentData result = new WechatPaymentData();
            result.FromXml(response);

            return new WxApiResponse(result);
        }

        /// <summary>
        /// 查询退款
        /// 提交退款申请后，通过该接口查询退款状态。退款有一定延时，
        /// 用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
        /// out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个
        /// </summary>
        /// <param name="inputObj">提交给查询退款API的参数</param>
        /// <param name="timeOut">接口超时时间</param>
        /// <returns>成功时返回，其他抛异常</returns>
        /// <exception cref="WxPayException"></exception>
        public static WxApiResponse RefundQuery(RefundQueryRequest refundQueryRequest, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";

            WechatPaymentData inputObj = new WechatPaymentData();
            inputObj.SetValue("appid", refundQueryRequest.AppId);//公众账号ID
            inputObj.SetValue("mch_id", refundQueryRequest.Mch_Id);//商户号

            if (!refundQueryRequest.Out_Trade_No.IsNullOrEmpty())
                inputObj.SetValue("out_trade_no", refundQueryRequest.Out_Trade_No);

            if (!refundQueryRequest.Transaction_Id.IsNullOrEmpty())
                inputObj.SetValue("transaction_id", refundQueryRequest.Transaction_Id);

            if (!refundQueryRequest.Out_Refund_No.IsNullOrEmpty())
                inputObj.SetValue("out_refund_no", refundQueryRequest.Out_Refund_No);

            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign(refundQueryRequest.Key));//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            string response = HttpTool.HttpRequest(url, xml, HttpRequestMethod.POST, HttpRequestDataFormat.Json, timeOut: timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WechatPaymentData result = new WechatPaymentData();
            result.FromXml(response);

            return new WxApiResponse(result);
        }

        /// <summary>
        /// 下载账单
        /// </summary>
        /// <param name="tradeBaseData"></param>
        /// <param name="billDate"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static WechatPaymentData DownloadBill(TradeBaseData tradeBaseData,DateTime billDate, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/downloadbill";

            WechatPaymentData inputObj = new WechatPaymentData();
            inputObj.SetValue("appid", tradeBaseData.AppId);//公众账号ID
            inputObj.SetValue("mch_id", tradeBaseData.Mch_Id);//商户号
            inputObj.SetValue("bill_date", billDate.ToString("yyyyMMdd"));
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign(tradeBaseData.Key));//签名

            string xml = inputObj.ToXml();

            string response = HttpTool.HttpRequest(url,xml,HttpRequestMethod.POST,HttpRequestDataFormat.Json,timeOut:timeOut);//调用HTTP通信接口以提交数据到API

            WechatPaymentData result = new WechatPaymentData();
            //若接口调用失败会返回xml格式的结果
            if (response.Substring(0, 5) == "<xml>")
            {
                result.FromXml(response);
            }
            //接口调用成功则返回非xml格式的数据
            else
                result.SetValue("result", response);

            return result;
        }


        /// <summary>
        ///  生成随机串，随机串包含字母或数字
        /// </summary>
        /// <returns>随机串</returns>
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
