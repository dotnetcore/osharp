using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.MultiTenancy.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Entity;
using OSharp.EventBuses;

namespace Liuliu.Demo.MultiTenancy
{
    /// <summary>
    /// 业务实现基类：信息模块
    /// </summary>
    public partial class MultiTenancyService : IMultiTenancyContract
    {
        /// <summary>
        /// 获取或设置 服务提供者对象
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取或设置 日志对象
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 初始化一个<see cref="MultiTenancyService"/>类型的新实例
        /// </summary>
        public MultiTenancyService(IServiceProvider provider)
        {
            ServiceProvider = provider;
            Logger = provider.GetLogger(GetType());
        }

        #region 属性
        /// <summary>
        /// 获取或设置 站内信信息仓储对象
        /// </summary>
        protected IRepository<Tenant, long> TenantRepository => ServiceProvider.GetService<IRepository<Tenant, long>>();

        /// <summary>
        /// 系统缓存
        /// </summary>
        protected IDistributedCache Cache => ServiceProvider.GetService<IDistributedCache>();

        /// <summary>
        /// 获取 事件总线
        /// </summary>
        protected IEventBus EventBus => ServiceProvider.GetService<IEventBus>();

        #endregion
    }
}
