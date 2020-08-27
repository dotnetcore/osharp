﻿// -----------------------------------------------------------------------
//  <copyright file="UnitOfWorkManager.cs" company="柳柳软件">
//      Copyright (c) 2016-2018 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-31 21:33</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 工作单元管理器
    /// </summary>
    public class UnitOfWorkManager : Disposable, IUnitOfWorkManager
    {
        private readonly ILogger _logger;
        private readonly ScopedDictionary _scopedDictionary;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWorkManager"/>类型的新实例
        /// </summary>
        public UnitOfWorkManager(IServiceProvider provider)
        {
            ServiceProvider = provider;
            _logger = provider.GetLogger(this);
            _scopedDictionary = provider.GetService<ScopedDictionary>();
        }

        /// <summary>
        /// 获取 服务提供器
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommitted
        {
            get
            {
                return _scopedDictionary.GetConnUnitOfWorks().All(m => m.HasCommitted);
            }
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
                throw new OsharpException($"类型 {entityType} 不是实体类型");
            }

            IUnitOfWork unitOfWork = _scopedDictionary.GetEntityUnitOfWork(entityType);
            if (unitOfWork != null)
            {
                _logger.LogDebug($"由实体类 {entityType} 获取到已存在的工作单元，工作单元标识：{unitOfWork.GetHashCode()}");
                return unitOfWork;
            }

            Type dbContextType = GetDbContextType(entityType);
            if (dbContextType == null)
            {
                throw new OsharpException($"实体类 {entityType} 的所属上下文类型无法找到");
            }

            unitOfWork = GetDbContextUnitOfWork(dbContextType);
            _scopedDictionary.SetEntityUnitOfWork(entityType, unitOfWork);

            _logger.LogDebug($"由实体类 {entityType} 创建新的工作单元，工作单元标识：{unitOfWork.GetHashCode()}");
            return unitOfWork;
        }

        /// <summary>
        /// 获取指定上下文类型的上下文实例
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork GetDbContextUnitOfWork(Type dbContextType)
        {
            if (!dbContextType.IsBaseOn(typeof(DbContext)))
            {
                throw new OsharpException($"类型 {dbContextType} 不是数据上下文类型");
            }
            OsharpDbContextOptions dbContextOptions = GetDbContextResolveOptions(dbContextType);
            IUnitOfWork unitOfWork = _scopedDictionary.GetConnUnitOfWork(dbContextOptions.ConnectionString);
            if (unitOfWork != null)
            {
                _logger.LogDebug($"由上下文 {dbContextType} 的连接串获取到已存在的工作单元，工作单元标识：{unitOfWork.GetHashCode()}");
                return unitOfWork;
            }
            unitOfWork = ActivatorUtilities.CreateInstance<UnitOfWork>(ServiceProvider);
            _scopedDictionary.SetConnUnitOfWork(dbContextOptions.ConnectionString, unitOfWork);
            return unitOfWork;
        }

        /// <summary>
        /// 获取指定实体类所属的上下文类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>上下文类型</returns>
        public Type GetDbContextType(Type entityType)
        {
            IEntityManager entityManager = ServiceProvider.GetService<IEntityManager>();
            return entityManager.GetDbContextTypeForEntity(entityType);
        }

        /// <summary>
        /// 获取数据上下文选项
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns>数据上下文选项</returns>
        public OsharpDbContextOptions GetDbContextResolveOptions(Type dbContextType)
        {
            OsharpDbContextOptions dbContextOptions = ServiceProvider.GetOSharpOptions()?.GetDbContextOptions(dbContextType);
            if (dbContextOptions == null)
            {
                throw new OsharpException($"无法找到数据上下文 {dbContextType} 的配置信息");
            }
            return dbContextOptions;
        }

        /// <summary>
        /// 提交所有工作单元的事务更改
        /// </summary>
        public void Commit()
        {
            foreach (IUnitOfWork unitOfWork in _scopedDictionary.GetConnUnitOfWorks())
            {
                _logger.LogDebug($"提交工作单元事务，工作单元标识：{unitOfWork.GetHashCode()}");
                unitOfWork.Commit();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                foreach (IUnitOfWork unitOfWork in _scopedDictionary.GetConnUnitOfWorks())
                {
                    _logger.LogDebug($"释放工作单元，工作单元标识：{unitOfWork.GetHashCode()}");
                    unitOfWork.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}