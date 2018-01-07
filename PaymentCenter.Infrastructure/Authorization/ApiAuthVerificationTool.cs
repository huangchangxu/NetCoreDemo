using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PaymentCenter.Infrastructure.Authorization
{
    /// <summary>
    /// API请求身份验证
    /// </summary>
    public static class ApiAuthVerificationTool
    {
        private static string DataKey = ConfigCenter.ConfigCenterHelper.GetInstance().Get("Maticsoft.DS365.Security.dataKey");
        private static string Appkey = ConfigCenter.ConfigCenterHelper.GetInstance().Get("Maticsoft.DS365.Security.appkey");
        private static string AuthVer = ConfigCenter.ConfigCenterHelper.GetInstance().Get("Maticsoft.DS365.Security.auth_ver");
        private static string Appsecret = ConfigCenter.ConfigCenterHelper.GetInstance().Get("Maticsoft.DS365.Security.appsecret");


        /// <summary>
        /// 数据加密
        /// </summary>
        /// <param name="dataKey">加密key</param>
        /// <param name="data">body域</param>
        /// <returns></returns>
        private static SecretMetaData DataEcryption(string dataKey, string data)
        {
            long timeStamp = CommonTools.GetTimeStamp();
            long iv = timeStamp + 1;
            //1.加密
            string dataChiperText = DESTool.Encryption(dataKey, Encoding.UTF8.GetBytes(iv.ToString().Substring(2, 8)), Encoding.UTF8.GetBytes(data));
            //3.返回数据
            return new SecretMetaData { createTime = timeStamp, msg = dataChiperText };
        }
        /// <summary>
        /// 生成 数据完整性的checkStr 【需要在数据域处理完成后】
        /// </summary>
        /// <param name="data">post数据域</param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public static string BuilderDataIntegrityStr(string data, string appkey)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (data.Length > 50)
                {
                    data = data.Substring(0, 50);
                }
            }
            MD5 md5 = MD5.Create();
            var md5Value = md5.ComputeHash(Encoding.UTF8.GetBytes(data + appkey));
            string localCheckStr = BitConverter.ToString(md5Value).Replace("-", "").ToLower();
            return localCheckStr;
        }
        /// <summary>
        /// 生成数据认证签名
        /// </summary>
        /// <param name="appsecret">认证key：a63bab826e87f1276c26ae9feedd1622</param>
        /// <param name="paramsNV">url参数</param>
        /// <param name="signName">url参数 签名生成字段名 s</param>
        /// <returns></returns>
        private static string MakeDataAuthenticationSign(string appsecret, Dictionary<string, string> paramsNV)
        {
            StringBuilder sb = new StringBuilder();
            //1.参数排序
            SortedDictionary<string, string> sortDict = new SortedDictionary<string, string>(paramsNV);
            //2.拼凑
            foreach (var item in sortDict)
            {
                sb.Append(item.Key + item.Value);
            }
            sb.Append(appsecret);

            //3.签名
            var md5 = MD5.Create();
            byte[] md5Value = md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            var sign = BitConverter.ToString(md5Value).Replace("-", "").ToLower();
            return sign;
        }
        /// <summary>
        /// 获取请求信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetDataAuth(string json, bool DES = false)
        {
            string chkStr = string.Empty;
            SecretMetaData msg = null;
            if (DES)
            {
                msg = DataEcryption(DataKey, json);
                chkStr = msg.ToJson();
            }
            else
                chkStr = json;

            var checkStr = BuilderDataIntegrityStr(chkStr, Appkey);

            var tSpan = msg == null ? CommonTools.GetTimeStamp() : msg.createTime;
            var md5 = MD5.Create();
            var md5Value = md5.ComputeHash(Encoding.UTF8.GetBytes(tSpan.ToString()));
            var tk = BitConverter.ToString(md5Value).Replace("-", "").ToLower();
            var dic = new Dictionary<string, string>
            {
                {"tk", tk},
                {"appkey", Appkey},
                {"checkStr", checkStr},
                {"auth_ver", AuthVer},
                {"nonce",tSpan.ToString()}
            };
            var s = MakeDataAuthenticationSign(Appsecret, dic);
            dic.Add("s", s);
            //是否传输密文
            if (DES)
                return Tuple.Create(msg.ToJson(), string.Join("&", dic.Select(item => $"{item.Key}={item.Value}")));
            else
                return Tuple.Create(json, string.Join("&", dic.Select(item => $"{item.Key}={item.Value}")));
        }
        /// <summary>
        /// 数据认证
        /// </summary>
        /// <param name="appsecret">认证key：a63bab826e87f1276c26ae9feedd1622</param>
        /// <param name="paramsNV">url参数(全部)</param>
        /// <param name="signName">url参数 签名字段 s</param>
        /// <returns></returns>
        public static bool DataAuthentication(string appsecret, Dictionary<string, string> paramsNV, string signName)
        {
            string sign = paramsNV[signName];
            paramsNV.Remove(signName);
            var makeSign = MakeDataAuthenticationSign(appsecret, paramsNV);
            return makeSign.Equals(sign);
        }
    }

    /// <summary>
    /// 加密请求数据
    /// </summary>
    public sealed class SecretMetaData
    {
        /// <summary>
        /// 消息主体
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 时间 iv向量 10位+1  后8位
        /// </summary>
        public long createTime { get; set; }
    }
}
