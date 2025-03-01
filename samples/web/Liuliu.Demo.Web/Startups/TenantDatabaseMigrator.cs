using OSharp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Entity;
using Microsoft.EntityFrameworkCore;
using OSharp.Authorization.Modules;
using OSharp.Authorization.Functions;
using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;
using OSharp.Authorization.EntityInfos;

namespace Liuliu.Demo.Web.Startups
{
    public class TenantDatabaseMigrator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITenantStore _tenantStore;
        private readonly ILogger<TenantDatabaseMigrator> _logger;

        public TenantDatabaseMigrator(
            IServiceProvider serviceProvider,
            ITenantStore tenantStore,
            ILogger<TenantDatabaseMigrator> logger)
        {
            _serviceProvider = serviceProvider;
            _tenantStore = tenantStore;
            _logger = logger;
        }

        /// <summary>
        /// 迁移所有租户数据库
        /// </summary>
        public async Task MigrateAllTenantsAsync()
        {
            // 获取所有租户
            var tenants = await _tenantStore.GetAllTenantsAsync();

            foreach (var tenant in tenants)
            {
                try
                {
                    await MigrateTenantDatabaseAsync(tenant);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "迁移租户 {TenantId} 的数据库时出错", tenant.TenantId);
                }
            }
        }

        /// <summary>
        /// 迁移指定租户的数据库
        /// </summary>
        public async Task MigrateTenantDatabaseAsync(TenantInfo tenant)
        {
            if (tenant == null || !tenant.IsEnabled)
            {
                return;
            }

            _logger.LogInformation("开始迁移租户 {TenantId} 的数据库", tenant.TenantId);

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
            InitializeSeedDataAsync(tenant);

            InitFrameWorkData(tenant);
        }
        /// <summary>
        /// 初始化种子数据
        /// </summary>
        private void InitializeSeedDataAsync(TenantInfo tenant)
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

        private void InitFrameWorkData(TenantInfo tenant)
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
