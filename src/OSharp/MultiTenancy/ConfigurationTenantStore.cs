using Microsoft.Extensions.Configuration;
using OSharp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 基于配置文件的租户存储实现
    /// </summary>
    public class ConfigurationTenantStore : ITenantStore
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConfigurationTenantStore> _logger;
        private readonly Dictionary<string, TenantInfo> _tenants = new Dictionary<string, TenantInfo>();

        public ConfigurationTenantStore(
            IConfiguration configuration,
            ILogger<ConfigurationTenantStore> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // 从配置中加载租户信息
            LoadTenantsFromConfiguration();
        }

        public Task<bool> DeleteTenantAsync(string tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TenantInfo>> GetAllTenantsAsync()
        {
            return Task.FromResult(_tenants.Values.Where(t => t.IsEnabled).AsEnumerable());
        }

        public Task<TenantInfo> GetTenantAsync(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId) || !_tenants.TryGetValue(tenantId, out var tenant) || !tenant.IsEnabled)
            {
                return Task.FromResult<TenantInfo>(null);
            }

            return Task.FromResult(tenant);
        }

        public Task<TenantInfo> GetTenantByHostAsync(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return Task.FromResult<TenantInfo>(null);
            }

            var tenant = _tenants.Values
                .FirstOrDefault(t => t.IsEnabled &&
                    (t.Host.Equals(host, StringComparison.OrdinalIgnoreCase) ||
                     host.EndsWith("." + t.Host, StringComparison.OrdinalIgnoreCase)));

            return Task.FromResult(tenant);
        }

        public Task<bool> SaveTenantAsync(TenantInfo tenant)
        {
            // 配置文件实现通常是只读的，不支持保存
            _logger.LogWarning("ConfigurationTenantStore 不支持保存租户信息");
            return Task.FromResult(false);
        }

        private void LoadTenantsFromConfiguration()
        {
            var tenantsSection = _configuration.GetSection("Tenants");
            if (!tenantsSection.Exists())
            {
                _logger.LogWarning("在配置中未找到 'Tenants' 节点");
                return;
            }

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

                if (!string.IsNullOrEmpty(tenant.TenantId) && !string.IsNullOrEmpty(tenant.Host))
                {
                    _tenants[tenant.TenantId] = tenant;
                    _logger.LogInformation("已加载租户: {TenantId}, {Name}, {Host}", tenant.TenantId, tenant.Name, tenant.Host);
                }
                else
                {
                    _logger.LogWarning("租户配置不完整，已跳过: {TenantId}", tenant.TenantId);
                }
            }
        }
    }
}
