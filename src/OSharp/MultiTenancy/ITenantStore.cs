using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    public interface ITenantStore
    {
        Task<IEnumerable<TenantInfo>> GetAllTenantsAsync();
        Task<TenantInfo> GetTenantAsync(string tenantId);
    }
}
