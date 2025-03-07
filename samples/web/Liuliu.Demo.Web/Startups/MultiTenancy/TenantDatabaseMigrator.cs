using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy;
using Liuliu.Demo.MultiTenancy.Dtos;
using Liuliu.Demo.MultiTenancy.Entities;
using Liuliu.Demo.Web.Startups;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;
using OSharp.Authorization.EntityInfos;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Entity;

namespace Liuliu.Demo.Web.Startups
{
    public class TenantDatabaseMigrator
    {
        public TenantDatabaseMigrator()
        {
        }
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TenantDatabaseMigrator> _logger;

        public TenantDatabaseMigrator(
            IServiceProvider serviceProvider,
            ILogger<TenantDatabaseMigrator> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 迁移所有租户数据库
        /// </summary>
        public void MigrateAllTenants()
        {
            // 获取所有租户
            var tenants = new List<TenantOutputDto>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var _multiTenancyContract = scope.ServiceProvider.GetService<IMultiTenancyContract>();
                tenants = _multiTenancyContract.GetAllTenants().Where(p => p.TenantKey != "Default").ToList();
            }

            foreach (var tenant in tenants)
            {
                try
                {
                    MigrateTenantDatabase(tenant);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "迁移租户 {TenantId} 的数据库时出错", tenant.TenantKey);
                }
            }
        }

        /// <summary>
        /// 迁移指定租户的数据库
        /// </summary>
        public void MigrateTenantDatabase(TenantOutputDto tenant)
        {
            if (tenant == null || !tenant.IsEnabled)
            {
                return;
            }

            _logger.LogInformation("开始迁移租户 {TenantId} 的数据库", tenant.TenantKey);

            // 创建一个新的作用域，以便我们可以使用租户特定的服务
            using (var scope = _serviceProvider.CreateScope())
            {
                // 设置当前租户
                var tenantAccessor = scope.ServiceProvider.GetRequiredService<ITenantAccessor>();
                tenantAccessor.CurrentTenant = tenant;

                // 获取 DbContext 并执行迁移
                var dbContext = new DesignTimeDefaultDbContextFactory(scope.ServiceProvider).CreateDbContext(new string[0]);//scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
                ILogger logger = _serviceProvider.GetLogger(GetType());
                dbContext.CheckAndMigration(logger);
            }
            InitializeSeedData(tenant);

            InitFrameWorkData(tenant);
        }
        /// <summary>
        /// 初始化种子数据
        /// </summary>
        private void InitializeSeedData(TenantOutputDto tenant)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                //初始化种子数据，只初始化当前上下文的种子数据
                IEntityManager entityManager = scope.ServiceProvider.GetRequiredService<IEntityManager>();
                Type[] entityTypes = entityManager.GetEntityRegisters(typeof(DefaultDbContext)).Select(m => m.EntityType).Distinct().ToArray();
                IEnumerable<ISeedDataInitializer> seedDataInitializers = scope.ServiceProvider.GetServices<ISeedDataInitializer>()
                    .Where(m => entityTypes.Contains(m.EntityType)).OrderBy(m => m.Order);
                try
                {
                    ITenantAccessor tenantAccessor = scope.ServiceProvider.GetRequiredService<ITenantAccessor>();
                    tenantAccessor.CurrentTenant = tenant;
                    foreach (ISeedDataInitializer initializer in seedDataInitializers)
                    {
                        initializer.Initialize();
                    }

                    var csp = scope.ServiceProvider.GetRequiredService<IConnectionStringProvider>();
                    var str = csp.GetConnectionString(typeof(DefaultDbContext));
                    _logger.LogInformation("IConnectionStringProvider 链接：" + str);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "初始化种子数据时出错" + ex.Message);
                }
            }
        }

        private void InitFrameWorkData(TenantOutputDto tenant)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                ITenantAccessor tenantAccessor = scope.ServiceProvider.GetRequiredService<ITenantAccessor>();
                tenantAccessor.CurrentTenant = tenant;

                IFunctionHandler functionHandler = scope.ServiceProvider.GetServices<IFunctionHandler>().FirstOrDefault(m => m.GetType() == typeof(MvcFunctionHandler));
                if (functionHandler != null)
                {
                    functionHandler.Initialize();
                }

                IModuleHandler moduleHandler = scope.ServiceProvider.GetRequiredService<IModuleHandler>();
                moduleHandler.Initialize();

                //IFunctionAuthCache functionAuthCache = scope.ServiceProvider.GetRequiredService<IFunctionAuthCache>();
                //functionAuthCache.BuildRoleCaches();

                IEntityInfoHandler entityInfoHandler = scope.ServiceProvider.GetRequiredService<IEntityInfoHandler>();
                entityInfoHandler.Initialize();
            }
        }
    }
}
