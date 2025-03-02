using Microsoft.EntityFrameworkCore;
using OSharp.Entity;
using OSharp.MultiTenancy;

namespace OSharp.Entity
{
    /// <summary>
    /// 租户数据库上下文
    /// </summary>
    public class TenantDbContext : DbContextBase
    {
        /// <summary>
        /// 初始化一个<see cref="TenantDbContext"/>类型的新实例
        /// </summary>
        public TenantDbContext(DbContextOptions<TenantDbContext> options, IServiceProvider serviceProvider)
            : base(options, serviceProvider)
        {
        }
    }
} 
