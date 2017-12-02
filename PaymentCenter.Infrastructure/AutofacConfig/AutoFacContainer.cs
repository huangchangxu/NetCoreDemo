using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.AutofacConfig
{
    public sealed class AutoFacContainer
    {
        public static IContainer container;

        private static ContainerBuilder builder;

        static AutoFacContainer()
        {
            builder = new ContainerBuilder();
            builder.RegisterModule<DefaultModule>();
        }

        public static void Build<T>(IEnumerable<ServiceDescriptor> descriptors = null) where T : IModule, new()
        {
            if (descriptors != null)
                builder.Populate(descriptors);
            builder.RegisterModule<T>();
            container = builder.Build();
        }
    }
}
