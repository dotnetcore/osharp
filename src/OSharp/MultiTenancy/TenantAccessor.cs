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
}
