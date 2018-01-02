using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Shouldly;

using Xunit;


namespace OSharp.Dependency.Tests
{
    public class ServiceLocatorTests
    {
        [Fact]
        public void Instance_Test()
        {
            ServiceLocator locator1 = ServiceLocator.Instance;
            ServiceLocator locator2 = ServiceLocator.Instance;
            locator2.ShouldBe(locator1);
        }

        [Fact]
        public void TrySetApplicationServiceProvider_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ServiceLocatorTests, ServiceLocatorTests>();
            IServiceProvider provider = services.BuildServiceProvider();
            ServiceLocator.Instance.TrySetApplicationServiceProvider(provider);
        }
    }
}
