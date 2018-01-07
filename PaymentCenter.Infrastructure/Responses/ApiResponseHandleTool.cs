using Autofac;
using PaymentCenter.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Responses
{
    /// <summary>
    /// api输出通用处理工具类
    /// </summary>
    public class ApiResponseHandleTool
    {
        /// <summary>
        /// 通用异常处理
        /// 在使用项目中优先检查是否实现【IApiResponseExceptionHandle】异常处理接口
        /// 如未实现则执行默认处理
        /// </summary>
        /// <param name="exception">待处理的异常</param>
        /// <param name="statueCode">异常响应状态码</param>
        /// <returns></returns>
        public static string HandleException(Exception exception, int statueCode=500)
        {
            if (AutofacConfig.AutoFacContainer.container.TryResolve(out IApiResponseExceptionHandle apiResponseExceptionHandle))
            {
                return apiResponseExceptionHandle.ExcetionHandle(exception).ToJson();
            }
            else
            {
                var response = new ApiCommonResponseDto<object>(statueCode, exception.ToString());
                return response.ToJson();
            }
        }
    }
}
