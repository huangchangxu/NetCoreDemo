using Newtonsoft.Json.Linq;
using PaymentCenter.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaymentCenter.Infrastructure.Authorization
{
    /// <summary>
    /// 授权相关配置
    /// </summary>
    public sealed class AuthorizationConfig
    {
        /// <summary>
        /// 获取支付中心接口许可平台
        /// </summary>
        /// <param name="id">平台ID（-1：获取所有许可平台）</param>
        /// <returns></returns>
        public static List<AuthPlatData> GetAuthPlats(int id = -1)
        {
            var jsonStr = ConfigCenter.ConfigCenterHelper.GetInstance().Get("DSHLN.PaymentCenter.AuthPlat");
            var jsonObj = jsonStr.JsonDeserialize<List<AuthPlatData>>();
            if (id == -1)//获取配置中所有许可平台配置
                return jsonObj;
            else
            {
                var selectObj = jsonObj.Where(m => m.Id == id);
                if (selectObj.IsNull())
                    return default(List<AuthPlatData>);
                else
                    return selectObj.ToList();
            }
        }

        /// <summary>
        /// 根据平台获取指定平台的校验信息
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="auth_ver"></param>
        /// <returns></returns>
        public static AuthSecretData GeAuthSecretDataByPlat(string plat, string auth_ver)
        {
            //获取配置中心授权校验信息
            var configCenterKey = string.Format("DSHLN.PaymentCenter_DataAuthList_v{0}", auth_ver);
            var dataAuthJson = ConfigCenter.ConfigCenterHelper.GetInstance().Get(configCenterKey);

            if (dataAuthJson.IsNullOrEmpty())
                return default(AuthSecretData);
            try
            {
                //校验请求appkey是否合法
                var jObject = JObject.Parse(dataAuthJson);

                if (jObject.Properties().Any(j => j.Name.Equals(plat, StringComparison.OrdinalIgnoreCase)))
                    return Tools.JsonTool.DeserializeJsonToObject<AuthSecretData>(jObject[plat].ToString());
                else
                    return default(AuthSecretData);
            }
            catch (Exception ex)
            {
                //记录日志
                return default(AuthSecretData);
            }
        }
        /// <summary>
        /// 根据Appkey获取指定平台的校验信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="auth_ver"></param>
        /// <returns></returns>
        public static AuthSecretData GetAuthSecretDataByAppKey(string appKey, string auth_ver)
        {
            //获取配置中心授权校验信息
            var configCenterKey = string.Format("DSHLN.PaymentCenter_DataAuthList_v{0}", auth_ver);
            var dataAuthJson = ConfigCenter.ConfigCenterHelper.GetInstance().Get(configCenterKey);

            if (dataAuthJson.IsNullOrEmpty())
                return default(AuthSecretData);
            try
            {
                //校验请求appkey是否合法
                var jObject = Newtonsoft.Json.Linq.JObject.Parse(dataAuthJson);

                var plats = GetAuthPlats();
                var authJson = string.Empty;
                foreach (var item in plats)
                {
                    if (jObject.Properties().Any(j => j.Name.Equals(item.Plat, StringComparison.OrdinalIgnoreCase)))
                    {
                        var key = jObject[item]["appkey"].ToString();
                        if (key.Equals(appKey))
                            authJson = jObject[item].ToString();
                    }
                }
                if (authJson.IsNullOrEmpty())
                    return Tools.JsonTool.DeserializeJsonToObject<AuthSecretData>(authJson);
                else
                    return default(AuthSecretData);

            }
            catch (Exception ex)
            {
                //记录日志
                return default(AuthSecretData);
            }
        }
        /// <summary>
        /// 获取配置中心中相应的控制器和action是否允许匿名访问
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static bool GetIsAllowAnonymousRequest(string controlName, string actionName)
        {
            try
            {
                var jsonStr = ConfigCenter.ConfigCenterHelper.GetInstance().Get("DSHLN.PaymentCenter.AllowAnonymous");

                if (jsonStr.IsNullOrEmpty())
                    return false;

                var jObject = jsonStr.JsonDeserialize<List<ConfigCenterAllowAnonymous>>();

                if (jObject.Any(m => m.ControlName.Equals(controlName, StringComparison.OrdinalIgnoreCase)))
                {
                    if (actionName.IsNullOrEmpty())
                        return true;

                    return jObject.First(m => m.ControlName.Equals(controlName, StringComparison.OrdinalIgnoreCase)).ActionList.Any(m => m.Equals(actionName,StringComparison.OrdinalIgnoreCase));
                }
                else
                    return false;

            }
            catch {
                return false;
            }
        }


    }
}
