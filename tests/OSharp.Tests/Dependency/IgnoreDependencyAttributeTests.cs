using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;

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
            DependencyPack pack = new DependencyPack();
            services = pack.AddServices(services);

            services.ShouldContain(m => m.ServiceType == typeof(ITestContract));
            services.ShouldNotContain(m => m.ServiceType == typeof(IIgoreContract));
        }

        [IgnoreDependency]
        private interface IIgoreContract { }

        private interface ITestContract : IIgoreContract { }

        private class TestService : ITestContract, ITransientDependency { }
    }
}
