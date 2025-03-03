using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSharp.Entity;
using OSharp.MultiTenancy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Entity
{
    /// <summary>
    /// 租户数据库选择器，实现IConnectionStringProvider接口
    /// </summary>
    public class MultiTenantConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ILogger<MultiTenantConnectionStringProvider> _logger;
        private readonly ConcurrentDictionary<string, string> _connectionStringCache = new ConcurrentDictionary<string, string>();

        public MultiTenantConnectionStringProvider(
            IConfiguration configuration,
            ITenantAccessor tenantAccessor,
            ILogger<MultiTenantConnectionStringProvider> logger)
        {
            _configuration = configuration;
            _tenantAccessor = tenantAccessor;
            _logger = logger;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public string GetConnectionString(Type dbContextType)
        {
            if(dbContextType.Name == "TenantDbContext")
            {
                return _configuration.GetConnectionString("Tenant");
            }
            // 获取当前租户
            TenantInfo tenant = _tenantAccessor.CurrentTenant;
            
            // 获取DbContext的连接字符串名称
            string connectionStringName = dbContextType.Name.Replace("DbContext", "");
            
            // 构建缓存键
            string cacheKey = tenant?.TenantId + "_" + connectionStringName;
            
            // 尝试从缓存获取连接字符串
            if (!string.IsNullOrEmpty(cacheKey) && _connectionStringCache.TryGetValue(cacheKey, out string cachedConnectionString))
            {
                return cachedConnectionString;
            }

            string connectionString = null;

            // 如果有租户信息
            if (tenant != null)
            {
                // 1. 首先尝试使用租户自己的连接字符串
                if (!string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    connectionString = tenant.ConnectionString;
                    _logger.LogDebug("使用租户 {TenantId} 的连接字符串", tenant.TenantId);
                }
                else
                {
                    // 2. 尝试从配置中获取特定租户的特定DbContext连接字符串
                    string tenantConnectionStringKey = $"ConnectionStrings:{tenant.TenantId}:{connectionStringName}";
                    connectionString = _configuration[tenantConnectionStringKey];

                    // 3. 尝试从配置中获取特定租户的默认连接字符串
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        string tenantDefaultConnectionStringKey = $"ConnectionStrings:{tenant.TenantId}:Default";
                        connectionString = _configuration[tenantDefaultConnectionStringKey];
                        
                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            _logger.LogDebug("使用租户 {TenantId} 的默认连接字符串", tenant.TenantId);
                        }
                    }
                    else
                    {
                        _logger.LogDebug("使用租户 {TenantId} 的 {DbContext} 连接字符串", tenant.TenantId, connectionStringName);
                    }
                }
            }

            // 4. 如果仍未找到连接字符串，则使用应用程序的默认连接字符串
            if (string.IsNullOrEmpty(connectionString))
            {
                // 尝试获取特定DbContext的连接字符串
                connectionString = _configuration.GetConnectionString(connectionStringName);
                
                // 如果没有特定DbContext的连接字符串，则使用默认连接字符串
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration.GetConnectionString("Default");
                    _logger.LogDebug("使用应用程序默认连接字符串");
                }
                else
                {
                    _logger.LogDebug("使用应用程序 {DbContext} 连接字符串", connectionStringName);
                }
            }

            // 缓存连接字符串
            if (!string.IsNullOrEmpty(cacheKey) && !string.IsNullOrEmpty(connectionString))
            {
                _connectionStringCache[cacheKey] = connectionString;
            }

            return connectionString;
        }
    }
} 
