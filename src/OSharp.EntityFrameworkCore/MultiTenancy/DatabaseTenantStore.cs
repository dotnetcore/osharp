using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace OSharp.Entity
{
    /// <summary>
    /// 基于数据库的租户存储实现
    /// </summary>
    public class DatabaseTenantStore : ITenantStore
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DatabaseTenantStore> _logger;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);
        private const string CacheKeyPrefix = "Tenant_";
        private const string AllTenantsCacheKey = "AllTenants";
        private readonly IConfiguration _configuration;

        public DatabaseTenantStore(
            IServiceProvider serviceProvider,
            IMemoryCache cache,
            IConfiguration configuration,
            ILogger<DatabaseTenantStore> logger)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// 获取所有启用的租户
        /// </summary>
        public async Task<IEnumerable<TenantInfo>> GetAllTenantsAsync()
        {
            // 尝试从缓存获取
            if (_cache.TryGetValue(AllTenantsCacheKey, out IEnumerable<TenantInfo> cachedTenants))
            {
                return cachedTenants;
            }

            // 从数据库获取
            using var scope = _serviceProvider.CreateScope();
            var TenantRepository = scope.ServiceProvider.GetRequiredService<IRepository<TenantEntity,Guid>>();
            var tenantEntities = await TenantRepository.QueryAsNoTracking().Where(t => t.IsEnabled).ToListAsync();

            if(tenantEntities.Count == 0)
            {
                await ImportFromConfigurationAsync(_configuration);
            }
            tenantEntities = await TenantRepository.QueryAsNoTracking().Where(t => t.IsEnabled).ToListAsync();

            var tenants = tenantEntities.Select(MapToTenantInfo).ToList();

            // 缓存结果
            _cache.Set(AllTenantsCacheKey, tenants, _cacheExpiration);

            return tenants;
        }

        /// <summary>
        /// 根据租户ID获取租户
        /// </summary>
        public async Task<TenantInfo> GetTenantAsync(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            // 尝试从缓存获取
            string cacheKey = $"{CacheKeyPrefix}{tenantId}";
            if (_cache.TryGetValue(cacheKey, out TenantInfo cachedTenant))
            {
                return cachedTenant;
            }

            // 从数据库获取
            using var scope = _serviceProvider.CreateScope();
            var TenantRepository = scope.ServiceProvider.GetRequiredService<IRepository<TenantEntity, Guid>>();
            var tenantEntity = await TenantRepository.QueryAsNoTracking().Where(t => t.TenantId == tenantId).FirstOrDefaultAsync();

            if (tenantEntity == null)
            {
                return null;
            }

            var tenant = MapToTenantInfo(tenantEntity);

            // 缓存结果
            _cache.Set(cacheKey, tenant, _cacheExpiration);

            return tenant;
        }

        /// <summary>
        /// 根据主机名获取租户
        /// </summary>
        public async Task<TenantInfo> GetTenantByHostAsync(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return null;
            }

            // 获取所有租户
            var allTenants = await GetAllTenantsAsync();

            // 查找匹配的租户
            return allTenants.FirstOrDefault(t =>
                t.Host.Equals(host, StringComparison.OrdinalIgnoreCase) ||
                host.EndsWith("." + t.Host, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 保存租户信息
        /// </summary>
        public async Task<bool> SaveTenantAsync(TenantInfo tenant)
        {
            if (tenant == null || string.IsNullOrEmpty(tenant.TenantId))
            {
                return false;
            }

            using var scope = _serviceProvider.CreateScope();
            var TenantRepository = scope.ServiceProvider.GetRequiredService<IRepository<TenantEntity, Guid>>();
            var existingEntity = await TenantRepository.Query().Where(t => t.TenantId == tenant.TenantId).FirstOrDefaultAsync();
            try
            {
                if (existingEntity == null)
                {
                    // 创建新租户
                    var newEntity = new TenantEntity
                    {
                        TenantId = tenant.TenantId,
                        Name = tenant.Name,
                        Host = tenant.Host,
                        ConnectionString = tenant.ConnectionString,
                        IsEnabled = tenant.IsEnabled,
                        CreatedTime = DateTime.Now
                    };

                    await TenantRepository.InsertAsync(newEntity);
                    _logger.LogInformation("创建新租户: {TenantId}, {Name}", tenant.TenantId, tenant.Name);
                }
                else
                {
                    // 更新现有租户
                    existingEntity.Name = tenant.Name;
                    existingEntity.Host = tenant.Host;
                    existingEntity.ConnectionString = tenant.ConnectionString;
                    existingEntity.IsEnabled = tenant.IsEnabled;
                    existingEntity.UpdatedTime = DateTime.Now;

                    await TenantRepository.UpdateAsync(existingEntity);
                    _logger.LogInformation("更新租户: {TenantId}, {Name}", tenant.TenantId, tenant.Name);
                }

                // 清除缓存
                InvalidateCache(tenant.TenantId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存租户信息失败: {TenantId}", tenant.TenantId);
                return false;
            }
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        public async Task<bool> DeleteTenantAsync(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return false;
            }

            using var scope = _serviceProvider.CreateScope();
            var TenantRepository = scope.ServiceProvider.GetRequiredService<IRepository<TenantEntity, Guid>>();
            var tenantEntity = await TenantRepository.Query().Where(t => t.TenantId == tenantId).FirstOrDefaultAsync();

            if (tenantEntity == null)
            {
                return false;
            }

            try
            {
                await TenantRepository.DeleteAsync(tenantEntity);

                // 清除缓存
                InvalidateCache(tenantId);

                _logger.LogInformation("删除租户: {TenantId}", tenantId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除租户失败: {TenantId}", tenantId);
                return false;
            }
        }

        /// <summary>
        /// 从配置导入租户信息到数据库
        /// </summary>
        public async Task<int> ImportFromConfigurationAsync(IConfiguration configuration)
        {
            var tenantsSection = configuration.GetSection("Tenants");
            if (!tenantsSection.Exists())
            {
                _logger.LogWarning("在配置中未找到 'Tenants' 节点");
                return 0;
            }

            int importCount = 0;
            foreach (var tenantSection in tenantsSection.GetChildren())
            {
                var tenant = new TenantInfo
                {
                    TenantId = tenantSection.Key,
                    Name = tenantSection["Name"],
                    Host = tenantSection["Host"],
                    ConnectionString = tenantSection["ConnectionString"],
                    IsEnabled = tenantSection.GetValue<bool>("IsEnabled", true)
                };

                // 如果ConnectionString为空，尝试从ConnectionStrings节点获取
                if (string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    string connectionStringKey = $"ConnectionStrings:{tenant.TenantId}:Default";
                    tenant.ConnectionString = configuration[connectionStringKey];
                    
                    // 如果仍然为空，尝试直接获取租户ID对应的连接字符串
                    if (string.IsNullOrEmpty(tenant.ConnectionString))
                    {
                        tenant.ConnectionString = configuration.GetConnectionString(tenant.TenantId);
                    }
                }

                if (!string.IsNullOrEmpty(tenant.TenantId) && !string.IsNullOrEmpty(tenant.Host))
                {
                    bool success = await SaveTenantAsync(tenant);
                    if (success)
                    {
                        importCount++;
                        _logger.LogInformation("已导入租户: {TenantId}, {Name}, {Host}", 
                            tenant.TenantId, tenant.Name, tenant.Host);
                    }
                }
                else
                {
                    _logger.LogWarning("租户配置不完整，已跳过: {TenantId}", tenant.TenantId);
                }
            }

            // 清除所有缓存
            _cache.Remove(AllTenantsCacheKey);

            return importCount;
        }

        /// <summary>
        /// 将实体映射为租户信息
        /// </summary>
        private TenantInfo MapToTenantInfo(TenantEntity entity)
        {
            return new TenantInfo
            {
                TenantId = entity.TenantId,
                Name = entity.Name,
                Host = entity.Host,
                ConnectionString = entity.ConnectionString,
                IsEnabled = entity.IsEnabled
            };
        }

        /// <summary>
        /// 使缓存失效
        /// </summary>
        private void InvalidateCache(string tenantId)
        {
            _cache.Remove($"{CacheKeyPrefix}{tenantId}");
            _cache.Remove(AllTenantsCacheKey);
        }
    }
} 
