using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using NSubstitute;

using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Entities;
using OSharp.Entity;

using Xunit;


namespace OSharp.Demo.Core.Tests
{
    public class IdentityTests
    {
        [Fact]
        public void StartupTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddOSharp();
            services.Replace(ServiceDescriptor.Scoped(p => Substitute.For<IUnitOfWork>()));
            services.Replace(ServiceDescriptor.Scoped(p => Substitute.For<IRepository<User, int>>()));
            services.Replace(ServiceDescriptor.Scoped(p => Substitute.For<IRepository<UserLogin, Guid>>()));
            services.Replace(ServiceDescriptor.Scoped(p => Substitute.For<IRepository<UserClaim, int>>()));
            services.Replace(ServiceDescriptor.Scoped(p => Substitute.For<IRepository<UserToken, Guid>>()));
            services.Replace(ServiceDescriptor.Scoped(p => Substitute.For<IRepository<Role, int>>()));
            services.Replace(ServiceDescriptor.Scoped(p => Substitute.For<IRepository<UserRole, Guid>>()));
            services.AddIdentity<User, Role>();
            services.AddLogging();
            services.AddScoped<IUserStore<User>, UserStore>();

            IServiceProvider provider = services.BuildServiceProvider();
            UserManager<User> userManager = provider.GetService<UserManager<User>>();
            Assert.NotNull(userManager);
        }

    }
}