using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.Core.Builders;
using OSharp.Core.Packs;


namespace OSharp.UnitTest
{
    public static class ServicesExtensions
    {
        public static IServiceProvider UnitTestInit(this IServiceCollection services)
        {
            services.AddOSharp(build => build.AddCorePack());
            IServiceProvider provider = services.BuildServiceProvider();
            OSharpPackManager packManager = provider.GetService<OSharpPackManager>();
            packManager.UsePacks(provider);
            return provider;
        }
    }
}
