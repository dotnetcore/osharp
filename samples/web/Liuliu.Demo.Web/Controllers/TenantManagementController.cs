using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSharp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liuliu.Demo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Administrator")] // 确保只有管理员可以管理租户
    public class TenantManagementController : ControllerBase
    {
        private readonly ITenantStore _tenantStore;
        private readonly ILogger<TenantManagementController> _logger;

        public TenantManagementController(
            ITenantStore tenantStore,
            ILogger<TenantManagementController> logger)
        {
            _tenantStore = tenantStore;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有租户
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            var tenants = await _tenantStore.GetAllTenantsAsync();
            return Ok(tenants);
        }

        /// <summary>
        /// 获取指定租户
        /// </summary>
        [HttpGet("{tenantId}")]
        public async Task<IActionResult> GetTenant(string tenantId)
        {
            var tenant = await _tenantStore.GetTenantAsync(tenantId);
            if (tenant == null)
            {
                return NotFound();
            }

            return Ok(tenant);
        }

        /// <summary>
        /// 创建或更新租户
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateTenant([FromBody] TenantInfo tenant)
        {
            if (tenant == null || string.IsNullOrEmpty(tenant.TenantId))
            {
                return BadRequest("租户ID不能为空");
            }

            if (string.IsNullOrEmpty(tenant.Name))
            {
                return BadRequest("租户名称不能为空");
            }

            if (string.IsNullOrEmpty(tenant.Host))
            {
                return BadRequest("租户主机名不能为空");
            }

            bool success = await _tenantStore.SaveTenantAsync(tenant);
            if (!success)
            {
                return StatusCode(500, "保存租户信息失败");
            }

            return Ok(tenant);
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        [HttpDelete("{tenantId}")]
        public async Task<IActionResult> DeleteTenant(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest("租户ID不能为空");
            }

            bool success = await _tenantStore.DeleteTenantAsync(tenantId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
} 