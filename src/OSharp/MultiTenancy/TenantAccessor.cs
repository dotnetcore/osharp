using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    // 租户访问器实现
    public class TenantAccessor : ITenantAccessor
    {
        public TenantInfo CurrentTenant { get; set; }
    }

    /// <summary>
    /// 使用 AsyncLocal<T> 实现的租户信息访问器
    /// </summary>
    public class AsyncLocalTenantAccessor : ITenantAccessor
    {
        // 使用 AsyncLocal<T> 存储租户信息
        private static readonly AsyncLocal<TenantInfo> _currentTenant = new AsyncLocal<TenantInfo>();

        /// <summary>
        /// 获取或设置当前租户
        /// </summary>
        public TenantInfo CurrentTenant
        {
            get => _currentTenant.Value;
            set => _currentTenant.Value = value;
        }
    }
}
