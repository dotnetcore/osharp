using OSharp.Dependency;
using OSharp.MultiTenancy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// 基于HTTP请求的租户提供者实现，支持多种方式识别租户
    /// </summary>
    //[Dependency(ServiceLifetime.Singleton)]
    public class HttpTenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, TenantInfo> _tenantCache = new ConcurrentDictionary<string, TenantInfo>();
        private readonly ILogger<HttpTenantProvider> _logger;

        // 租户识别方式配置
        private readonly TenantResolveOptions _resolveOptions;

        public HttpTenantProvider(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            ILogger<HttpTenantProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;

            // 初始化租户识别方式配置
            _resolveOptions = new TenantResolveOptions();
            ConfigureTenantResolveOptions();

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

            // 尝试使用多种方式识别租户
            TenantInfo tenant = null;

            // 按照优先级顺序尝试不同的租户识别方式
            foreach (var resolver in _resolveOptions.Resolvers.OrderBy(r => r.Priority))
            {
                tenant = resolver.ResolveTenant(httpContext, _tenantCache);
                if (tenant != null)
                {
                    _logger.LogDebug("已通过 {ResolverName} 识别到租户: {TenantId}", 
                        resolver.GetType().Name, tenant.TenantId);
                    break;
                }
            }

            return tenant;
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
                    _logger.LogInformation("已加载租户: {TenantId}, {Name}, {Host}", 
                        tenant.TenantId, tenant.Name, tenant.Host);
                }
            }
        }

        /// <summary>
        /// 配置租户识别方式
        /// </summary>
        private void ConfigureTenantResolveOptions()
        {
            // 从配置中读取租户识别方式配置
            var resolveSection = _configuration.GetSection("MultiTenancy:TenantResolve");
            
            // 添加域名解析器（默认启用，优先级最高）
            bool enableDomain = resolveSection.GetValue<bool>("EnableDomain", true);
            if (enableDomain)
            {
                _resolveOptions.AddResolver(new DomainTenantResolver(), 100);
            }

            // 添加请求头解析器
            bool enableHeader = resolveSection.GetValue<bool>("EnableHeader", false);
            string headerName = resolveSection.GetValue<string>("HeaderName", "X-Tenant");
            if (enableHeader)
            {
                _resolveOptions.AddResolver(new HeaderTenantResolver(headerName), 200);
            }

            // 添加查询参数解析器
            bool enableQueryString = resolveSection.GetValue<bool>("EnableQueryString", false);
            string queryStringName = resolveSection.GetValue<string>("QueryStringName", "tenant");
            if (enableQueryString)
            {
                _resolveOptions.AddResolver(new QueryStringTenantResolver(queryStringName), 300);
            }

            // 添加Cookie解析器
            bool enableCookie = resolveSection.GetValue<bool>("EnableCookie", false);
            string cookieName = resolveSection.GetValue<string>("CookieName", "tenant");
            if (enableCookie)
            {
                _resolveOptions.AddResolver(new CookieTenantResolver(cookieName), 400);
            }

            // 添加Claims解析器
            bool enableClaim = resolveSection.GetValue<bool>("EnableClaim", false);
            string claimType = resolveSection.GetValue<string>("ClaimType", "tenant");
            if (enableClaim)
            {
                _resolveOptions.AddResolver(new ClaimTenantResolver(claimType), 500);
            }

            // 添加路由解析器
            bool enableRoute = resolveSection.GetValue<bool>("EnableRoute", false);
            string routeParamName = resolveSection.GetValue<string>("RouteParamName", "tenant");
            if (enableRoute)
            {
                _resolveOptions.AddResolver(new RouteTenantResolver(routeParamName), 600);
            }

            _logger.LogInformation("已配置租户识别方式: Domain={0}, Header={1}, QueryString={2}, Cookie={3}, Claim={4}, Route={5}",
                enableDomain, enableHeader, enableQueryString, enableCookie, enableClaim, enableRoute);
        }
    }

    /// <summary>
    /// 租户识别选项
    /// </summary>
    public class TenantResolveOptions
    {
        public List<ITenantResolver> Resolvers { get; } = new List<ITenantResolver>();

        public void AddResolver(ITenantResolver resolver, int priority)
        {
            resolver.Priority = priority;
            Resolvers.Add(resolver);
        }
    }

    /// <summary>
    /// 租户解析器接口
    /// </summary>
    public interface ITenantResolver
    {
        /// <summary>
        /// 优先级，数值越小优先级越高
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// 解析租户
        /// </summary>
        TenantInfo ResolveTenant(HttpContext context, ConcurrentDictionary<string, TenantInfo> tenantCache);
    }

    /// <summary>
    /// 基于域名的租户解析器
    /// </summary>
    public class DomainTenantResolver : ITenantResolver
    {
        public int Priority { get; set; }

        public TenantInfo ResolveTenant(HttpContext context, ConcurrentDictionary<string, TenantInfo> tenantCache)
        {
            string host = context.Request.Host.Host.ToLower();
            if (string.IsNullOrEmpty(host))
            {
                return null;
            }

            // 查找匹配的租户
            return tenantCache.Values.FirstOrDefault(t =>
                t.IsEnabled &&
                (t.Host.Equals(host, StringComparison.OrdinalIgnoreCase) ||
                 host.EndsWith("." + t.Host, StringComparison.OrdinalIgnoreCase)));
        }
    }

    /// <summary>
    /// 基于请求头的租户解析器
    /// </summary>
    public class HeaderTenantResolver : ITenantResolver
    {
        private readonly string _headerName;

        public HeaderTenantResolver(string headerName)
        {
            _headerName = headerName;
        }

        public int Priority { get; set; }

        public TenantInfo ResolveTenant(HttpContext context, ConcurrentDictionary<string, TenantInfo> tenantCache)
        {
            if (!context.Request.Headers.TryGetValue(_headerName, out var values) || values.Count == 0)
            {
                return null;
            }

            string tenantId = values.First();
            if (string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            tenantCache.TryGetValue(tenantId, out TenantInfo tenant);
            return tenant?.IsEnabled == true ? tenant : null;
        }
    }

    /// <summary>
    /// 基于查询参数的租户解析器
    /// </summary>
    public class QueryStringTenantResolver : ITenantResolver
    {
        private readonly string _paramName;

        public QueryStringTenantResolver(string paramName)
        {
            _paramName = paramName;
        }

        public int Priority { get; set; }

        public TenantInfo ResolveTenant(HttpContext context, ConcurrentDictionary<string, TenantInfo> tenantCache)
        {
            if (!context.Request.Query.TryGetValue(_paramName, out var values) || values.Count == 0)
            {
                return null;
            }

            string tenantId = values.First();
            if (string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            tenantCache.TryGetValue(tenantId, out TenantInfo tenant);
            return tenant?.IsEnabled == true ? tenant : null;
        }
    }

    /// <summary>
    /// 基于Cookie的租户解析器
    /// </summary>
    public class CookieTenantResolver : ITenantResolver
    {
        private readonly string _cookieName;

        public CookieTenantResolver(string cookieName)
        {
            _cookieName = cookieName;
        }

        public int Priority { get; set; }

        public TenantInfo ResolveTenant(HttpContext context, ConcurrentDictionary<string, TenantInfo> tenantCache)
        {
            if (!context.Request.Cookies.TryGetValue(_cookieName, out string tenantId) || string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            tenantCache.TryGetValue(tenantId, out TenantInfo tenant);
            return tenant?.IsEnabled == true ? tenant : null;
        }
    }

    /// <summary>
    /// 基于Claims的租户解析器
    /// </summary>
    public class ClaimTenantResolver : ITenantResolver
    {
        private readonly string _claimType;

        public ClaimTenantResolver(string claimType)
        {
            _claimType = claimType;
        }

        public int Priority { get; set; }

        public TenantInfo ResolveTenant(HttpContext context, ConcurrentDictionary<string, TenantInfo> tenantCache)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var claim = context.User.Claims.FirstOrDefault(c => c.Type == _claimType);
            if (claim == null || string.IsNullOrEmpty(claim.Value))
            {
                return null;
            }

            tenantCache.TryGetValue(claim.Value, out TenantInfo tenant);
            return tenant?.IsEnabled == true ? tenant : null;
        }
    }

    /// <summary>
    /// 基于路由参数的租户解析器
    /// </summary>
    public class RouteTenantResolver : ITenantResolver
    {
        private readonly string _routeParamName;

        public RouteTenantResolver(string routeParamName)
        {
            _routeParamName = routeParamName;
        }

        public int Priority { get; set; }

        public TenantInfo ResolveTenant(HttpContext context, ConcurrentDictionary<string, TenantInfo> tenantCache)
        {
            if (!context.Request.RouteValues.TryGetValue(_routeParamName, out var value) || value == null)
            {
                return null;
            }

            string tenantId = value.ToString();
            if (string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            tenantCache.TryGetValue(tenantId, out TenantInfo tenant);
            return tenant?.IsEnabled == true ? tenant : null;
        }
    }
}
