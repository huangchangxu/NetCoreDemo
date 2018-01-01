using Microsoft.AspNetCore.Http;
using PaymentCenter.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentCenter.Infrastructure.Middleware
{
    public class ApiAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiAuthenticationHandle _authenticationHandle;

        public ApiAuthenticationMiddleware(RequestDelegate next,IApiAuthenticationHandle authenticationHandle)
        {
            this._next = next;
            this._authenticationHandle = authenticationHandle;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_authenticationHandle.AuthenticationVerification(context, out string msg))
            {
                context.Response.OnCompleted(ResponseCompletedCallback, context);
                await _next.Invoke(context);
            }
            else
            {
                var data = new Responses.ApiCommonResponseDto<object>(401, msg);
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(data.ToJson());
            }
        }

        private Task ResponseCompletedCallback(object obj)
        {
            return Task.FromResult(0);
        }
    }

    public interface IApiAuthenticationHandle
    {
        /// <summary>
        /// API访问授权身份校验
        /// </summary>
        /// <param name="context">Http请求上下文</param>
        /// <param name="errormsg">校验结果描述</param>
        /// <returns></returns>
        bool AuthenticationVerification(HttpContext context, out string msg);
    }
}
