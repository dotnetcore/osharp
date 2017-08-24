using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSharp.Dependency;
using OSharp.Entity;
using Shouldly;

namespace OSharp.Tests.MsTester
{
    [TestClass]
    public class StartupTests
    {
        [TestMethod]
        public void StartupTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddOSharp();

            IServiceProvider provider = services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                IUnitOfWork uow1 = scope.ServiceProvider.GetService<IUnitOfWork>();
                IUnitOfWork uow2 = scope.ServiceProvider.GetService<IUnitOfWork>();
                uow1.GetHashCode().ShouldBe(uow2.GetHashCode());                
            }
        }
    }
}
