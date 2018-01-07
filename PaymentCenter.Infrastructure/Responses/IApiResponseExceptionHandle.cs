using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Responses
{
    /// <summary>
    /// API响应未拦截异常处理
    /// </summary>
    public interface IApiResponseExceptionHandle
    {
        ApiCommonResponseDto<object> ExcetionHandle(Exception exception);
    }
}
