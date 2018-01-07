using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using PaymentCenter.Infrastructure.Extension;

namespace PaymentCenter.Infrastructure.Log
{
    /// <summary>
    /// api接口监控日志数据实体
    /// </summary>
    public class LogMonitorDto: LogMonitorBase
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// 日志来源的模块类型
        /// 
        /// </summary>
        public ModuleType? ModuleType { get; set; }

        /// <summary>
        /// api请求地址
        /// 
        /// </summary>
        public string ApiUrl { get; set; }


        /// <summary>
        /// 参数
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// 服务器MAC地址
        /// 
        /// </summary>
        public string ServerMac { get; set; }

        /// <summary>
        /// 消息
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常的json格式
        /// 
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 唯一id，对象初始化时自动生成
        /// 
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 创建时间
        /// 
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 请求耗时 单位毫秒
        /// </summary>
        public double TimeTotal { get; set; }

        /// <summary>
        /// 数据记录来源
        /// </summary>
        public string Source { get; set; }

        public LogMonitorDto(HttpContext context)
            :base(context)
        {
            Init();
        }

        private void Init()
        {
            Guid = System.Guid.NewGuid().ToString();
            BeginTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"/>
        public void SetException(Exception ex)
        {
            Level = LogLevel.Error;
            if (ex == null)
                return;
            Exception innermostException = ex.GetInnestException();
            Exception = ex.ToJson();
            Message = Message ?? $"{innermostException.GetType() as object} {innermostException.Message as object}";
        }
    }
    /// <summary>
    /// api接口监控日志基础数据
    /// </summary>
    public class LogMonitorBase
    {
        #region 属性
        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string ClientBrowser { get; set; }

        /// <summary>
        /// 浏览器版本
        /// </summary>
        public string ClientBrowserVersion { get; set; }

        /// <summary>
        /// cookieid
        /// </summary>
        public string ClientCookieId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public ulong? ClientUserId { get; set; }

        /// <summary>
        /// 提交过来的cookie
        /// </summary>
        public string ClientHttpCookies { get; set; }

        /// <summary>
        /// 提交过来的Headers
        /// </summary>
        public string ClientHttpHeaders { get; set; }


        /// <summary>
        /// session
        /// </summary>
        public string ClientHttpSessions { get; set; }

        /// <summary>
        /// 客户端ip
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 客户端cookie是否可用
        /// </summary>
        public bool ClientIsCookieEnabled { get; set; }

        /// <summary>
        /// 是否是手机请求
        /// </summary>
        public bool ClientIsMobileDevice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientLanguage { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public string ClientPlatform { get; set; }

        /// <summary>
        /// 提交数据
        /// </summary>
        public string ClientPosts { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string ClientRequestMethod { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string ClientRequestUrl { get; set; }

        /// <summary>
        /// 客户端UserAgent
        /// </summary>
        public string ClientUserAgent { get; set; }

        #endregion

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public LogMonitorBase()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public LogMonitorBase(HttpContext context)
        {
            if (context.IsNotNull())
            {
                ClientIp = context.GetClientIp();
                ClientBrowser = "";
                ClientBrowserVersion = "";
                ClientLanguage = context.GetHeadersValue("Accept-Language");

                ClientPlatform = "";
                ClientIsCookieEnabled = true;
                ClientIsMobileDevice = context.IsMobileDevice();
                ClientUserAgent = context.GetHeadersValue("User-Agent");
                ClientHttpHeaders = context.Request.Headers.ToJson();
                ClientHttpCookies = context.Request.Cookies.ToJson();
                ClientPosts = context.GetClientPostData();


                ClientRequestUrl = $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}";
                ClientRequestMethod = context.Request.Method;

            }
        }
        #endregion
    }
}
