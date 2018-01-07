using Microsoft.AspNetCore.Http;
using PaymentCenter.Infrastructure.Extension;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace PaymentCenter.Infrastructure.Authorization
{
    /// <summary>
    /// 店商API公用请求身份校验
    /// </summary>
    public class ApiAuthenticationVerification: IApiAuthenticationHandle
    {
        public bool AuthVerification(HttpContext context, out string msg)
        {
            msg = string.Empty;

            var getData = GetUrlParameter(context);
            if (!Tools.CommonTools.ModelValidation(getData, out string errormsg))
            {
                msg = errormsg;
                return false;
            }

            var postBody = GetPostJsonStrData(context);
            AuthSecretData secretData = null;

            if (postBody.IsNullOrEmpty())
            {
                var json = postBody.JsonDeserialize<BasePostData>();
                var plat = AuthorizationConfig.GetAuthPlats(json.plat);
                if (plat == null || plat.FirstOrDefault() == default(AuthPlatData))
                {
                    msg = $"平台[{json.plat}]未获得请求许可";
                    return false;
                }


                secretData = AuthorizationConfig.GeAuthSecretDataByPlat(plat.FirstOrDefault().Plat, getData.auth_ver);
                if (secretData.IsNull() || secretData == default(AuthSecretData))
                {
                    msg = $"平台[{json.plat}]未获取到校验数据";
                    return false;
                }
            }
            else
            {
                secretData = AuthorizationConfig.GetAuthSecretDataByAppKey(getData.appkey, getData.auth_ver);
                if (secretData.IsNull() || secretData == default(AuthSecretData))
                {
                    msg = $"该AppKey[{getData.appkey}]未获取到校验数据";
                    return false;
                }
            }

            if (!ApiAuthVerificationTool.DataAuthentication(secretData.authkey, getData.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(getData).ToString()), "s"))
            {
                msg = "签名校验失败";
                return false;
            }

            if (!ApiAuthVerificationTool.BuilderDataIntegrityStr(postBody, getData.appkey).Equals(getData.checkStr))
            {
                msg = "数据完整性校验失败";
                return false;
            }

            return true;
        }

        private string GetPostJsonStrData(HttpContext context)
        {
            if (context.IsNotNull() && context.Request.IsNotNull() && context.Request.Body.IsNotNull())
            {
                var stream = context.Request.Body;
                StreamReader reader = new StreamReader(stream);
                var bodyContent = reader.ReadToEnd();
                return bodyContent;
            }
            else
                return string.Empty;
        }


        private AuthData GetUrlParameter(HttpContext context)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (context != null && context.Request != null && context.Request.Query.IsNotNull())
            {
                foreach (var item in context.Request.Query)
                {
                    dictionary.Add(item.Key, item.Value.ToString());
                }
            }
            return dictionary.ToJson().JsonDeserialize<AuthData>();
        }
    }

    /// <summary>
    /// Url身份验证数据
    /// </summary>
    public class AuthData
    {
        /// <summary>
        /// 签名
        /// </summary>
        [Required(ErrorMessage ="缺少签名字段值")]
        public string s { get; set; }
        /// <summary>
        /// 公钥
        /// </summary>
        [Required(ErrorMessage ="缺少公钥值")]
        public string appkey { get; set; }
        /// <summary>
        /// 设备号MD5
        /// </summary>
        [Required(ErrorMessage ="缺少设备号")]
        public string tk { get; set; }
        /// <summary>
        /// 签名版本
        /// </summary>
        [Required(ErrorMessage ="缺少签名版本值")]
        public string auth_ver { get; set; }
        /// <summary>
        /// Post数据String类型的MD5
        /// </summary>
        [Required(ErrorMessage ="checkStr必填")]
        public string checkStr { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [Required(ErrorMessage ="缺少时间戳值")]
        public string nonce { get; set; }

    }

    /// <summary>
    /// 授权平台配置数据
    /// </summary>
    public sealed class AuthPlatData
    {
        /// <summary>
        /// 平台ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 平台名称
        /// </summary>
        public string Plat { get; set; }
    }

    /// <summary>
    /// 授权校验秘钥数据
    /// </summary>
    public class AuthSecretData
    {
        /// <summary>
        /// appkey
        /// </summary>
        public string appkey { get; set; }
        /// <summary>
        /// 认证key
        /// </summary>
        public string authkey { get; set; }
        /// <summary>
        /// 数据加密key
        /// </summary>
        public string datakey { get; set; }
    }
    /// <summary>
    /// 支付中心Post请求基础数据
    /// </summary>
    public class BasePostData
    {
        /// <summary>
        /// 请求许可平台ID
        /// 同配置中心保持一致【DSHLN.PaymentCenter.AuthPlat】
        /// </summary>
        public int plat { get; set; }
        /// <summary>
        /// 请求客户端ID
        /// </summary>
        public int terminal { get; set; }
        /// <summary>
        /// 请求客户端设备描述
        /// </summary>
        public int equipment { get; set; }
        /// <summary>
        /// 请求客户端Ip
        /// </summary>
        public string ip { get; set; }
    }
    /// <summary>
    /// 配置允许匿名配置
    /// </summary>
    public class ConfigCenterAllowAnonymous
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControlName { get; set; }
        /// <summary>
        /// action名称
        /// </summary>
        public string[] ActionList { get; set; }
    }
}

