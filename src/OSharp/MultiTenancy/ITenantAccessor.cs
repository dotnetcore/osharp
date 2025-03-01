using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    // 租户访问器接口，用于在作用域内设置当前租户
    public interface ITenantAccessor
    {
        TenantInfo CurrentTenant { get; set; }
    }
}
