using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Packs;


namespace OSharp.UnitTest.Infrastructure
{
    public class AppUnitTestBase
    {
        public AppUnitTestBase()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHttpContextAccessor().AddLogging();
            services.AddOSharp<OsharpPackManager>();
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
