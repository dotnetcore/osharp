using System.ComponentModel;
using Liuliu.Demo.MultiTenancy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.Core.Packs;
using OSharp.Entity;

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

            services.AddScoped<ITenantAccessor, AsyncLocalTenantAccessor>();
            services.AddScoped<ITenantProvider, HttpTenantProvider>();

            services.AddHttpContextAccessor(); // 注册 IHttpContextAccessor

            // 注册租户数据库迁移器
            services.AddSingleton<TenantDatabaseMigrator>();

            // 替换默认的连接字符串提供者
            services.Replace<IConnectionStringProvider, MultiTenantConnectionStringProvider>(ServiceLifetime.Scoped);

            return services;
        }
    }
}
