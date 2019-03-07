// -----------------------------------------------------------------------
//  <copyright file="UnitOfWorkManagerExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-15 20:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// <see cref="IUnitOfWorkManager"/>扩展方法
    /// </summary>
    public static class UnitOfWorkManagerExtensions
    {
        /// <summary>
        /// 获取指定实体所在的上下文对象
        /// </summary>
        public static IDbContext GetDbContext<TEntity, TKey>(this IUnitOfWorkManager unitOfWorkManager) where TEntity : IEntity<TKey>
        {
            Type entityType = typeof(TEntity);
            return unitOfWorkManager.GetDbContext(entityType);
        }

        /// <summary>
        /// 获取指定实体类型所在的上下文对象
        /// </summary>
        public static IDbContext GetDbContext(this IUnitOfWorkManager unitOfWorkManager, Type entityType)
        {
            if (!entityType.IsEntityType())
            {
                throw new OsharpException($"类型“{entityType}”不是实体类型");
            }
            IUnitOfWork unitOfWork = unitOfWorkManager.GetUnitOfWork(entityType);
            return unitOfWork?.GetDbContext(entityType);
        }

        /// <summary>
        /// 获取指定实体类型的数据上下文选项
        /// </summary>
        public static OsharpDbContextOptions GetDbContextResolveOptions<TEntity,TKey>(this IUnitOfWorkManager unitOfWorkManager) where TEntity : IEntity<TKey>
        {
            Type entityType = typeof(TEntity);
            return unitOfWorkManager.GetDbContextResolveOptions(entityType);
        }
        
        /// <summary>
        /// 获取指定实体类型的数据上下文选项
        /// </summary>
        public static OsharpDbContextOptions GetDbContextResolveOptions(this IUnitOfWorkManager unitOfWorkManager, Type entityType)
        {
            Type dbContextType = unitOfWorkManager.GetDbContextType(entityType);
            OsharpDbContextOptions dbContextOptions = unitOfWorkManager.ServiceProvider.GetOSharpOptions()?.GetDbContextOptions(dbContextType);
            if (dbContextOptions == null)
            {
                throw new OsharpException($"无法找到数据上下文“{dbContextType}”的配置信息");
            }
            return dbContextOptions;
        }

        /// <summary>
        /// 获取指定实体类型的Sql执行器
        /// </summary>
        public static ISqlExecutor<TEntity,TKey> GetSqlExecutor<TEntity,TKey>(this IUnitOfWorkManager unitOfWorkManager) where TEntity : IEntity<TKey>
        {
            OsharpDbContextOptions options = unitOfWorkManager.GetDbContextResolveOptions(typeof(TEntity));
            DatabaseType databaseType = options.DatabaseType;
            IList<ISqlExecutor<TEntity, TKey>> executors = unitOfWorkManager.ServiceProvider.GetServices<ISqlExecutor<TEntity, TKey>>().ToList();
            return executors.FirstOrDefault(m => m.DatabaseType == databaseType);
        }
    }
}