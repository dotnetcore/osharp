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
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// 业务单元操作
    /// </summary>
    public class UnitOfWork : Disposable, IUnitOfWork
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        private readonly List<DbContextBase> _dbContexts = new List<DbContextBase>();
        private DbTransaction _transaction;
        private DbConnection _connection;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWork"/>类型的新实例
        /// </summary>
        public UnitOfWork(IServiceProvider provider)
        {
            _provider = provider;
            _logger = provider.GetLogger(this);
        }

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommitted { get; private set; }

        /// <summary>
        /// 获取指定数据上下文类型<typeparamref name="TEntity"/>的实例，并将同数据库连接字符串的上下文实例进行分组归类
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
        public virtual IDbContext GetEntityDbContext<TEntity, TKey>() where TEntity : IEntity<TKey>
        {
            Type entityType = typeof(TEntity);
            return GetEntityDbContext(entityType);
        }

        /// <summary>
        /// 获取指定数据实体的上下文类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体所属上下文实例</returns>
        public IDbContext GetEntityDbContext(Type entityType)
        {
            if (!entityType.IsEntityType())
            {
                throw new OsharpException($"类型 {entityType} 不是实体类型");
            }

            IEntityManager manager = _provider.GetService<IEntityManager>();
            Type dbContextType = manager.GetDbContextTypeForEntity(entityType);

            //已存在上下文对象，直接返回
            DbContextBase dbContext = _dbContexts.FirstOrDefault(m => m.GetType() == dbContextType);
            if (dbContext != null)
            {
                _logger.LogDebug($"由实体类 {entityType} 获取到已存在的上下文 {dbContext.GetType()} 实例，上下文标识：{dbContext.GetHashCode()}");
                return dbContext;
            }

            dbContext = (DbContextBase)GetDbContext(dbContextType);
            _logger.LogDebug($"由实体类 {entityType} 创建新的上下文 {dbContext.GetType()} 实例，上下文标识：{dbContext.GetHashCode()}");

            return dbContext;
        }

        /// <summary>
        /// 获取指定类型的上下文实例
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <returns></returns>
        public IDbContext GetDbContext(Type dbContextType)
        {
            DbContextBase dbContext = (DbContextBase)_provider.GetService(dbContextType);
            if (!dbContext.ExistsRelationalDatabase())
            {
                throw new OsharpException($"数据上下文 {dbContext.GetType().FullName} 的数据库不存在，请通过 Migration 功能进行数据迁移创建数据库。");
            }

            OsharpDbContextOptions dbContextOptions = _provider.GetOSharpOptions().GetDbContextOptions(dbContextType);
            ScopedDictionary scopedDictionary = _provider.GetService<ScopedDictionary>();
            _connection = dbContext.Database.GetDbConnection();
            scopedDictionary.TryAdd($"DbConnection_{dbContextOptions.ConnectionString}", _connection);

            dbContext.UnitOfWork = this;
            _dbContexts.Add(dbContext);

            return dbContext;
        }

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        public virtual void BeginOrUseTransaction()
        {
            if (_dbContexts.Count == 0)
            {
                return;
            }
            if (_transaction?.Connection == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                _transaction = _connection.BeginTransaction();
                _logger.LogDebug($"在连接 {_connection.ConnectionString} 上开启新事务，事务标识：{_transaction.GetHashCode()}");
            }

            foreach (DbContextBase context in _dbContexts)
            {
                if (context.Database.CurrentTransaction != null && context.Database.CurrentTransaction.GetDbTransaction() == _transaction)
                {
                    continue;
                }
                if (context.IsRelationalTransaction())
                {
                    context.Database.UseTransaction(_transaction);
                    _logger.LogDebug($"在上下文 {context.GetType()} 上应用现有事务，事务标识：{_transaction.GetHashCode()}");
                }
                else
                {
                    context.Database.BeginTransaction();
                }
            }

            HasCommitted = false;
        }

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="cancellationToken">异步取消标记</param>
        /// <returns></returns>
        public virtual async Task BeginOrUseTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_dbContexts.Count == 0)
            {
                return;
            }
            if (_transaction?.Connection == null)
            {
                if (_connection.State != ConnectionState.Open)
                {
                    await _connection.OpenAsync(cancellationToken);
                }

                _transaction = await _connection.BeginTransactionAsync(cancellationToken);
                _logger.LogDebug($"在连接 {_connection.ConnectionString} 上开启新事务，事务标识：{_transaction.GetHashCode()}");
            }

            foreach (DbContextBase context in _dbContexts)
            {
                if (context.Database.CurrentTransaction != null && context.Database.CurrentTransaction.GetDbTransaction() == _transaction)
                {
                    continue;
                }
                if (context.IsRelationalTransaction())
                {
                    await context.Database.UseTransactionAsync(_transaction, cancellationToken);
                    _logger.LogDebug($"在上下文 {context.GetType()} 上应用现有事务");
                }
                else
                {
                    await context.Database.BeginTransactionAsync(cancellationToken);
                }
            }

            HasCommitted = false;
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public virtual void Commit()
        {
            if (HasCommitted || _dbContexts.Count == 0 || _transaction == null)
            {
                return;
            }

            _transaction.Commit();
            _logger.LogDebug($"提交事务，事务标识：{_transaction.GetHashCode()}");
            foreach (DbContextBase context in _dbContexts)
            {
                if (context.IsRelationalTransaction())
                {
                    context.Database.CurrentTransaction.Dispose();
                    //关系型数据库共享事务
                    continue;
                }

                context.Database.CommitTransaction();
            }

            HasCommitted = true;
        }

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        public virtual void Rollback()
        {
            if (_transaction?.Connection != null)
            {
                _transaction.Rollback();
                _logger.LogDebug($"回滚事务，事务标识：{_transaction.GetHashCode()}");
            }
            foreach (var context in _dbContexts)
            {
                if (context.IsRelationalTransaction())
                {
                    CleanChanges(context);
                    if (context.Database.CurrentTransaction != null)
                    {
                        context.Database.CurrentTransaction.Dispose();
                    }
                    continue;
                }
                context.Database.RollbackTransaction();
            }
            HasCommitted = true;
        }

        private void CleanChanges(DbContext context)
        {
            var entries = context.ChangeTracker.Entries().ToArray();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
            _logger.LogDebug($"回滚事务，清理上下文 {context.GetType()} 的变更");
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                _transaction?.Dispose();
                foreach (DbContextBase context in _dbContexts)
                {
                    context.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}