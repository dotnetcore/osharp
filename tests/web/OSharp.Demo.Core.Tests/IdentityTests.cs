using System;
using System.Security.Authentication.ExtendedProtection;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Entities;
using OSharp.Entity;
using OSharp.Reflection;

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
            services.Replace(ServiceDescriptor.Scoped<IUnitOfWork, TestUnitOfWork>());
            services.AddIdentity<User, Role>();
            services.AddLogging();
            services.AddScoped<IUserStore<User>, UserStore>();

            IServiceProvider provider = services.BuildServiceProvider();
            UserManager<User> userManager = provider.GetService<UserManager<User>>();
            Assert.NotNull(userManager);
        }
        
        private class TestUnitOfWork : IUnitOfWork
        {
            #region Implementation of IDisposable

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            { }

            #endregion

            #region Implementation of IUnitOfWork

            /// <summary>
            /// 获取指定数据上下文类型<typeparamref name="TEntity"/>的实例
            /// </summary>
            /// <typeparam name="TEntity">实体类型</typeparam>
            /// <typeparam name="TKey">实体主键类型</typeparam>
            /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
            public IDbContext GetDbContext<TEntity, TKey>() where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
            {
                return new TestDbContext();
            }

            /// <summary>
            /// 提交当前上下文的事务更改
            /// </summary>
            public void Commit()
            { }

            #endregion
        }


        private class TestDbContext : DbContext, IDbContext
        { }
    }
}