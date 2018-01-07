using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// HttpContext相关拓展
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (ip.IsNullOrEmpty())
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        /// <summary>
        /// 获取客户端Post提交的数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientPostData(this HttpContext context)
        {
            if (context.IsNotNull() && context.Request.IsNotNull() && context.Request.Body.IsNotNull() && context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var stream = context.Request.Body;
                StreamReader reader = new StreamReader(stream);
                var bodyContent = reader.ReadToEnd();
                return bodyContent;
            }
            else
                return string.Empty;
        }

        //public static string GetActionContextResult(this ActionExecutedContext context)
        //{
        //    if (context.IsNotNull() && context.Result.IsNotNull())
        //    {
        //        //根据实际需求进行具体实现
        //        if (context.Result is ObjectResult)
        //        {
        //            var objectResult = context.Result as ObjectResult;
        //            if (objectResult.Value == null)
        //            {
        //                context.Result = new ObjectResult(new { code = 404, sub_msg = "未找到资源", msg = "" });
        //            }
        //            else
        //            {
        //                context.Result = new ObjectResult(new { code = 200, msg = "", result = objectResult.Value });
        //            }
        //        }
        //        else if (context.Result is EmptyResult)
        //        {
        //            context.Result = new ObjectResult(new { code = 404, sub_msg = "未找到资源", msg = "" });
        //        }
        //        else if (context.Result is ContentResult)
        //        {
        //            context.Result = new ObjectResult(new { code = 200, msg = "", result = (context.Result as ContentResult).Content });
        //        }
        //        else if (context.Result is StatusCodeResult)
        //        {
        //            context.Result = new ObjectResult(new { code = (context.Result as StatusCodeResult).StatusCode, sub_msg = "", msg = "" });
        //        }
        //    }
        //    else
        //        return string.Empty;
        //}
        /// <summary>
        /// 根据Key获取请求头中的信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="headerKey"></param>
        /// <returns></returns>
        public static string GetHeadersValue(this HttpContext context, string headerKey)
        {
            if (context.IsNotNull()
                && context.Request.IsNotNull()
                && context.Request.Headers.IsNotNull()
                && context.Request.Headers.ContentLength.HasValue
                && context.Request.Headers.Any(m=>m.Key.Equals(headerKey,StringComparison.OrdinalIgnoreCase)))
            {
                var value = context.Request.Headers.FirstOrDefault(m => m.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).ToString();
                return value;
            }
            else
                return string.Empty;
        }
        /// <summary>
        /// 判断客户端是不是移动设备
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsMobileDevice(this HttpContext context)
        {
            var userAgent = context.GetHeadersValue("User-Agent");
            if (userAgent.IsNullOrEmpty())
                return false;
            
            return (userAgent.ToLower().Contains("android") || userAgent.ToLower().Contains("ios"));
        }
    }
}
