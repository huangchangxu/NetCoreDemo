using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentCenter.Infrastructure.Authorization;
using System.Linq;

namespace PaymentCenter.Infrastructure.Filters
{
    /// <summary>
    /// Api身份验证过滤器
    /// </summary>
    public class ApiActionAuthenticationFilter : IActionFilter
    {
        private readonly Authorization.IApiAuthenticationHandle _authenticationHandle;
        public ApiActionAuthenticationFilter()
        {
            _authenticationHandle = AutofacConfig.AutoFacContainer.container.Resolve<Authorization.IApiAuthenticationHandle>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new System.NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsAllowAnonymousRequest(context))
            {
                if (!_authenticationHandle.AuthVerification(context.HttpContext, out string msg))
                {
                    var data = new Responses.ApiCommonResponseDto<object>(401, msg);
                    context.Result = new JsonResult(data) { ContentType = "application/json;charset=utf-8" };
                }
            }
        }

        private bool IsAllowAnonymousRequest(ActionExecutingContext context)
        {
            var controlName = context.Controller.GetType().Name;
            var actionName = context.ActionDescriptor.DisplayName.Split("(")[0].Trim();
            actionName = actionName.Substring(actionName.LastIndexOf('.')+1);
            var configCenterSetting = AuthorizationConfig.GetIsAllowAnonymousRequest(controlName, actionName);
            return (context.Controller.GetType().CustomAttributes.Any(m=>m.AttributeType.Name==typeof(AllowAnonymousAttribute).Name)
                || configCenterSetting);
        }


    }
}
