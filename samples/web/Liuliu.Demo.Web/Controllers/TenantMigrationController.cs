using Liuliu.Demo.Web.Startups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSharp.Entity;
using OSharp.MultiTenancy;

namespace Liuliu.Demo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantMigrationController : SiteApiControllerBase
    {
        private readonly TenantDatabaseMigrator _migrator;
        private readonly ITenantStore _tenantStore;
        private readonly ILogger<TenantMigrationController> _logger;

        public TenantMigrationController(
            TenantDatabaseMigrator migrator,
            ITenantStore tenantStore,
            ILogger<TenantMigrationController> logger)
        {
            _migrator = migrator;
            _tenantStore = tenantStore;
            _logger = logger;
        }

        [HttpPost("migrate-all")]
        public async Task<IActionResult> MigrateAllTenants()
        {
            try
            {
                _logger.LogInformation("开始迁移所有租户数据库");

                var tenants = await _tenantStore.GetAllTenantsAsync();
                _logger.LogInformation("找到 {Count} 个租户需要迁移", tenants.Count());

                foreach (var tenant in tenants)
                {
                    _logger.LogInformation("开始迁移租户 {TenantId} 的数据库", tenant.TenantId);
                    await _migrator.MigrateTenantDatabaseAsync(tenant);
                }

                _logger.LogInformation("所有租户数据库迁移完成");

                return Ok(new { message = "所有租户数据库迁移完成" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "租户数据库迁移过程中发生错误");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("migrate/{tenantId}")]
        public async Task<IActionResult> MigrateTenant(string tenantId)
        {
            try
            {
                _logger.LogInformation("开始迁移租户 {TenantId} 的数据库", tenantId);

                var tenant = await _tenantStore.GetTenantAsync(tenantId);
                if (tenant == null)
                {
                    _logger.LogWarning("租户 {TenantId} 不存在", tenantId);
                    return NotFound(new { error = $"租户 {tenantId} 不存在" });
                }

                await _migrator.MigrateTenantDatabaseAsync(tenant);

                _logger.LogInformation("租户 {TenantId} 的数据库迁移完成", tenantId);

                return Ok(new { message = $"租户 {tenantId} 的数据库迁移完成" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "迁移租户 {TenantId} 的数据库时发生错误", tenantId);
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
