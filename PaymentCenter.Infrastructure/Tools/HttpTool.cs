using PaymentCenter.Infrastructure.Authorization;
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
                    strData = (data as IDictionary<string, object>).ToHttpFormData();
                else if (data is IList)
                    strData = (data as IList<object>).ToHttFormData();
                else if (data is string)
                    strData = data as string;
                else
                {
                    List<string> list = new List<string>();
                    var dic = data.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(data) == null ? "" : p.GetValue(data).ToString());
                    strData = dic.ToHttpFormData();
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

            var desResult =ApiAuthVerificationTool.GetDataAuth(json, isDES);

            var strHttpResult = HttpRequest($"{url}?{desResult.Item2}", desResult.Item1, method, HttpRequestDataFormat.Json);
            return strHttpResult;
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
