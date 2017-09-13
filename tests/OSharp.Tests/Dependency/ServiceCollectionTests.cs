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
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<ITestContract, TestService1>();

            IServiceProvider provider = services.BuildServiceProvider();
            ITestContract contract = provider.GetService<ITestContract>();
            Assert.NotNull(contract);
            
        }

        private interface ITestContract { }

        private class TestService1 : ITestContract { }

        private class TestService2 : ITestContract { }
    }
}
