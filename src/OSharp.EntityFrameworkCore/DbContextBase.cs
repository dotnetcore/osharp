// -----------------------------------------------------------------------
//  <copyright file="DbContextBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-08 4:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Audits;
using OSharp.Core.Options;
using OSharp.EventBuses;
using OSharp.Extensions;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// EntityFramework上下文基类
    /// </summary>
    public abstract class DbContextBase : DbContext, IDbContext
    {
        private readonly IEntityManager _entityManager;
        private readonly OsharpDbContextOptions _osharpDbOptions;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="DbContextBase"/>类型的新实例
        /// </summary>
        protected DbContextBase(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
            _entityManager = serviceProvider.GetService<IEntityManager>();
            _osharpDbOptions = serviceProvider?.GetOSharpOptions()?.DbContexts?.Values.FirstOrDefault(m => m.DbContextType == GetType());
            Logger = serviceProvider.GetLogger(this);
        }

        /// <summary>
        /// 获取 日志对象
        /// </summary>
        protected ILogger Logger { get; }
        
        /// <summary>
        ///     将在此上下文中所做的所有更改保存到数据库中，同时自动开启事务或使用现有同连接事务
        /// </summary>
        /// <remarks>
        ///     此方法将自动调用 <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> 
        ///     若要在保存到基础数据库之前发现对实体实例的任何更改，请执行以下操作。这可以通过以下类型禁用
        ///     <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        /// <returns>
        ///     写入数据库的状态项的数目。
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     保存到数据库时遇到错误。
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     保存到数据库时会遇到并发冲突。
        ///     当在保存期间影响到意外数量的行时，就会发生并发冲突。
        ///     这通常是因为数据库中的数据在加载到内存后已经被修改。
        /// </exception>
        public override int SaveChanges()
        {
            IList<AuditEntityEntry> auditEntities = new List<AuditEntityEntry>();
            if (_osharpDbOptions.AuditEntityEnabled == true)
            {
                IAuditEntityProvider auditEntityProvider = _serviceProvider.GetService<IAuditEntityProvider>();
                auditEntities = auditEntityProvider?.GetAuditEntities(this).ToList();
            }

            //开启或使用现有事务
            BeginOrUseTransaction();

            int count = base.SaveChanges();
            if (count > 0 && auditEntities?.Count > 0)
            {
                AuditEntityEventData eventData = new AuditEntityEventData(auditEntities);
                IEventBus eventBus = _serviceProvider.GetService<IEventBus>();
                eventBus?.Publish(this, eventData);
            }

            return count;
        }

        /// <summary>
        ///     异步地将此上下文中的所有更改保存到数据库中，同时自动开启事务或使用现有同连接事务
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         此方法将自动调用 <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> 
        ///         若要在保存到基础数据库之前发现对实体实例的任何更改，请执行以下操作。这可以通过以下类型禁用
        ///         <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         不支持同一上下文实例上的多个活动操作。请使用“await”确保在此上下文上调用其他方法之前任何异步操作都已完成。
        ///     </para>
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     表示异步保存操作的任务。任务结果包含写入数据库的状态条目数。
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     保存到数据库时遇到错误。
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     保存到数据库时会遇到并发冲突。
        ///     当在保存期间影响到意外数量的行时，就会发生并发冲突。
        ///     这通常是因为数据库中的数据在加载到内存后已经被修改。
        /// </exception>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            IList<AuditEntityEntry> auditEntities = new List<AuditEntityEntry>();
            if (_osharpDbOptions.AuditEntityEnabled == true)
            {
                IAuditEntityProvider auditEntityProvider = _serviceProvider.GetService<IAuditEntityProvider>();
                auditEntities = auditEntityProvider?.GetAuditEntities(this).ToList();
            }

            //开启或使用现有事务
#if NET5_0
            await BeginOrUseTransactionAsync(cancellationToken);
#else
            BeginOrUseTransaction();
#endif

            int count;
            try
            {
                count = await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                while (ex.InnerException != null)
                {
                    msg += $"---{ex.InnerException.Message}";
                    ex = ex.InnerException;
                }
                Logger.LogDebug($"SaveChangesAsync 引发异常：{msg}");
                throw;
            }
            if (count > 0 && auditEntities?.Count > 0)
            {
                AuditEntityEventData eventData = new AuditEntityEventData(auditEntities);
                IEventBus eventBus = _serviceProvider.GetService<IEventBus>();
                if (eventBus != null)
                {
                    await eventBus.PublishAsync(this, eventData);
                }
            }

            return count;
        }

        /// <summary>
        /// 开启或使用现有事务
        /// </summary>
        public void BeginOrUseTransaction()
        {
            IUnitOfWork unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
            unitOfWork?.BeginOrUseTransaction(this);
        }

#if NET5_0
        
        /// <summary>
        /// 异步开启或使用现有事务
        /// </summary>
        public async Task BeginOrUseTransactionAsync(CancellationToken cancellationToken)
        {
            IUnitOfWork unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
            if (unitOfWork != null)
            {
                await unitOfWork.BeginOrUseTransactionAsync(this, cancellationToken);
            }
        }
        
#endif
        /// <summary>
        /// 创建上下文数据模型时，对各个实体类的数据库映射细节进行配置
        /// </summary>
        /// <param name="modelBuilder">上下文数据模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //通过实体配置信息将实体注册到当前上下文
            Type contextType = GetType();
            IEntityRegister[] registers = _entityManager.GetEntityRegisters(contextType);
            foreach (IEntityRegister register in registers)
            {
                register.RegisterTo(modelBuilder);
                Logger.LogDebug($"将实体类 {register.EntityType} 注册到上下文 {contextType} 中");
            }
            Logger.LogInformation($"上下文 {contextType} 注册了{registers.Length}个实体类");

            // 应用批量实体配置
            List<IMutableEntityType> mutableEntityTypes = modelBuilder.Model.GetEntityTypes().ToList();
            IEntityBatchConfiguration[] entityBatchConfigurations =
                _serviceProvider.GetServices<IEntityBatchConfiguration>().ToArray();
            if (entityBatchConfigurations.Length > 0)
            {
                foreach (IMutableEntityType mutableEntityType in mutableEntityTypes)
                {
                    foreach (IEntityBatchConfiguration entityBatchConfiguration in entityBatchConfigurations)
                    {
                        entityBatchConfiguration.Configure(modelBuilder, mutableEntityType);
                    }
                }
            }
        }
        
        ///// <summary>
        ///// 模型配置
        ///// </summary>
        ///// <param name="optionsBuilder"></param>
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (_osharpDbOptions != null && _osharpDbOptions.LazyLoadingProxiesEnabled)
        //    {
        //        optionsBuilder.UseLazyLoadingProxies();
        //    }
        //}
    }
}