using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Liuliu.Demo.MultiTenancy
{
    /// <summary>
    /// 基于HTTP请求的租户提供者实现，支持多种方式识别租户
    /// </summary>
    public class HttpTenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HttpTenantProvider> _logger;
        private readonly ITenantAccessor _tenantAccessor; // 添加租户访问器
        private readonly IMultiTenancyContract _multiTenancyContract;  // 使用 ITenantStore

        // 租户识别方式配置
        private readonly TenantResolveOptions _resolveOptions;

        public HttpTenantProvider(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HttpTenantProvider> logger,
            IMultiTenancyContract multiTenancyContract,
            ITenantAccessor tenantAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _tenantAccessor = tenantAccessor;
            _multiTenancyContract = multiTenancyContract;

            // 初始化租户识别方式配置
            _resolveOptions = new TenantResolveOptions();
            ConfigureTenantResolveOptions();
        }

        /// <summary>
        /// 获取当前租户信息
        /// </summary>
        public TenantOutputDto GetCurrentTenant()
        {
            // 首先检查租户访问器中是否已有租户信息
            if (_tenantAccessor.CurrentTenant != null)
            {
                return _tenantAccessor.CurrentTenant;
            }

            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            // 尝试使用多种方式识别租户
            TenantOutputDto tenant = null;

            // 按照优先级顺序尝试不同的租户识别方式
            foreach (var resolver in _resolveOptions.Resolvers.OrderBy(r => r.Priority))
            {
                tenant = resolver.ResolveTenant(httpContext, _multiTenancyContract);
                if (tenant != null)
                {
                    // 将解析到的租户设置到租户访问器中
                    _tenantAccessor.CurrentTenant = tenant;
                    return tenant;
                }
            }

            if(tenant==null)
                return _multiTenancyContract.GetTenant("Default");

            return tenant;
        }

        /// <summary>
        /// 根据租户标识获取租户信息
        /// </summary>
        public TenantOutputDto GetTenant(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return _multiTenancyContract.GetTenant(identifier);
        }

        /// <summary>
        /// 配置租户识别方式
        /// </summary>
        private void ConfigureTenantResolveOptions()
        {
            bool enableDomain = true;
            bool enableHeader = true;
            string headerName = "X-Tenant";
            bool enableQueryString = true;
            string queryStringName = "tenant";
            bool enableCookie = true;
            string cookieName = "tenant";
            bool enableClaim = true;
            string claimType = "tenant";
            bool enableRoute = true;
            string routeParamName = "tenant";


            // 添加域名解析器（默认启用，优先级最高）
            if (enableDomain)
            {
                _resolveOptions.AddResolver(new DomainTenantResolver(), 100);
            }

            // 添加请求头解析器
            if (enableHeader)
            {
                _resolveOptions.AddResolver(new HeaderTenantResolver(headerName), 200);
            }

            // 添加查询参数解析器
            
            if (enableQueryString)
            {
                _resolveOptions.AddResolver(new QueryStringTenantResolver(queryStringName), 300);
            }

            // 添加Cookie解析器
            
            if (enableCookie)
            {
                _resolveOptions.AddResolver(new CookieTenantResolver(cookieName), 400);
            }

            // 添加Claims解析器
            
            if (enableClaim)
            {
                _resolveOptions.AddResolver(new ClaimTenantResolver(claimType), 500);
            }

            // 添加路由解析器
            
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
        TenantOutputDto ResolveTenant(HttpContext context, IMultiTenancyContract multiTenancyContract);
    }

    /// <summary>
    /// 基于域名的租户解析器
    /// </summary>
    public class DomainTenantResolver : ITenantResolver
    {
        public int Priority { get; set; }

        public TenantOutputDto ResolveTenant(HttpContext context, IMultiTenancyContract multiTenancyContract)
        {
            string host = context.Request.Host.Host.ToLower();
            if (string.IsNullOrEmpty(host))
            {
                return null;
            }

            // 获取所有租户并查找匹配的租户
            var tenants = multiTenancyContract.GetAllTenants();
            return tenants.FirstOrDefault(t =>
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

        public TenantOutputDto ResolveTenant(HttpContext context, IMultiTenancyContract multiTenancyContract)
        {
            if (!context.Request.Headers.TryGetValue(_headerName, out var values) || values.Count == 0)
            {
                return null;
            }

            string tenantKey = values.First();
            if (string.IsNullOrEmpty(tenantKey))
            {
                return null;
            }

            return multiTenancyContract.GetTenant(tenantKey);
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

        public TenantOutputDto ResolveTenant(HttpContext context, IMultiTenancyContract multiTenancyContract)
        {
            if (!context.Request.Query.TryGetValue(_paramName, out var values) || values.Count == 0)
            {
                return null;
            }

            string tenantKey = values.First();
            if (string.IsNullOrEmpty(tenantKey))
            {
                return null;
            }

            return multiTenancyContract.GetTenant(tenantKey);
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

        public TenantOutputDto ResolveTenant(HttpContext context, IMultiTenancyContract multiTenancyContract)
        {
            if (!context.Request.Cookies.TryGetValue(_cookieName, out string tenantKey) || string.IsNullOrEmpty(tenantKey))
            {
                return null;
            }

            return multiTenancyContract.GetTenant(tenantKey);
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

        public TenantOutputDto ResolveTenant(HttpContext context, IMultiTenancyContract multiTenancyContract)
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
            var tenantKey = claim.Value;
            return multiTenancyContract.GetTenant(tenantKey);
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

        public TenantOutputDto ResolveTenant(HttpContext context, IMultiTenancyContract multiTenancyContract)
        {
            if (!context.Request.RouteValues.TryGetValue(_routeParamName, out var value) || value == null)
            {
                return null;
            }

            string tenantKey = value.ToString();
            if (string.IsNullOrEmpty(tenantKey))
            {
                return null;
            }

            return multiTenancyContract.GetTenant(tenantKey);
        }
    }
}
