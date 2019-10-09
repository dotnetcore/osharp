using Microsoft.Extensions.DependencyInjection;

using Xunit;


namespace OSharp.Dependency.Tests
{
    public class AppServiceAdderTests
    {
        [Fact]
        public void Ctor_Test()
        {
            DependencyPack pack = new DependencyPack();
            IServiceCollection services = new ServiceCollection();
            services = pack.AddServices(services);

        }

        [IgnoreDependency]
        private interface IIgnoreContract { }

        private interface ITestContract : IIgnoreContract { }

        private class TransientTestService : ITestContract, ITransientDependency { }

        private class ScopedTestService : ITestContract, IScopeDependency { }

        private class SingletonTestService : ITestContract, ISingletonDependency { }

        private interface IGenericContract<T> { }

        private class GenericService<T> : IGenericContract<T>, IScopeDependency { }
    }
}
