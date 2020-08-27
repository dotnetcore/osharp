using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;

using Shouldly;

using Xunit;


namespace OSharp.Tests.Startups
{
    public class StartupTests
    {
        [Fact]
        public void ServiceLifetimeTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddOSharp().AddPacks();

            IServiceProvider provider = services.BuildServiceProvider();

            //Singleton
            ITestSingletonContract singleton1, singleton2;
            using (provider.CreateScope())
            {
                singleton1 = provider.GetService<ITestSingletonContract>();
            }
            using (provider.CreateScope())
            {
                singleton2 = provider.GetService<ITestSingletonContract>();
            }
            singleton1.ShouldNotBeNull();
            singleton2.ShouldNotBeNull();
            singleton1.ShouldBe(singleton2);

            //Scoped
            ITestScopedContract scoped1, scoped2, scoped3;
            using (var scope = provider.CreateScope())
            {
                scoped1 = scope.ServiceProvider.GetService<ITestScopedContract>();
                scoped2 = scope.ServiceProvider.GetService<ITestScopedContract>();
            }
            using (var scope = provider.CreateScope())
            {
                scoped3 = scope.ServiceProvider.GetService<ITestScopedContract>();
            }
            scoped1.ShouldNotBeNull();
            scoped2.ShouldNotBeNull();
            scoped3.ShouldNotBeNull();
            scoped1.ShouldBe(scoped2);
            scoped1.ShouldNotBe(scoped3);
            scoped2.ShouldNotBe(scoped3);

            //Transient
            ITestTransientContract transient1, transient2;
            using (var scope = provider.CreateScope())
            {
                transient1 = scope.ServiceProvider.GetService<ITestTransientContract>();
                transient2 = scope.ServiceProvider.GetService<ITestTransientContract>();
            }
            transient1.ShouldNotBeNull();
            transient2.ShouldNotBeNull();
            transient1.ShouldNotBe(transient2);
        }
    }

    public interface ITestSingletonContract { }

    public class TestSingletonService : ITestSingletonContract, ISingletonDependency { }

    public interface ITestScopedContract { }

    public class TestScopedService : ITestScopedContract, IScopeDependency { }

    public interface ITestTransientContract { }

    public class TestTransientService : ITestTransientContract, ITransientDependency { }
}
