using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 定义租户存储
    /// </summary>
    public interface ITenantStore
    {
        /// <summary>
        /// 获取所有租户
        /// </summary>
        Task<IEnumerable<TenantInfo>> GetAllTenantsAsync();
        
        /// <summary>
        /// 根据租户ID获取租户
        /// </summary>
        Task<TenantInfo> GetTenantAsync(string tenantId);
        
        /// <summary>
        /// 根据主机名获取租户
        /// </summary>
        Task<TenantInfo> GetTenantByHostAsync(string host);
        
        /// <summary>
        /// 保存租户信息
        /// </summary>
        Task<bool> SaveTenantAsync(TenantInfo tenant);
        
        /// <summary>
        /// 删除租户
        /// </summary>
        Task<bool> DeleteTenantAsync(string tenantId);
    }
}
