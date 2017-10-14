using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PaymentCenter.Infrastructure.Tools
{
    /// <summary>
    /// Http请求工具
    /// </summary>
    public class HttpTool
    {
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
        /// <summary>
        /// post操作 提交键值对的dic
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> param)
        {
            var content = new FormUrlEncodedContent(param);
            //此处未来需要添加HttpClient连接池,复用连接
            using (var client = new HttpClient())
            {
                var result = client.PostAsync(url, content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }
        /// <summary>
        /// post操作提交json
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="jsonParas"></param>
        /// <returns></returns>
        public static string GetHttpPost(string requestUrl, string jsonParas)
        {
            return GetHttpPost(jsonParas, requestUrl, false, 5);
        }
        /// <summary>
        /// 处理Post请求
        /// </summary>
        /// <param name="json">json格式数据</param>
        /// <param name="url">请求Api的Url的链接</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <param name="timeout">请求超时时间（秒）</param>
        /// <returns></returns>
        public static string GetHttpPost(string json, string url, bool isUseCert, int timeout)
        {

            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置POST的数据类型和长度
                request.ContentType = "application/json";
                byte[] data = Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                //关闭连接和流
                response?.Close();
                request?.Abort();
            }
            return result;
        }
        /// <summary>
        /// 使用 X509证书提交数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="url"></param>
        /// <param name="certPath"></param>
        /// <param name="certPwd"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string GetHttpPostUseCert(string json, string url, string certPath, string certPwd, int timeout)
        {

            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置POST的数据类型和长度
                request.ContentType = "application/json";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;


                X509Certificate2 cert = new X509Certificate2(certPath, certPwd);
                request.ClientCertificates.Add(cert);

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                //关闭连接和流
                response?.Close();
                request?.Abort();
            }
            return result;
        }

        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
                throw e;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string FormPost(string data, string url)
        {
            string postString = data;//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
            WebClient webClient = new WebClient();
            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 200;
            //设置https验证方式
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
            }
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可
            byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流
            string srcString = Encoding.UTF8.GetString(responseData);//解码
            return srcString;
        }

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
    }
}
