using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 定义租户提供者
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// 获取当前租户信息
        /// </summary>
        /// <returns>租户信息</returns>
        TenantInfo GetCurrentTenant();

        /// <summary>
        /// 异步获取当前租户信息
        /// </summary>
        /// <returns>租户信息</returns>
        Task<TenantInfo> GetCurrentTenantAsync();

        /// <summary>
        /// 根据租户标识获取租户信息
        /// </summary>
        /// <param name="identifier">租户标识</param>
        /// <returns>租户信息</returns>
        TenantInfo GetTenant(string identifier);

        /// <summary>
        /// 异步根据租户标识获取租户信息
        /// </summary>
        /// <param name="identifier">租户标识</param>
        /// <returns>租户信息</returns>
        Task<TenantInfo> GetTenantAsync(string identifier);
    }
}
