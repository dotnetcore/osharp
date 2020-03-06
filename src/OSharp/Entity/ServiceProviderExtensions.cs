using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Data;


namespace OSharp.Entity
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 开启一个事务处理
        /// </summary>
        /// <param name="provider">信赖注入服务提供程序</param>
        /// <param name="action">要执行的业务委托</param>
        /// <param name="createScope">是否创建一个新的<see cref="IServiceScope"/>，如false，则使用传入的 provider</param>
        public static void BeginUnitOfWorkTransaction(this IServiceProvider provider, Action<IServiceProvider> action, bool createScope = false)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(action, nameof(action));
            if (!createScope)
            {
                IServiceProvider scopeProvider = provider;
                IUnitOfWorkManager unitOfWorkManager = scopeProvider.GetService<IUnitOfWorkManager>();
                action(scopeProvider);
                unitOfWorkManager.Commit();
            }
            else
            {
                using IServiceScope scope = provider.CreateScope();
                IServiceProvider scopeProvider = scope.ServiceProvider;
                IUnitOfWorkManager unitOfWorkManager = scopeProvider.GetService<IUnitOfWorkManager>();
                action(scopeProvider);
                unitOfWorkManager.Commit();
            }

        }

        /// <summary>
        /// 开启一个事务处理
        /// </summary>
        /// <param name="provider">信赖注入服务提供程序</param>
        /// <param name="actionAsync">要执行的业务委托</param>
        /// <param name="createScope">是否创建一个新的<see cref="IServiceScope"/>，如false，则使用传入的 provider</param>
        public static async Task BeginUnitOfWorkTransactionAsync(this IServiceProvider provider, Func<IServiceProvider, Task> actionAsync, bool createScope = false)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(actionAsync, nameof(actionAsync));
            IServiceProvider scopeProvider = provider;
            if (createScope)
            {
                using IServiceScope scope = provider.CreateScope();
                scopeProvider = scope.ServiceProvider;
            }

            IUnitOfWorkManager unitOfWorkManager = scopeProvider.GetService<IUnitOfWorkManager>();
            await actionAsync(scopeProvider);
            unitOfWorkManager.Commit();
        }
    }
}
