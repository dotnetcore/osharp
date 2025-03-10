using System.ComponentModel;
using OSharp.Hosting.Identity.Entities;
using OSharp.Hosting.MultiTenancy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.Authorization;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Core.Packs;
using OSharp.Core.Systems;
using OSharp.Entity;
using OSharp.Identity;

namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// 多租户模块
    /// </summary>
    [Description("多租户模块")]
    public class MultiTenancyPack : OsharpPack
    {
        /// <summary>将模块服务添加到依赖注入服务容器中</summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddScoped<IMultiTenancyContract, MultiTenancyService>();
            services.AddSingleton<ISeedDataInitializer, MultiTenancySeedDataInitializer>();

            services.AddScoped<ITenantAccessor, AsyncLocalTenantAccessor>();
            services.AddScoped<ITenantProvider, HttpTenantProvider>();

            services.AddHttpContextAccessor(); // 注册 IHttpContextAccessor

            // 注册租户数据库迁移器
            services.AddSingleton<TenantDatabaseMigrator>();

            // 替换默认的连接字符串提供者
            services.Replace<IConnectionStringProvider, MultiTenantConnectionStringProvider>(ServiceLifetime.Scoped);

            // 涉及到租户缓存的实现进行替换
            services.Replace<IOnlineUserProvider, TenantOnlineUserProvider>(ServiceLifetime.Scoped);
            services.Replace<IKeyValueStore, TenantKeyValueStore>(ServiceLifetime.Scoped);
            services.Replace<ICacheService, TenantCacheService>(ServiceLifetime.Scoped);
            services.Replace<IFunctionAuthCache, TenantFunctionAuthCache>(ServiceLifetime.Singleton);
            services.Replace<IDataAuthCache, TenantDataAuthCache>(ServiceLifetime.Singleton);

            //功能上进行调整
            services.Replace<IModuleHandler, TenantModuleHandler>(ServiceLifetime.Singleton);

            return services;
        }
    }
}
