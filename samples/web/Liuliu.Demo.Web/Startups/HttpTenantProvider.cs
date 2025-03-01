using OSharp.Dependency;
using OSharp.MultiTenancy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// 基于HTTP请求的租户提供者实现
    /// </summary>
    [Dependency(ServiceLifetime.Singleton)]
    public class HttpTenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, TenantInfo> _tenantCache = new ConcurrentDictionary<string, TenantInfo>();

        public HttpTenantProvider(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            // 初始化租户缓存
            InitializeTenantCache();
        }

        /// <summary>
        /// 获取当前租户信息
        /// </summary>
        public TenantInfo GetCurrentTenant()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            // 从请求的主机名中识别租户
            string host = httpContext.Request.Host.Host.ToLower();
            return GetTenantByHost(host);
        }

        /// <summary>
        /// 异步获取当前租户信息
        /// </summary>
        public Task<TenantInfo> GetCurrentTenantAsync()
        {
            return Task.FromResult(GetCurrentTenant());
        }

        /// <summary>
        /// 根据租户标识获取租户信息
        /// </summary>
        public TenantInfo GetTenant(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            // 尝试从缓存中获取租户信息
            if (_tenantCache.TryGetValue(identifier, out TenantInfo tenant))
            {
                return tenant.IsEnabled ? tenant : null;
            }

            return null;
        }

        /// <summary>
        /// 异步根据租户标识获取租户信息
        /// </summary>
        public Task<TenantInfo> GetTenantAsync(string identifier)
        {
            return Task.FromResult(GetTenant(identifier));
        }

        /// <summary>
        /// 根据主机名获取租户
        /// </summary>
        private TenantInfo GetTenantByHost(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return null;
            }

            // 查找匹配的租户
            return _tenantCache.Values.FirstOrDefault(t =>
                t.IsEnabled &&
                (t.Host.Equals(host, StringComparison.OrdinalIgnoreCase) ||
                 host.EndsWith("." + t.Host, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// 初始化租户缓存
        /// </summary>
        private void InitializeTenantCache()
        {
            // 从配置中加载租户信息
            var tenantsSection = _configuration.GetSection("Tenants");
            if (!tenantsSection.Exists())
            {
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
                    _tenantCache[tenant.TenantId] = tenant;
                }
            }
        }
    }
}
