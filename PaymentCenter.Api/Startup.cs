using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentCenter.Infrastructure.ConfigCenter;
using PaymentCenter.Infrastructure.Extension;
using System;

namespace PaymentCenter.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            ConfigCenterHelper.GetInstance();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option=> {
                option.Filters.Add(new Infrastructure.Filters.ApiActionAuthenticationFilter());
                option.Filters.Add(new Infrastructure.Filters.ApiResponseFormattingFilter());
            });
            services.AddScoped<Infrastructure.Filters.ApiActionAuthenticationFilter>();
            Infrastructure.AutofacConfig.AutoFacContainer.Build<AutoFac.ApiModule>(services);
            return new AutofacServiceProvider(Infrastructure.AutofacConfig.AutoFacContainer.container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders=Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor|Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

           app.UseApiMonitor();

            app.UseMvc();
        }
    }
}
