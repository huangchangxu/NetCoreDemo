using System;

namespace PaymentCenter.Infrastructure.Filters
{
    /// <summary>
    /// 允许非签名匿名访问API
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AllowAnonymousAttribute: Attribute
    {
    }
}
