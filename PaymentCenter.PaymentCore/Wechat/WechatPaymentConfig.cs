using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PaymentCenter.PaymentCore.Wechat
{
    /// <summary>
    /// 微信支付相关配置数据
    /// </summary>
    public class WechatPaymentConfig
    {
        public static Models.TradeBaseData GetTradeBaseData(string appId)
        {
            var json = Infrastructure.ConfigCenter.ConfigCenterHelper.GetInstance().Get("DSHLN.PaymentCenter_Pay_wechat");
            if (string.IsNullOrEmpty(json))
                throw new WxPayException("未获取到配置中心中微信支付的配置数据");

            var list = Infrastructure.Tools.JsonTool.DeserializeJsonToList<Models.TradeBaseData>(json);
            if (!list.Any(m => m.AppId.Equals(appId)))
                throw new WxPayException($"未获取到配置中心中有关appid为{appId}的微信支付的配置数据");


            return list.SingleOrDefault(m => m.AppId.Equals(appId));
        }
    }
}
