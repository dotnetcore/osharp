using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Xunit;


namespace OSharp.Dependency.Tests
{
    public class AppServiceAdderTests
    {
        [Fact]
        public void Ctor_Test()
        {
            IAppServiceAdder adder = new AppServiceAdder();
            IServiceCollection services = new ServiceCollection();
            services = adder.AddServices(services);

        }

        [IgnoreDependency]
        private interface IIgoreContract { }

        private interface ITestContract : IIgoreContract { }

        private class TransientTestService : ITestContract, ITransientDependency { }

        private class ScopedTestService : ITestContract, IScopeDependency { }

        private class SingletonTestService : ITestContract, ISingletonDependency { }

        private interface IGenericContract<T> { }

        private class GenericService<T> : IGenericContract<T>, IScopeDependency { }
    }
}
