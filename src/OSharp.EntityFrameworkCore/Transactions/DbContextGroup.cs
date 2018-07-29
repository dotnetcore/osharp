// -----------------------------------------------------------------------
//  <copyright file="DbContextGroup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-27 4:48</last-date>
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


namespace OSharp.Entity.Transactions
{
    /// <summary>
    /// 共享同一<see cref="DbConnection"/>的数据上下文分组
    /// </summary>
    public class DbContextGroup : IDisposable
    {
        private DbTransaction _transaction;

        /// <summary>
        /// 初始化一个<see cref="DbContextGroup"/>类型的新实例
        /// </summary>
        public DbContextGroup()
        {
            DbContexts = new List<DbContextBase>();
        }

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommited { get; private set; }

        /// <summary>
        /// 获取 数据上下文集合
        /// </summary>
        public IList<DbContextBase> DbContexts { get; }

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        public void BeginOrUseTransaction(DbConnection connection)
        {
            if (_transaction == null)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                _transaction = connection.BeginTransaction();
            }
            foreach (DbContextBase context in DbContexts)
            {
                if (context.Database.CurrentTransaction != null)
                {
                    continue;
                }
                if (context.IsRelationalTransaction())
                {
                    context.Database.UseTransaction(_transaction);
                }
                else
                {
                    context.Database.BeginTransaction();
                }
            }
        }

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cancellationToken">异步取消标记</param>
        public async Task BeginOrUseTransactionAsync(DbConnection connection, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_transaction == null)
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken);
                }
                _transaction = connection.BeginTransaction();
            }
            foreach (DbContextBase context in DbContexts)
            {
                if (context.Database.CurrentTransaction != null && context.Database.CurrentTransaction.GetDbTransaction() == _transaction)
                {
                    continue;
                }
                if (context.IsRelationalTransaction())
                {
                    context.Database.UseTransaction(_transaction);
                }
                else
                {
                    await context.Database.BeginTransactionAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// 对指定数据上下文开启或使用已存在事务
        /// </summary>
        /// <param name="context">上下文</param>
        public void BeginOrUseTransaction(DbContext context)
        {
            if (!DbContexts.Contains(context))
            {
                return;
            }
            DbConnection connection = context.Database.GetDbConnection();
            BeginOrUseTransaction(connection);
        }

        /// <summary>
        /// 异步对指定数据上下文开启或使用已存在事务
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="cancellationToken">异步取消标记</param>
        public async Task BeginOrUseTransactionAsync(DbContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (context.Database.CurrentTransaction != null)
            {
                return;
            }
            if (!DbContexts.Contains(context))
            {
                return;
            }
            DbConnection connection = context.Database.GetDbConnection();
            await BeginOrUseTransactionAsync(connection, cancellationToken);
        }

        /// <summary>
        /// 提交当前连接对象的上下文的事务更改
        /// </summary>
        public void Commit()
        {
            if (HasCommited || DbContexts.Count == 0)
            {
                return;
            }
            _transaction.Commit();
            foreach (var context in DbContexts)
            {
                if (context.IsRelationalTransaction())
                {
                    //关系型数据库共享事务
                    continue;
                }
                context.Database.CommitTransaction();
            }
            HasCommited = true;
        }

        #region IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            foreach (DbContextBase context in DbContexts)
            {
                context.Dispose();
            }
        }

        #endregion
    }
}