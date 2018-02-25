using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaymentCenter.PaymentCore.UcfPay.Models
{
    public class UcfPayApiRequestDto
    {
        private object data;
        private string mer_rsakey;
        /// <summary>
        /// 先锋API请求数据对象
        /// </summary>
        /// <param name="service">请求服务名称</param>
        /// <param name="merchantId">商户号</param>
        /// <param name="rsaKey">Rsa签名公钥</param>
        /// <param name="version">版本号</param>
        /// <param name="data">请求数据</param>
        public UcfPayApiRequestDto(string service, string merchantId, string rsaKey, string version,object data=null)
        {
            Service = service;
            MerchantId = merchantId;
            SecId = "RSA";
            this.mer_rsakey = rsaKey;
            Version = version;
            this.data = data;

            InitData();
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MerchantId { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        public string ReqSn { get; set; }
        /// <summary>
        /// 签名算法
        /// </summary>
        public string SecId { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 业务加密数据
        /// </summary>
        public string Data { get; set; }

        #region public method
        /// <summary>
        /// 未加密请求数据
        /// </summary>
        /// <returns></returns>
        public object GetData()
        {
            return this.data;
        }
        /// <summary>
        /// 获取加密密钥
        /// </summary>
        /// <returns></returns>
        public string GetMerRsakey()
        {
            return this.mer_rsakey;
        }
        #endregion

        #region private method
        /// <summary>
        /// 初始化请求数据
        /// </summary>
        /// <returns></returns>
        private void InitData()
        {
            this.ReqSn = UcfUtils.createUnRepeatCode(this.MerchantId, this.Service, "DSHL");
            if (data.IsNotNull())
            {
                this.Data = UcfUtils.AESEncrypt(JsonTool.SerializeObject(this.data), this.mer_rsakey);
            }
            var sortDic = new SortedDictionary<string, object>(GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(this)));
            if (data.IsNull())
                sortDic.Remove("Data");
            StringBuilder buff = new StringBuilder("");
            sortDic.Each((pair) =>
            {
                if (!pair.Key.Equals("sign",StringComparison.OrdinalIgnoreCase) && pair.Value.ToString() != "")
                {
                    buff.Append($"{pair.Key}={pair.Value}&");
                }

            });
            buff.ToString().Trim('&');
            this.Sign= UcfUtils.RSAEncrypt(UcfUtils.Md5Encrypt(buff.ToString()).ToLower(), this.mer_rsakey);
        }
        #endregion

        #region Override
        /// <summary>
        /// 实体json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var json = JsonTool.SerializeObject(this, true);

            return json;
        }
        #endregion
    }
}
