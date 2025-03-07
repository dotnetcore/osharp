using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Dtos;

namespace Liuliu.Demo.MultiTenancy
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
        TenantOutputDto GetCurrentTenant();

        /// <summary>
        /// 根据租户标识获取租户信息
        /// </summary>
        /// <param name="tenantKey">租户标识</param>
        /// <returns>租户信息</returns>
        TenantOutputDto GetTenant(string tenantKey);
    }
}
