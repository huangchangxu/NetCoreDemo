using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Authorization
{
    public interface IApiAuthenticationHandle
    {
        /// <summary>
        /// API访问授权身份校验
        /// </summary>
        /// <param name="context">Http请求上下文</param>
        /// <param name="errormsg">校验结果描述</param>
        /// <returns></returns>
        bool AuthVerification(HttpContext context, out string msg);
    }
}
