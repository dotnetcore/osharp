// -----------------------------------------------------------------------
//  <copyright file="UnitOfWork.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 22:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.Core.Options;
using OSharp.Entity.Transactions;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// 业务单元操作
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWork"/>类型的新实例
        /// </summary>
        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            HasCommited = false;
            ActiveTransactionInfos = new Dictionary<string, ActiveTransactionInfo>();
        }

        /// <summary>
        /// 获取 活动的事务信息字典，以连接字符串为健，活动事务信息为值
        /// </summary>
        protected IDictionary<string, ActiveTransactionInfo> ActiveTransactionInfos { get; }

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommited { get; private set; }

        /// <summary>
        /// 获取指定数据上下文类型<typeparamref name="TEntity"/>的实例
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
        public IDbContext GetDbContext<TEntity, TKey>() where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
        {
            IEntityConfigurationTypeFinder typeFinder = _serviceProvider.GetService<IEntityConfigurationTypeFinder>();
            Type entityType = typeof(TEntity);
            Type dbContextType = typeFinder.GetDbContextTypeForEntity(entityType);

            DbContext dbContext;
            OSharpDbContextOptions dbContextOptions = GetDbContextResolveOptions(dbContextType);
            DbContextResolveOptions resolveOptions = new DbContextResolveOptions(dbContextOptions);
            IDbContextResolver contextResolver = _serviceProvider.GetService<IDbContextResolver>();
            ActiveTransactionInfo transInfo = ActiveTransactionInfos.GetOrDefault(resolveOptions.ConnectionString);
            //连接字符串的事务不存在，添加起始上下文事务信息
            if (transInfo == null)
            {
                resolveOptions.ExistingConnection = null;
                dbContext = contextResolver.Resolve(resolveOptions);
                if (!dbContext.ExistsRelationalDatabase())
                {
                    throw new OsharpException($"数据上下文“{dbContext.GetType().FullName}”的数据库不存在，请通过 Migration 功能进行数据迁移创建数据库。");
                }

                IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
                transInfo = new ActiveTransactionInfo(transaction, dbContext);
                ActiveTransactionInfos[resolveOptions.ConnectionString] = transInfo;
            }
            else
            {
                resolveOptions.ExistingConnection = transInfo.DbContextTransaction.GetDbTransaction().Connection;
                //相同连接串相同上下文类型并且已存在对象，直接返回上下文对象
                if (transInfo.StarterDbContext.GetType() == resolveOptions.DbContextType)
                {
                    return transInfo.StarterDbContext as IDbContext;
                }
                dbContext = contextResolver.Resolve(resolveOptions);
                if (dbContext.IsRelationalTransaction())
                {
                    dbContext.Database.UseTransaction(transInfo.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction();
                }
                transInfo.AttendedDbContexts.Add(dbContext);
            }
            return dbContext as IDbContext;
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public void Commit()
        {
            if (HasCommited)
            {
                return;
            }
            foreach (ActiveTransactionInfo transInfo in ActiveTransactionInfos.Values)
            {
                transInfo.DbContextTransaction.Commit();

                foreach (DbContext attendedDbContext in transInfo.AttendedDbContexts)
                {
                    if (attendedDbContext.IsRelationalTransaction())
                    {
                        //关系型数据库共享事务
                        continue;
                    }
                    attendedDbContext.Database.CommitTransaction();
                }
            }
            HasCommited = true;
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
            foreach (ActiveTransactionInfo transInfo in ActiveTransactionInfos.Values)
            {
                transInfo.DbContextTransaction.Dispose();
                foreach (DbContext attendedDbContext in transInfo.AttendedDbContexts)
                {
                    attendedDbContext.Dispose();
                }
                transInfo.StarterDbContext.Dispose();
            }
            ActiveTransactionInfos.Clear();
        }
    }
}