using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Hosting.MultiTenancy.Dtos;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Core.Options;
using OSharp.Entity;
using OSharp.Exceptions;

namespace OSharp.Hosting.MultiTenancy
{
    /// <summary>
    /// 租户数据库选择器，实现IConnectionStringProvider接口
    /// </summary>
    public class MultiTenantConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IDictionary<string, OsharpDbContextOptions> _dbContexts;
        private readonly ISlaveDatabaseSelector[] _slaveDatabaseSelectors;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IMasterSlaveSplitPolicy _masterSlavePolicy;

        /// <summary>
        /// 初始化一个<see cref="MultiTenantConnectionStringProvider"/>类型的新实例
        /// </summary>
        public MultiTenantConnectionStringProvider(IServiceProvider provider, ITenantAccessor tenantAccessor)
        {
            _dbContexts = provider.GetOSharpOptions().DbContexts;
            _masterSlavePolicy = provider.GetService<IMasterSlaveSplitPolicy>();
            _slaveDatabaseSelectors = provider.GetServices<ISlaveDatabaseSelector>().ToArray();
            _tenantAccessor = tenantAccessor;
        }

        /// <summary>
        /// 获取指定数据上下文类型的数据库连接字符串
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        public virtual string GetConnectionString(Type dbContextType)
        {
            if (dbContextType.Name == "DefaultDbContext")
            {
                // 获取当前租户
                TenantOutputDto tenant = _tenantAccessor.CurrentTenant;
                if (tenant != null)
                {
                    return tenant.ConnectionString;
                }
            }

            OsharpDbContextOptions dbContextOptions = _dbContexts.Values.FirstOrDefault(m => m.DbContextType == dbContextType);
            if (dbContextOptions == null)
            {
                throw new OsharpException($"数据上下文“{dbContextType}”的数据上下文配置信息不存在");
            }

            bool isSlave = _masterSlavePolicy.IsToSlaveDatabase(dbContextOptions);
            if (!isSlave)
            {
                return dbContextOptions.ConnectionString;
            }

            SlaveDatabaseOptions[] slaves = dbContextOptions.Slaves;
            ISlaveDatabaseSelector slaveDatabaseSelector = _slaveDatabaseSelectors.LastOrDefault(m => m.Name == dbContextOptions.SlaveSelectorName)
                ?? _slaveDatabaseSelectors.First(m => m.Name == "Weight");
            SlaveDatabaseOptions slave = slaveDatabaseSelector.Select(slaves);
            return slave.ConnectionString;
        }
    }
}
