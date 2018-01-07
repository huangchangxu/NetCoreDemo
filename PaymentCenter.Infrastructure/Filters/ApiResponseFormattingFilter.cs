using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentCenter.Infrastructure.Extension;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PaymentCenter.Infrastructure.Filters
{
    /// <summary>
    /// API 输出内容格式化为统一输出格式
    /// </summary>
    public class ApiResponseFormattingFilter : IResultFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using (var ms = new MemoryStream())
            {
                
                Stopwatch stopwatch = Stopwatch.StartNew();
                var result = await next();
                stopwatch.Stop();
                var orgBodyStream = result.HttpContext.Response.Body;
                result.HttpContext.Response.Body.CopyTo(ms);
                using (var sr = new StreamReader(ms))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var body = sr.ReadToEnd();
                }

                    //if (result.Exception == null)
                    //{
                    //    LogHelper.WriteApiMonitorLog(result.HttpContext, context.Result.ToJson(), stopwatch.ElapsedMilliseconds);
                    //}
                    //else
                    //{
                    //    var res = Responses.ApiResponseHandleTool.HandleException(result.Exception);
                    //    LogHelper.WriteApiMonitorLog(result.HttpContext, context.Result.ToJson(), stopwatch.ElapsedMilliseconds, result.Exception.ToString(), res);
                    //    result.ExceptionHandled = true;
                    //    result.Result = new ContentResult() { Content = res };
                    //}
            }
            
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var actionResult = context.Result;
            object result;

            if (actionResult is ObjectResult)
            {
                result = (actionResult as ObjectResult).Value;
            }
            else if (actionResult is ContentResult)
            {
                result = (actionResult as ContentResult).Content;
            }
            else if (actionResult is JsonResult)
            {
                result = (actionResult as JsonResult).Value;
            }
            else
            {
                return;
            }

            if (result.GetType().ToString().Contains("PaymentCenter.Infrastructure.Responses.ApiCommonResponseDto"))
            {
                return;
            }
            else if (result is string && result.ToString().Contains("responseHeader") && Tools.JsonSplitTool.IsJson(result.ToString()))
            {
                return;
            }
            else
            {
                if (result is string && Tools.JsonSplitTool.IsJson(result.ToString()))
                {
                    result = result.ToString().JsonDeserialize<object>();
                }
                context.Result = new JsonResult(new Responses.ApiCommonResponseDto<object>() { data = result }) { ContentType = "application/json;charset=utf-8" };
            }

        }
    }
}
