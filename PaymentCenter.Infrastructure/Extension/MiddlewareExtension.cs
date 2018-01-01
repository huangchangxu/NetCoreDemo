using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// 拓展自定义中间件
    /// </summary>
    public static class MiddlewareExtension
    {
        /// <summary>
        /// 添加错误处理中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware.ErrorHandlingMiddleware>();
        }
        /// <summary>
        /// 添加API授权验证中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware.ApiAuthenticationMiddleware>();
        }
    }
}
