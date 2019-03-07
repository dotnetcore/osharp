// -----------------------------------------------------------------------
//  <copyright file="UnitOfWorkManager.cs" company="柳柳软件">
//      Copyright (c) 2016-2018 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-31 21:33</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Entity.Transactions;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// 工作单元管理器
    /// </summary>
    [Dependency(ServiceLifetime.Scoped, TryAdd = true)]
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 以数据库连接字符串为键，工作单元为值的字典
        /// </summary>
        private readonly ConcurrentDictionary<string, IUnitOfWork> _connStringUnitOfWorks
            = new ConcurrentDictionary<string, IUnitOfWork>();

        private readonly ConcurrentDictionary<Type, IUnitOfWork> _entityTypeUnitOfWorks
            = new ConcurrentDictionary<Type, IUnitOfWork>();

        /// <summary>
        /// 初始化一个<see cref="UnitOfWorkManager"/>类型的新实例
        /// </summary>
        public UnitOfWorkManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取 服务提供器
        /// </summary>
        public IServiceProvider ServiceProvider => _serviceProvider;

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommited
        {
            get { return _connStringUnitOfWorks.Values.All(m => m.HasCommited); }
        }

        /// <summary>
        /// 获取指定实体所在的工作单元对象
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns>工作单元对象</returns>
        public IUnitOfWork GetUnitOfWork<TEntity, TKey>() where TEntity : IEntity<TKey>
        {
            Type entityType = typeof(TEntity);
            return GetUnitOfWork(entityType);
        }

        /// <summary>
        /// 获取指定实体所在的工作单元对象
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>工作单元对象</returns>
        public IUnitOfWork GetUnitOfWork(Type entityType)
        {
            if (!entityType.IsEntityType())
            {
                throw new OsharpException($"类型“{entityType}”不是实体类型");
            }
            IUnitOfWork unitOfWork = _entityTypeUnitOfWorks.GetOrDefault(entityType);
            if (unitOfWork != null)
            {
                return unitOfWork;
            }

            IEntityManager entityManager = _serviceProvider.GetService<IEntityManager>();
            Type dbContextType = entityManager.GetDbContextTypeForEntity(entityType);
            if (dbContextType == null)
            {
                throw new OsharpException($"实体类“{entityType}”的所属上下文类型无法找到");
            }
            OsharpDbContextOptions dbContextOptions = GetDbContextResolveOptions(dbContextType);
            DbContextResolveOptions resolveOptions = new DbContextResolveOptions(dbContextOptions);
            unitOfWork = _connStringUnitOfWorks.GetOrDefault(resolveOptions.ConnectionString);
            if (unitOfWork != null)
            {
                return unitOfWork;
            }
            unitOfWork = ActivatorUtilities.CreateInstance<UnitOfWork>(_serviceProvider, resolveOptions);
            _entityTypeUnitOfWorks.TryAdd(entityType, unitOfWork);
            _connStringUnitOfWorks.TryAdd(resolveOptions.ConnectionString, unitOfWork);

            return unitOfWork;
        }

        /// <summary>
        /// 获取指定实体类所属的上下文类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>上下文类型</returns>
        public Type GetDbContextType(Type entityType)
        {
            IEntityManager entityManager = _serviceProvider.GetService<IEntityManager>();
            return entityManager.GetDbContextTypeForEntity(entityType);
        }

        /// <summary>
        /// 获取数据上下文选项
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns>数据上下文选项</returns>
        public OsharpDbContextOptions GetDbContextResolveOptions(Type dbContextType)
        {
            OsharpDbContextOptions dbContextOptions = _serviceProvider.GetOSharpOptions()?.GetDbContextOptions(dbContextType);
            if (dbContextOptions == null)
            {
                throw new OsharpException($"无法找到数据上下文“{dbContextType}”的配置信息");
            }
            return dbContextOptions;
        }

        /// <summary>
        /// 提交所有工作单元的事务更改
        /// </summary>
        public void Commit()
        {
            foreach (IUnitOfWork unitOfWork in _connStringUnitOfWorks.Values)
            {
                unitOfWork.Commit();
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            foreach (IUnitOfWork unitOfWork in _connStringUnitOfWorks.Values)
            {
                unitOfWork.Dispose();
            }

            _connStringUnitOfWorks.Clear();
        }
    }
}