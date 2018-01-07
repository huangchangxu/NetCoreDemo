using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Autofac.Extensions.DependencyInjection;
using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.ConfigCenter;

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

            //app.UseApiAuth();

            app.UseMvc();
        }
    }
}
