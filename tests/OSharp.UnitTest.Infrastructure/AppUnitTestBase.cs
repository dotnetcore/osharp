using System;

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.UnitTest.Infrastructure
{
    public class AppUnitTestBase
    {
        public AppUnitTestBase()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHttpContextAccessor().AddLogging();
            services.AddOSharp();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.UseOsharp();
            ServiceProvider = provider;
        }

        /// <summary>
        /// 获取 服务提供者
        /// </summary>
        protected IServiceProvider ServiceProvider { get; private set; }
    }
}
