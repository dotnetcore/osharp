// -----------------------------------------------------------------------
//  <copyright file="UnitOfWork.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 22:20</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Entity.Transactions;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// 业务单元操作
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbContextManager _dbContextMamager;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWork"/>类型的新实例
        /// </summary>
        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContextMamager = serviceProvider.GetService<IDbContextManager>();
        }

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommited => _dbContextMamager.HasCommited;

        /// <summary>
        /// 获取指定数据上下文类型<typeparamref name="TEntity"/>的实例，并将同数据库连接字符串的上下文实例进行分组归类
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
        public IDbContext GetDbContext<TEntity, TKey>() where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
        {
            IEntityConfigurationTypeFinder typeFinder = _serviceProvider.GetService<IEntityConfigurationTypeFinder>();
            Type entityType = typeof(TEntity);
            Type dbContextType = typeFinder.GetDbContextTypeForEntity(entityType);
            OSharpDbContextOptions dbContextOptions = GetDbContextResolveOptions(dbContextType);
            DbContextResolveOptions resolveOptions = new DbContextResolveOptions(dbContextOptions);

            //已存在上下文对象，直接返回
            DbContextBase dbContext = _dbContextMamager.Get(dbContextType, resolveOptions.ConnectionString);
            if (dbContext != null)
            {
                return dbContext;
            }
            IDbContextResolver contextResolver = _serviceProvider.GetService<IDbContextResolver>();
            dbContext = (DbContextBase)contextResolver.Resolve(resolveOptions);
            if (!dbContext.ExistsRelationalDatabase())
            {
                throw new OsharpException($"数据上下文“{dbContext.GetType().FullName}”的数据库不存在，请通过 Migration 功能进行数据迁移创建数据库。");
            }
            if (resolveOptions.ExistingConnection == null)
            {
                resolveOptions.ExistingConnection = dbContext.Database.GetDbConnection();
            }
            _dbContextMamager.Add(dbContextOptions.ConnectionString, dbContext);

            return dbContext;
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public void Commit()
        {
            _dbContextMamager.Commit();
        }

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        public void Rollback()
        {
            _dbContextMamager.Rollback();
        }

        private OSharpDbContextOptions GetDbContextResolveOptions(Type dbContextType)
        {
            OSharpDbContextOptions dbContextOptions = _serviceProvider.GetOSharpOptions()?.GetDbContextOptions(dbContextType);
            if (dbContextOptions == null)
            {
                throw new OsharpException($"无法找到数据上下文“{dbContextType}”的配置信息");
            }
            return dbContextOptions;
        }

        /// <summary>释放对象.</summary>
        public void Dispose()
        {
            _dbContextMamager.Dispose();
        }
    }
}