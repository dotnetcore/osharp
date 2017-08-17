using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OSharp.Entity;
using OSharp.Entity.Defaults;
using Xunit;
using Shouldly;
using OSharp.Reflection;

namespace OSharp.EntityFrameworkCore.Tests
{
    public class StartupTest
    {
        [Fact]
        public void DbContextCreateTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IAllAssemblyFinder, BinAllAssemblyFinder>();
            services.AddSingleton<IEntityConfigurationAssemblyFinder, EntityConfigurationAssemblyFinder>();
            services.AddSingleton<IEntityConfigurationTypeFinder, EntityConfigurationTypeFinder>();
            services.AddScoped<IUnitOfWork, DefaultDbContext>(provider =>
            {
                IEntityConfigurationTypeFinder finder = provider.GetService<IEntityConfigurationTypeFinder>();
                var options = new DbContextOptionsBuilder<DefaultDbContext>().Options;
                return new DefaultDbContext(options, finder);
            });

            IServiceProvider provider1 = services.BuildServiceProvider();
            using (var scope = provider1.CreateScope())
            {
                IUnitOfWork uow1 = scope.ServiceProvider.GetService<IUnitOfWork>();
                IUnitOfWork uow2 = scope.ServiceProvider.GetService<IUnitOfWork>();
                uow1.GetHashCode().ShouldBe(uow2.GetHashCode());
            }
        }
    }
}
