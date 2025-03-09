using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy;
using Liuliu.Demo.MultiTenancy.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OSharp.Caching;

namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// 多租户中间件，用于在请求处理过程中设置当前租户
    /// </summary>
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantMiddleware> _logger;

        public TenantMiddleware(
            RequestDelegate next,
            ILogger<TenantMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider, ITenantAccessor tenantAccessor,IDistributedCache cache, TenantDatabaseMigrator migrator)
        {
            try
            {
                // 解析当前租户
                TenantOutputDto tenant = tenantProvider.GetCurrentTenant();

                // 设置当前租户到访问器中
                if (tenant != null)
                {
                    tenantAccessor.CurrentTenant = tenant;
                    _logger.LogDebug("已设置当前租户: {TenantKey}, {Name}", tenant.TenantKey, tenant.Name);

                    if (tenant.TenantKey != "Default")
                    {
                        var key = "MultiTenancy:RunTime:" + tenant.TenantKey;
                        var tenantRunTime = cache.Get<DateTime?>(key);
                        if (tenantRunTime == null)
                        {
                            migrator.MigrateTenant(tenant);
                        }
                        else
                        {
                            key = "MultiTenancy:RunTime:Default";
                            var defaultRunTime = cache.Get<DateTime?>(key);
                            if (tenantRunTime < defaultRunTime)
                            {
                                migrator.MigrateTenant(tenant);
                            }
                        }
                    }

                    if (tenant.ExpireDate!=null && ((System.DateTime)tenant.ExpireDate).AddDays(1) < System.DateTime.Now)
                    {
                        context.Response.StatusCode = 500; // 直接报错
                        await context.Response.WriteAsync("您的授权于["+((System.DateTime)tenant.ExpireDate).ToString("yyyy-MM-dd")+"到期]."); // 可以自定义错误信息
                        return;
                    }

                    // 可以在这里添加租户相关的请求头或其他信息
                    //context.Items["CurrentTenant"] = tenant;
                }
                else
                {
                    _logger.LogDebug("未识别到租户信息");
                    context.Response.StatusCode = 500; // 直接报错
                    await context.Response.WriteAsync("未识别到租户信息."); // 可以自定义错误信息
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理租户信息时发生错误");
                context.Response.StatusCode = 500; // 直接报错
                await context.Response.WriteAsync("处理租户信息时发生错误."); // 可以自定义错误信息
                return;
            }

            // 继续处理请求
            await _next(context);
        }
    }
}