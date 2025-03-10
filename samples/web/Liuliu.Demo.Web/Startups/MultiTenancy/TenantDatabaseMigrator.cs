using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy;
using Liuliu.Demo.MultiTenancy.Dtos;
using Liuliu.Demo.MultiTenancy.Entities;
using Liuliu.Demo.Web.Startups;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;
using OSharp.Authorization.EntityInfos;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Data;
using OSharp.Entity;

namespace Liuliu.Demo.Web.Startups
{
    public class TenantDatabaseMigrator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TenantDatabaseMigrator> _logger;

        private object tenantMigrateLockObj = new object();

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
        public OperationResult MigrateAllTenants()
        {
            // 获取所有租户
            var tenants = new List<TenantOutputDto>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var _multiTenancyContract = scope.ServiceProvider.GetService<IMultiTenancyContract>();
                tenants = _multiTenancyContract.GetAllTenants().Where(p => p.TenantKey != "Default").ToList();
            }

            var count = 0;
            var errList = new List<string>();
            foreach (var tenant in tenants)
            {
                 var result =  MigrateTenantDatabase(tenant);
                if (result.Succeeded)
                    count++;
                else
                    errList.Add(result.Message);
            }

            return new OperationResult(OperationResultType.Success, "迁移完成" + count + "/" + tenants.Count, errList);
        }

        /// <summary>
        /// 迁移指定租户的数据库
        /// </summary>
        public OperationResult MigrateTenant(TenantOutputDto tenant)
        {
            lock (tenantMigrateLockObj)
            {
                if (tenant == null)
                {
                    _logger.LogDebug("租户迁移失败:未指定租户");
                    return new OperationResult(OperationResultType.Error, "未指定租户");
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    var key = "MultiTenancy:RunTime:"+tenant.TenantKey;
                    var _cache = scope.ServiceProvider.GetService<IDistributedCache>();
                    _cache.Set(key, System.DateTime.Now);
                }

                if (!tenant.IsEnabled)
                {
                    _logger.LogDebug("租户 " + tenant.TenantKey + " 已被禁用");
                    return new OperationResult(OperationResultType.Error, "租户 " + tenant.TenantKey + " 已被禁用");
                }

                var result = MigrateTenantDatabase(tenant);
                if (!result.Succeeded)
                    return result;
                result = InitializeSeedData(tenant);
                if (!result.Succeeded)
                    return result;
                result = InitFrameWorkData(tenant);
                return result;
            }
        }

        /// <summary>
        /// 迁移指定租户的数据库
        /// </summary>
        private OperationResult MigrateTenantDatabase(TenantOutputDto tenant)
        {
            _logger.LogDebug("租户 "+ tenant.TenantKey + " 启动数据库迁移");
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogError("租户 "+ tenant.TenantKey + " 数据库迁移失败");
                return new OperationResult(OperationResultType.Error, "租户 " + tenant.TenantKey + " 数据库迁移失败:" + ex.Message);
            }
            
            _logger.LogDebug("租户 "+ tenant.TenantKey + " 的数据库迁移完成");
            return new OperationResult(OperationResultType.Success);
        }

        /// <summary>
        /// 初始化种子数据
        /// </summary>
        private OperationResult InitializeSeedData(TenantOutputDto tenant)
        {
            _logger.LogDebug("租户 " + tenant.TenantKey + " 种子数据开始初始化种子数据");
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
                    _logger.LogError("租户 " + tenant.TenantKey + " 种子数据初始化失败",ex);
                    return new OperationResult(OperationResultType.Error, "租户 " + tenant.TenantKey + " 种子数据初始化失败:" + ex.Message);
                }
            }
            return new OperationResult(OperationResultType.Success);
        }

        private OperationResult InitFrameWorkData(TenantOutputDto tenant)
        {
            _logger.LogDebug("租户 " + tenant.TenantKey + " 缓存数据初始化开始");
            try
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

                    IFunctionAuthCache functionAuthCache = scope.ServiceProvider.GetRequiredService<IFunctionAuthCache>();
                    functionAuthCache.BuildRoleCaches();

                    IEntityInfoHandler entityInfoHandler = scope.ServiceProvider.GetRequiredService<IEntityInfoHandler>();
                    entityInfoHandler.Initialize();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("租户 " + tenant.TenantKey + " 缓存数据初始化失败", ex);
                return new OperationResult(OperationResultType.Error, "租户 " + tenant.TenantKey + " 缓存数据初始化开始:" + ex.Message);
            }
            return new OperationResult(OperationResultType.Success);
        }
    }
}
