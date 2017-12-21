using PaymentCenter.Infrastructure.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PaymentCenter.Infrastructure.Tools
{
    /// <summary>
    /// Http请求工具
    /// </summary>
    public class HttpTool
    {
        #region HTTP请求方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
        /// <summary>
        /// http请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        /// <param name="timeOut">请求超时时间</param>
        /// <param name="method">请求方式</param>
        /// <param name="format">请求数据格式</param>
        /// <returns></returns>
        public static string HttpRequest(string url, string data,HttpRequestMethod method,HttpRequestDataFormat format, bool isUseCert=false, string certPath="", string certPwd="", int timeOut=5)
        {
            GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = string.Empty;//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 1000 * timeOut;


                request.Method = method.ToString().ToUpper();

                if (isUseCert)
                {
                    X509Certificate2 certificate2 = new X509Certificate2(certPath, certPwd);
                    request.ClientCertificates.Add(certificate2);
                }

                if (method == HttpRequestMethod.POST)
                {
                    request.Method = "POST";
                    switch (format)
                    {
                        case HttpRequestDataFormat.Form:
                            request.ContentType = "application/x-www-form-urlencoded";
                            break;
                        case HttpRequestDataFormat.Json:
                            request.ContentType = "application/json";
                            break;
                        default:
                            throw new InvalidCastException("请求数据格式无效");
                    }
                    var postData = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = postData.Length;
                    //写入数据
                    reqStream = request.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    reqStream.Close();
                }
                else
                    request.Method = "GET";

                //获取服务端返回结果
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd().Trim();
                reader.Close();
            }
            catch (Exception ex)
            {
                if (ex is System.Threading.ThreadAbortException)
                    System.Threading.Thread.ResetAbort();

                throw ex;
            }
            finally
            {
                response?.Close();
                request?.Abort();
            }
            return result;
        }
        /// <summary>
        /// http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        /// <param name="timeOut">请求超时时间</param>
        /// <param name="method">请求方式</param>
        /// <param name="format">请求数据格式</param>
        /// <returns></returns>
        public static string HttpRequest<T>(string url, T data, HttpRequestMethod method, HttpRequestDataFormat format, bool isUseCert = false, string certPath = "", string certPwd = "", int timeOut = 5)
        {
            var strData = string.Empty;
            if (format == HttpRequestDataFormat.Json)
                strData = data.ToJson();
            else
            {
                if (data is IDictionary<string, object>)
                    (data as IDictionary<string, object>).ToHttpFormData();
                else if (data is IList)
                    (data as IList<object>).ToHttFormData();
                else if (data is string)
                    strData = data as string;
                else
                {
                    List<string> list = new List<string>();
                    var dic = data.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(data) == null ? "" : p.GetValue(data).ToString());
                    foreach (var item in dic)
                    {
                        list.Add($"{item.Key}={System.Web.HttpUtility.UrlEncode(item.Value)}");
                    }
                    strData = $"{string.Join("&", list)}";
                }
            }

            return HttpRequest(url, strData, method, format, isUseCert, certPath, certPwd, timeOut);
        }
        /// <summary>
        /// 签名加密Http请求方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">请求数据</param>
        /// <param name="url">请求地址</param>
        /// <param name="isDES">是否加密</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="method">请求方式（GET,POST）</param>
        /// <returns></returns>
        public static string HttpDshlAuthRequest<T>(T data, string url, bool isDES = false, HttpRequestMethod method = HttpRequestMethod.POST, int timeOut = 5)
        {
            string json = string.Empty;
            if (data != null)
            {
                if (typeof(T) == typeof(string))
                    json = data as string;
                else
                    json = data.ToJson();
            }
            else
                json = string.Empty;

            var desResult = GetDataAuth(json, isDES);

            var strHttpResult = HttpRequest($"{url}?{desResult.Item2}", desResult.Item1, method, HttpRequestDataFormat.Json);
            return strHttpResult;
        }
        #endregion

        #region 数据加密基础方法
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintextBuffer">明文</param>
        /// <returns></returns>
        private static string Encryption(string key, byte[] iv, byte[] plaintextBuffer)
        {
            ICryptoTransform transform = GetTripleDES(key, iv).CreateEncryptor();
            byte[] cipherTextBuffer = transform.TransformFinalBlock(plaintextBuffer, 0, plaintextBuffer.Length);
            transform.Dispose();
            return Convert.ToBase64String(cipherTextBuffer);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="key">密匙</param>
        /// <param name="iv">IV向量</param>
        /// <param name="cipherTextBuffer">密文</param>
        /// <returns></returns>
        private static string Decryption(string key, byte[] iv, byte[] cipherTextBuffer)
        {
            ICryptoTransform transform = GetTripleDES(key, iv).CreateDecryptor();
            byte[] decryption = transform.TransformFinalBlock(cipherTextBuffer, 0, cipherTextBuffer.Length);
            transform.Dispose();
            return Encoding.UTF8.GetString(decryption);
        }

        private static TripleDESCryptoServiceProvider GetTripleDES(string key, byte[] iv)
        {
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            //key不能直接用，应该先转换成散列
            //MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            //byte[] tdesKey = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            //与java一致
            var key24 = new byte[24];
            Array.Copy(Encoding.UTF8.GetBytes(key), key24, 24);
            tripleDES.Key = key24;
            tripleDES.IV = iv;
            return tripleDES;
        }

        #endregion

        #region 店商加密方法
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
            string dataChiperText = Encryption(dataKey, Encoding.UTF8.GetBytes(iv.ToString().Substring(2, 8)), Encoding.UTF8.GetBytes(data));
            //3.返回数据
            return new SecretMetaData { createTime = timeStamp, msg = dataChiperText };
        }
        /// <summary>
        /// 生成 数据完整性的checkStr 【需要在数据域处理完成后】
        /// </summary>
        /// <param name="data">post数据域</param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        private static string BuilderDataIntegrityStr(string data, string appkey)
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
        #endregion

        #region HTTP下载文件
        /// <summary>
        /// Http下载文件支持断点续传
        /// </summary>
        /// <param name="uri">下载地址</param>
        /// <param name="filefullpath">存放完整路径（含文件名）</param>
        /// <param name="size">每次多的大小</param>
        /// <returns>下载操作是否成功</returns>
        public static bool HttpDownLoadFiles(string uri, string filefullpath, int size = 1024)
        {
            try
            {
                string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                string fileFullPath = filefullpath;
                string fileTempFullPath = filefullpath + ".tmp";

                if (File.Exists(fileFullPath))
                {
                    return true;
                }
                else
                {
                    if (File.Exists(fileTempFullPath))
                    {
                        FileStream fs = new FileStream(fileTempFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                        byte[] buffer = new byte[512];
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

                        request.Timeout = 10000;
                        request.AddRange((int)fs.Length);

                        Stream ns = request.GetResponse().GetResponseStream();

                        long contentLength = request.GetResponse().ContentLength;

                        int length = ns.Read(buffer, 0, buffer.Length);

                        while (length > 0)
                        {
                            fs.Write(buffer, 0, length);

                            buffer = new byte[512];

                            length = ns.Read(buffer, 0, buffer.Length);
                        }

                        fs.Close();
                        File.Move(fileTempFullPath, fileFullPath);
                    }
                    else
                    {
                        FileStream fs = new FileStream(fileTempFullPath, FileMode.Create);

                        byte[] buffer = new byte[512];
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                        request.Timeout = 10000;
                        request.AddRange((int)fs.Length);

                        Stream ns = request.GetResponse().GetResponseStream();

                        long contentLength = request.GetResponse().ContentLength;

                        int length = ns.Read(buffer, 0, buffer.Length);

                        while (length > 0)
                        {
                            fs.Write(buffer, 0, length);

                            buffer = new byte[512];

                            length = ns.Read(buffer, 0, buffer.Length);
                        }

                        fs.Close();
                        File.Move(fileTempFullPath, fileFullPath);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Http下载文件
        /// </summary>
        /// <param name="uri">下载地址</param>
        /// <param name="filefullpath">存放完整路径（含文件名）</param>
        /// <param name="size">每次多的大小</param>
        /// <returns>下载操作是否成功</returns>
        public static bool DownLoadFiles(string uri, string filefullpath, int size = 1024)
        {
            try
            {
                if (File.Exists(filefullpath))
                {
                    try
                    {
                        File.Delete(filefullpath);
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                FileStream fs = new FileStream(filefullpath, FileMode.Create);
                byte[] buffer = new byte[size];
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Timeout = 10000;
                request.AddRange((int)fs.Length);

                Stream ns = request.GetResponse().GetResponseStream();

                long contentLength = request.GetResponse().ContentLength;

                int length = ns.Read(buffer, 0, buffer.Length);

                while (length > 0)
                {
                    fs.Write(buffer, 0, length);

                    buffer = new byte[size];

                    length = ns.Read(buffer, 0, buffer.Length);
                }
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
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
    /// <summary>
    /// 请求方式
    /// </summary>
    public enum HttpRequestMethod
    {
        /// <summary>
        /// Get
        /// </summary>
        GET = 1,
        /// <summary>
        /// POST
        /// </summary>
        POST = 2
    }
    /// <summary>
    /// 请求数据格式
    /// </summary>
    public enum HttpRequestDataFormat
    {
        /// <summary>
        /// form表单键值对（key&value）
        /// </summary>
        Form = 1,
        /// <summary>
        /// Json格式
        /// </summary>
        Json = 2
    }
}
