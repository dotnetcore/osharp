using OSharp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Entity;
using Microsoft.EntityFrameworkCore;

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

                // 获取实体管理器
                _logger.LogDebug("正在获取实体管理器");
                IEntityManager entityManager = scope.ServiceProvider.GetRequiredService<IEntityManager>();

                // 获取当前 DbContext 支持的实体类型
                _logger.LogDebug("正在获取 DbContext {DbContextType} 支持的实体类型", dbContext.GetType().Name);
                var entityRegisters = entityManager.GetEntityRegisters(dbContext.GetType()).ToList();
                _logger.LogDebug("找到 {Count} 个实体注册信息", entityRegisters.Count);

                Type[] entityTypes = entityRegisters.Select(m => m.EntityType).Distinct().ToArray();
                _logger.LogDebug("找到 {Count} 个不同的实体类型: {EntityTypes}",
                    entityTypes.Length,
                    string.Join(", ", entityTypes.Select(t => t.Name)));

                // 获取种子数据初始化器
                _logger.LogDebug("正在获取种子数据初始化器");
                var allInitializers = scope.ServiceProvider.GetServices<ISeedDataInitializer>().ToList();
                _logger.LogDebug("找到 {Count} 个种子数据初始化器", allInitializers.Count);

                IEnumerable<ISeedDataInitializer> seedDataInitializers = allInitializers
                    .Where(m => entityTypes.Contains(m.EntityType))
                    .OrderBy(m => m.Order);

                var filteredInitializers = seedDataInitializers.ToList();
                _logger.LogDebug("筛选出 {Count} 个适用于当前 DbContext 的种子数据初始化器", filteredInitializers.Count);

                if (filteredInitializers.Count == 0)
                {
                    _logger.LogWarning("没有找到适用于当前 DbContext 的种子数据初始化器，请检查实体类型和初始化器注册");
                }

                // 执行种子数据初始化
                _logger.LogInformation("开始初始化租户 {TenantId} 的种子数据", tenant.TenantId);
                int initializedCount = 0;
                foreach (ISeedDataInitializer initializer in filteredInitializers)
                {
                    try
                    {
                        _logger.LogDebug("正在执行种子数据初始化器: {InitializerType} 用于实体: {EntityType}",
                            initializer.GetType().Name, initializer.EntityType.Name);

                        // 使用新的作用域执行初始化器，确保每个初始化器都有正确的服务实例
                        using (var initializerScope = scope.ServiceProvider.CreateScope())
                        {
                            // 确保在新作用域中也设置当前租户
                            var initializerTenantAccessor = initializerScope.ServiceProvider.GetRequiredService<ITenantAccessor>();
                            initializerTenantAccessor.CurrentTenant = tenant;

                            // 创建初始化器实例
                            var initializerType = initializer.GetType();
                            var initializerInstance = ActivatorUtilities.CreateInstance(initializerScope.ServiceProvider, initializerType) as ISeedDataInitializer;

                            // 执行初始化
                            initializerInstance.Initialize();
                        }

                        initializedCount++;
                        _logger.LogDebug("种子数据初始化器 {InitializerType} 执行成功", initializer.GetType().Name);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "执行种子数据初始化器 {InitializerType} 时出错: {ErrorMessage}",
                            initializer.GetType().Name, ex.Message);
                    }
                }

                _logger.LogInformation("租户 {TenantId} 的种子数据初始化完成，共执行了 {Count} 个初始化器", tenant.TenantId, initializedCount);
            }
        }
    }
}
