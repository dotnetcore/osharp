using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.Core.Builders;


namespace OSharp.UnitTest
{
    public static class ServicesExtensions
    {
        public static IServiceProvider UnitTestInit(this IServiceCollection services)
        {
            services.AddOSharp(build => build.AddCorePack());
            IServiceProvider provider = services.BuildServiceProvider();
            provider.UseOSharp();
            return provider;
        }
    }
}
