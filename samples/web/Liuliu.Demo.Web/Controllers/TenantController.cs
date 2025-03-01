using Microsoft.AspNetCore.Mvc;
using OSharp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Liuliu.Demo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ITenantStore _tenantStore;
        private readonly ILogger<TenantController> _logger;

        public TenantController(
            ITenantAccessor tenantAccessor,
            ITenantStore tenantStore,
            ILogger<TenantController> logger)
        {
            _tenantAccessor = tenantAccessor;
            _tenantStore = tenantStore;
            _logger = logger;
        }

        /// <summary>
        /// 获取当前租户信息
        /// </summary>
        [HttpGet("current")]
        public IActionResult GetCurrentTenant()
        {
            var tenant = _tenantAccessor.CurrentTenant;
            if (tenant == null)
            {
                return NotFound(new { message = "未找到当前租户信息" });
            }

            return Ok(new
            {
                tenant.TenantId,
                tenant.Name,
                tenant.Host,
                tenant.IsEnabled
            });
        }

        /// <summary>
        /// 获取所有租户信息
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            var tenants = await _tenantStore.GetAllTenantsAsync();
            return Ok(tenants.Select(t => new
            {
                t.TenantId,
                t.Name,
                t.Host,
                t.IsEnabled
            }));
        }

        /// <summary>
        /// 设置Cookie租户
        /// </summary>
        [HttpGet("set-cookie/{tenantId}")]
        public IActionResult SetCookieTenant(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest(new { message = "租户ID不能为空" });
            }

            // 设置租户Cookie
            Response.Cookies.Append("tenant", tenantId, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                Path = "/"
            });

            return Ok(new { message = $"已设置租户Cookie: {tenantId}" });
        }

        /// <summary>
        /// 清除Cookie租户
        /// </summary>
        [HttpGet("clear-cookie")]
        public IActionResult ClearCookieTenant()
        {
            Response.Cookies.Delete("tenant");
            return Ok(new { message = "已清除租户Cookie" });
        }
    }
} 