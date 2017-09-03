using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Xunit;


namespace OSharp.Tests.Dependency
{
    public class ServiceCollectionTests
    {
        [Fact]
        public void AddTest()
        {
            ServiceCollection services = new ServiceCollection();

            //services.AddTransient<ITestContract, TestService1>();
            //services.AddTransient<ITestContract, TestService2>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<ITestContract, TestService1>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<ITestContract, TestService1>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<ITestContract, TestService2>());


            Assert.Equal(2, services.Count);

            IServiceProvider provider = services.BuildServiceProvider();
            Assert.Equal(2, provider.GetServices<ITestContract>().Count());

            Assert.IsAssignableFrom<TestService2>(provider.GetService<ITestContract>());
        }

        private interface ITestContract { }

        private class TestService1 : ITestContract { }

        private class TestService2 : ITestContract { }
    }
}
