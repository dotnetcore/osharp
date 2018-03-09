using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Shouldly;

using Xunit;


namespace OSharp.Dependency.Tests
{
    public class IgnoreDependencyAttributeTests
    {
        [Fact]
        public void Ignore_Test()
        {
            IServiceCollection services = new ServiceCollection();
            IAppServiceAdder adder = new AppServiceAdder(new AppServiceScanOptions());
            services = adder.AddServices(services);

            services.ShouldContain(m => m.ServiceType == typeof(ITestContract));
            services.ShouldNotContain(m => m.ServiceType == typeof(IIgoreContract));
        }

        [IgnoreDependency]
        private interface IIgoreContract { }

        private interface ITestContract : IIgoreContract { }

        private class TestService : ITestContract, ITransientDependency { }
    }
}
