using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSharp.Core.Options;
using OSharp.Entity;
using OSharp.Dependency;
using OSharp.MultiTenancy;

namespace OSharp.Entity
{
    public class MultiTenantConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ITenantProvider _tenantProvider;
        private readonly ITenantAccessor _tenantAccessor; // 添加 ITenantAccessor
        private readonly ILogger<MultiTenantConnectionStringProvider> _logger;

        public MultiTenantConnectionStringProvider(
            IConfiguration configuration,
            ITenantProvider tenantProvider,
            ITenantAccessor tenantAccessor, // 注入 ITenantAccessor
            ILogger<MultiTenantConnectionStringProvider> logger)
        {
            _configuration = configuration;
            _tenantProvider = tenantProvider;
            _tenantAccessor = tenantAccessor;
            _logger = logger;
        }

        public string GetConnectionString(string name)
        {
            try
            {
                // 首先尝试从 TenantAccessor 获取当前租户
                TenantInfo tenant = _tenantAccessor.CurrentTenant;

                // 如果 TenantAccessor 中没有租户，则从 TenantProvider 获取
                if (tenant == null)
                {
                    tenant = _tenantProvider.GetCurrentTenant();
                }

                // 记录日志，帮助调试
                _logger.LogDebug("GetConnectionString(name: {Name}) - Current tenant: {TenantId}",
                    name, tenant?.TenantId ?? "null");

                // 如果没有租户信息，返回默认连接字符串
                if (tenant == null)
                {
                    string defaultConnectionString = _configuration.GetConnectionString(name);
                    _logger.LogDebug("Using default connection string for {Name}: {ConnectionString}",
                        name, defaultConnectionString);
                    return defaultConnectionString;
                }

                // 如果租户有指定的连接字符串，则使用租户的连接字符串
                if (!string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    _logger.LogDebug("Using tenant's connection string for {TenantId}: {ConnectionString}",
                        tenant.TenantId, tenant.ConnectionString);
                    return tenant.ConnectionString;
                }

                // 尝试从配置中获取特定租户的连接字符串
                string tenantConnectionStringKey = $"ConnectionStrings:{tenant.TenantId}:{name}";
                string connectionString = _configuration[tenantConnectionStringKey];

                // 如果没有找到特定租户的连接字符串，则尝试使用租户ID作为连接字符串名称
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration.GetConnectionString(tenant.TenantId);
                }

                // 如果仍然没有找到连接字符串，则使用默认连接字符串
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration.GetConnectionString(name);
                    _logger.LogDebug("Using default connection string for tenant {TenantId}: {ConnectionString}",
                        tenant.TenantId, connectionString);
                }
                else
                {
                    _logger.LogDebug("Using tenant-specific connection string for {TenantId}: {ConnectionString}",
                        tenant.TenantId, connectionString);
                }

                return connectionString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting connection string for {Name}", name);
                // 出错时返回默认连接字符串
                return _configuration.GetConnectionString(name);
            }
        }

        public string GetConnectionString(Type dbContextType)
        {
            try
            {
                if (dbContextType == null)
                {
                    throw new ArgumentNullException(nameof(dbContextType));
                }

                // 首先尝试从 TenantAccessor 获取当前租户
                TenantInfo tenant = _tenantAccessor.CurrentTenant;

                // 如果 TenantAccessor 中没有租户，则从 TenantProvider 获取
                if (tenant == null)
                {
                    tenant = _tenantProvider.GetCurrentTenant();
                }

                // 记录日志，帮助调试
                _logger.LogDebug("GetConnectionString(dbContextType: {DbContextType}) - Current tenant: {TenantId}",
                    dbContextType.Name, tenant?.TenantId ?? "null");

                // 使用 DbContext 类型的名称作为连接字符串名称
                string connectionStringName = dbContextType.Name;

                // 如果名称以 "DbContext" 结尾，则移除它
                if (connectionStringName.EndsWith("DbContext", StringComparison.OrdinalIgnoreCase))
                {
                    connectionStringName = connectionStringName.Substring(0, connectionStringName.Length - "DbContext".Length);
                }

                // 如果没有租户信息，返回默认连接字符串
                if (tenant == null)
                {
                    string defaultConnectionString = _configuration.GetConnectionString(connectionStringName);
                    if (string.IsNullOrEmpty(defaultConnectionString))
                    {
                        // 尝试使用 "Default" 作为连接字符串名称
                        defaultConnectionString = _configuration.GetConnectionString("Default");
                    }

                    _logger.LogDebug("Using default connection string for {DbContextType}: {ConnectionString}",
                        dbContextType.Name, defaultConnectionString);
                    return defaultConnectionString;
                }

                // 如果租户有指定的连接字符串，则使用租户的连接字符串
                if (!string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    _logger.LogDebug("Using tenant's connection string for {TenantId}: {ConnectionString}",
                        tenant.TenantId, tenant.ConnectionString);
                    return tenant.ConnectionString;
                }

                // 尝试从配置中获取特定租户的连接字符串
                string tenantConnectionStringKey = $"ConnectionStrings:{tenant.TenantId}:{connectionStringName}";
                string connectionString = _configuration[tenantConnectionStringKey];

                // 如果没有找到特定租户的连接字符串，则尝试使用租户ID作为连接字符串名称
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration.GetConnectionString(tenant.TenantId);
                }

                // 如果仍然没有找到连接字符串，则使用默认连接字符串
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration.GetConnectionString(connectionStringName);

                    // 如果仍然没有找到连接字符串，则尝试使用 "Default" 作为连接字符串名称
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        connectionString = _configuration.GetConnectionString("Default");
                    }

                    _logger.LogDebug("Using default connection string for tenant {TenantId}: {ConnectionString}",
                        tenant.TenantId, connectionString);
                }
                else
                {
                    _logger.LogDebug("Using tenant-specific connection string for {TenantId}: {ConnectionString}",
                        tenant.TenantId, connectionString);
                }

                return connectionString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting connection string for {DbContextType}", dbContextType.Name);

                // 出错时返回默认连接字符串
                string connectionStringName = dbContextType.Name;
                if (connectionStringName.EndsWith("DbContext", StringComparison.OrdinalIgnoreCase))
                {
                    connectionStringName = connectionStringName.Substring(0, connectionStringName.Length - "DbContext".Length);
                }

                string defaultConnectionString = _configuration.GetConnectionString(connectionStringName);
                if (string.IsNullOrEmpty(defaultConnectionString))
                {
                    defaultConnectionString = _configuration.GetConnectionString("Default");
                }

                return defaultConnectionString;
            }
        }
    }
}
