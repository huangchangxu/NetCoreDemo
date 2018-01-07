using Autofac;
using PaymentCenter.Infrastructure.Authorization;

namespace PaymentCenter.Api.AutoFac
{
    public class ApiModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiAuthenticationVerification>().As<IApiAuthenticationHandle>();
        }
    }
}
