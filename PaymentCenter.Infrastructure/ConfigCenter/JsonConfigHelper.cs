using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PaymentCenter.Infrastructure.ConfigCenter
{
    /// <summary>
    /// appsetting文件
    /// </summary>
    public sealed class JsonConfigHelper
    {
        public T GetAppSettings<T>(string key) where T : class, new()
        {
            var baseDir = AppContext.BaseDirectory;
            var indexSrc = baseDir.IndexOf("src");
            var subToSrc = baseDir.Substring(0, indexSrc);
            var currentClassDir = subToSrc + "src" + Path.DirectorySeparatorChar + "StutdyEFCore.Data";

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
                .Build();
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }
    }
}
