using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Dtos;

namespace Liuliu.Demo.MultiTenancy
{
    // 租户访问器实现
    public class TenantAccessor : ITenantAccessor
    {
        public TenantOutputDto CurrentTenant { get; set; }

        public string TenantCacheKeyPre
        { get
            {
                if (CurrentTenant == null)
                    return "Tenant:Default:";
                else
                    return "Tenant:" + CurrentTenant.TenantKey+":";
            }
        }
    }

    /// <summary>
    /// 使用 AsyncLocal实现的租户信息访问器
    /// </summary>
    public class AsyncLocalTenantAccessor : ITenantAccessor
    {
        // 使用 AsyncLocal<T> 存储租户信息
        private static readonly AsyncLocal<TenantOutputDto> _currentTenant = new AsyncLocal<TenantOutputDto>();

        /// <summary>
        /// 获取或设置当前租户
        /// </summary>
        public TenantOutputDto CurrentTenant
        {
            get => _currentTenant.Value;
            set => _currentTenant.Value = value;
        }

        public string TenantCacheKeyPre
        {
            get
            {
                if (CurrentTenant == null)
                    return "Tenant:Default:";
                else
                    return "Tenant:" + CurrentTenant.TenantKey+":";
            }
        }
    }
}
