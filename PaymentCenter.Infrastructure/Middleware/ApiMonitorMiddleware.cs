using Microsoft.AspNetCore.Http;
using PaymentCenter.Infrastructure.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentCenter.Infrastructure.Middleware
{
    /// <summary>
    /// api访问日志记录中间件
    /// </summary>
    public class ApiMonitorMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiMonitorMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                using (var ms = new MemoryStream())
                {
                    var orgBodyStream = context.Response.Body;
                    context.Response.Body = ms;

                    stopwatch.Start();
                    await _next.Invoke(context);
                    stopwatch.Stop();

                    using (var sr = new StreamReader(ms))
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        var body = sr.ReadToEnd();

                        LogHelper.WriteApiMonitorLog(context, body, stopwatch.ElapsedMilliseconds);
                        
                        ms.Seek(0, SeekOrigin.Begin);
                        await ms.CopyToAsync(orgBodyStream);
                        context.Response.Body = orgBodyStream;
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var res =Responses.ApiResponseHandleTool.HandleException(ex, context.Response.StatusCode);
                LogHelper.WriteApiMonitorLog(context, "", stopwatch.ElapsedMilliseconds, ex.ToString(), res);
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(res);
            }
        }
        
    }
}
