// -----------------------------------------------------------------------
//  <copyright file="UnitOfWork2.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-18 18:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

#if NET5_0
using System.Threading;
using System.Threading.Tasks;
#endif

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
    /// 实现一个单元操作内的功能，管理单元操作内涉及的所有上下文对象及其事务
    /// </summary>
    public class UnitOfWork : Disposable, IUnitOfWork
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        private readonly ConcurrentDictionary<DbConnection, List<DbContextBase>> _contextDict;
        private readonly ConcurrentDictionary<DbConnection, DbTransaction> _transDict;

        private readonly Stack<string> _transactionStack = new Stack<string>();

        /// <summary>
        /// 初始化一个<see cref="UnitOfWork"/>类型的新实例
        /// </summary>
        public UnitOfWork(IServiceProvider provider)
        {
            _provider = provider;
            _logger = provider.GetLogger<UnitOfWork>();

            _contextDict = new ConcurrentDictionary<DbConnection, List<DbContextBase>>();
            _transDict = new ConcurrentDictionary<DbConnection, DbTransaction>();
        }

        /// <summary>
        /// 获取 是否已提交
        /// </summary>
        public virtual bool HasCommitted { get; private set; }

        /// <summary>
        /// 允许事务
        /// </summary>
        public void EnableTransaction()
        {
            string token = Guid.NewGuid().ToString();
            _transactionStack.Push(token);
            _logger.LogDebug($"允许事务提交，标识：{token}，当前总标识数：{_transactionStack.Count}");
        }

        /// <summary>
        /// 获取指定数据上下文类型的实例
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
        /// 获取指定数据实体的上下文实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体所属上下文实例</returns>
        public virtual IDbContext GetEntityDbContext(Type entityType)
        {
            if (!entityType.IsEntityType())
            {
                throw new OsharpException($"类型 {entityType} 不是实体类型");
            }

            IEntityManager manager = _provider.GetRequiredService<IEntityManager>();
            Type dbContextType = manager.GetDbContextTypeForEntity(entityType);

            DbContextBase dbContext = (DbContextBase)GetDbContext(dbContextType);
            _logger.LogDebug($"由实体类 {entityType} 获取到上下文 {dbContext.GetType()} 实例，上下文标识：{dbContext.GetHashCode()}");

            return dbContext;
        }

        /// <summary>
        /// 获取指定类型的上下文实例
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <returns></returns>
        public IDbContext GetDbContext(Type dbContextType)
        {
            //已存在上下文对象，直接返回
            DbContextBase dbContext = _contextDict.SelectMany(m => m.Value)
                .FirstOrDefault(m => m.GetType() == dbContextType);
            if (dbContext != null)
            {
                _logger.LogDebug($"获取到已存在的上下文 {dbContext.GetType()} 实例，上下文标识：{dbContext.GetHashCode()}");
                return dbContext;
            }

            dbContext = (DbContextBase)GetDbContextInternal(dbContextType);
            _logger.LogDebug($"创建新的上下文 {dbContext.GetType()} 实例，上下文标识：{dbContext.GetHashCode()}");

            return dbContext;
        }

        /// <summary>
        /// 获取指定类型的上下文实例
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <returns></returns>
        private IDbContext GetDbContextInternal(Type dbContextType)
        {
            DbContextBase dbContext = (DbContextBase)_provider.GetRequiredService(dbContextType);
            if (!dbContext.ExistsRelationalDatabase())
            {
                throw new OsharpException($"数据上下文 {dbContext.GetType().FullName} 的数据库不存在，请通过 Migration 功能进行数据迁移创建数据库。");
            }

            //将连接对象DbConnection缓存到ScopedDictionary，在再次构建DbContextOptionsBuilder的时候可以直接使用
            OsharpDbContextOptions dbContextOptions = _provider.GetOSharpOptions().GetDbContextOptions(dbContextType);
            ScopedDictionary scopedDictionary = _provider.GetRequiredService<ScopedDictionary>();
            DbConnection connection = dbContext.Database.GetDbConnection();
            scopedDictionary.TryAdd($"DbConnection_{dbContextOptions.ConnectionString}", connection);

            //缓存DbContext
            if (_contextDict.TryGetValue(connection, out List<DbContextBase> value))
            {
                value.Add(dbContext);
            }
            else
            {
                _contextDict.TryAdd(connection, new List<DbContextBase>() { dbContext });
            }

            return dbContext;
        }

        /// <summary>
        /// 对数据库连接开启事务或应用现有同连接对象的上下文事务
        /// </summary>
        /// <param name="context">数据上下文</param>
        public virtual void BeginOrUseTransaction(IDbContext context)
        {
            if (_contextDict.IsEmpty || _transactionStack.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<DbConnection, List<DbContextBase>> pair in _contextDict)
            {
                DbContextBase dbContext = pair.Value.FirstOrDefault(m => m.Equals((DbContextBase)context));
                if (dbContext == null)
                {
                    continue;
                }

                DbConnection connection = pair.Key;
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                if (!_transDict.TryGetValue(connection, out DbTransaction transaction))
                {
                    transaction = connection.BeginTransaction();
                    _transDict.TryAdd(connection, transaction);
                    _logger.LogDebug($"在上下文 {context.GetType()} 创建事务，事务标识：{transaction.GetHashCode()}");
                }

                if (dbContext.Database.CurrentTransaction != null && dbContext.Database.CurrentTransaction.GetDbTransaction() == transaction)
                {
                    continue;
                }

                if (dbContext.IsRelationalTransaction())
                {
                    dbContext.Database.UseTransaction(transaction);
                    _logger.LogDebug($"在上下文 {context.GetType()} 应用现有事务，事务标识：{transaction.GetHashCode()}");
                }
                else
                {
                    dbContext.Database.BeginTransaction();
                }

                break;
            }

            HasCommitted = false;
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public virtual void Commit()
        {
            if (HasCommitted || _contextDict.IsEmpty || _transDict.IsEmpty)
            {
                return;
            }

            if (_transactionStack.Count > 1)
            {
                string token = _transactionStack.Pop();
                _logger.LogDebug($"跳过事务提交，标识：{token}，当前剩余标识数：{_transactionStack.Count}");
                return;
            }

            if(_transactionStack.Count == 0)
            {
                throw new OsharpException("执行 IUnitOfWork.Commit() 之前，需要在事务开始时调用 IUnitOfWork.EnableTransaction()");
            }

            foreach (DbTransaction transaction in _transDict.Values)
            {
                transaction.Commit();
                _logger.LogDebug($"提交事务，事务标识：{transaction.GetHashCode()}");
            }

            HasCommitted = true;
        }

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        public virtual void Rollback()
        {
            foreach (DbConnection connection in _transDict.Keys)
            {
                DbTransaction transaction = _transDict[connection];
                if (transaction.Connection == null)
                {
                    continue;
                }

                transaction.Rollback();
                _logger.LogDebug($"回滚事务，事务标识：{transaction.GetHashCode()}");
            }

            HasCommitted = true;
        }
        
        protected override void Dispose(bool disposing)
        {
            _contextDict.SelectMany(m => m.Value).ToList().ForEach(m => m.Dispose());
            _transDict.Values.ToList().ForEach(m => m.Dispose());

            base.Dispose(disposing);
        }

#if NET5_0
        
        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="cancellationToken">异步取消标记</param>
        /// <returns></returns>
        public virtual async Task BeginOrUseTransactionAsync(IDbContext context, CancellationToken cancellationToken = default)
        {
            if (_contextDict.IsEmpty)
            {
                return;
            }

            foreach (KeyValuePair<DbConnection, List<DbContextBase>> pair in _contextDict)
            {
                DbContextBase dbContext = pair.Value.FirstOrDefault(m => m.Equals((DbContextBase)context));
                if (dbContext == null)
                {
                    continue;
                }

                DbConnection connection = pair.Key;
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken);
                }

                if (!_transDict.TryGetValue(connection, out DbTransaction transaction))
                {
                    transaction = await connection.BeginTransactionAsync(cancellationToken);
                    _transDict.TryAdd(connection, transaction);
                }

                if (dbContext.Database.CurrentTransaction != null && dbContext.Database.CurrentTransaction.GetDbTransaction() == transaction)
                {
                    continue;
                }

                if (dbContext.IsRelationalTransaction())
                {
                    await dbContext.Database.UseTransactionAsync(transaction, cancellationToken: cancellationToken);
                    _logger.LogDebug($"在上下文 {context.GetType()} 上应用现有事务，事务标识：{transaction.GetHashCode()}");
                }
                else
                {
                    await dbContext.Database.BeginTransactionAsync(cancellationToken);
                }

                break;
            }

            HasCommitted = false;
        }

        /// <summary>
        /// 异步提交当前上下文的事务更改
        /// </summary>
        /// <returns></returns>
        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (HasCommitted || _contextDict.IsEmpty || _transDict.IsEmpty)
            {
                return;
            }

            if (_transactionStack.Count > 1)
            {
                string token = _transactionStack.Pop();
                _logger.LogDebug($"跳过事务提交，标识：{token}，当前剩余标识数：{_transactionStack.Count}");
                return;
            }

            if (_transactionStack.Count == 0)
            {
                throw new OsharpException("执行 IUnitOfWork.Commit() 之前，需要在事务开始时调用 IUnitOfWork.EnableTransaction()");
            }

            _transactionStack.Pop();
            foreach (DbTransaction transaction in _transDict.Values)
            {
                await transaction.CommitAsync(cancellationToken);
                _logger.LogDebug($"提交事务，事务标识：{transaction.GetHashCode()}");
            }

            HasCommitted = true;
        }

        /// <summary>
        /// 异步回滚所有事务
        /// </summary>
        /// <returns></returns>
        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            foreach (DbConnection connection in _transDict.Keys)
            {
                DbTransaction transaction = _transDict[connection];
                if (transaction.Connection == null)
                {
                    continue;
                }

                await transaction.RollbackAsync(cancellationToken);
                _logger.LogDebug($"回滚事务，事务标识：{transaction.GetHashCode()}");
            }

            HasCommitted = true;
        }

#endif
    }
}