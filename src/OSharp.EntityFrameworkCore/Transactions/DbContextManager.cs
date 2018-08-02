// -----------------------------------------------------------------------
//  <copyright file="DbContextManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-30 14:57</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Collections;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Entity.Transactions
{
    /// <summary>
    /// 数据上下文管理器
    /// </summary>
    public class DbContextManager : IDbContextManager
    {
        private readonly ConcurrentDictionary<string, DbContextGroup> _groups
            = new ConcurrentDictionary<string, DbContextGroup>();

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommited
        {
            get { return _groups.Values.All(m => m.HasCommited); }
        }

        /// <summary>
        /// 获取指定类型的数据上下文
        /// </summary>
        /// <param name="contextType">数据上下文类型</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>数据上下文对象</returns>
        public DbContextBase Get(Type contextType, string connectionString = null)
        {
            if (connectionString == null)
            {
                return _groups.Values.SelectMany(m => m.DbContexts).FirstOrDefault(m => m.GetType() == contextType);
            }
            DbContextGroup group = _groups.GetOrDefault(connectionString);
            return @group?.DbContexts.FirstOrDefault(m => m.GetType() == contextType);
        }

        /// <summary>
        /// 添加数据上下文
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="context">数据上下文对象</param>
        public void Add(string connectionString, DbContextBase context)
        {
            DbContextGroup group = _groups.GetOrAdd(connectionString, () => new DbContextGroup());
            group.DbContexts.AddIfNotExist(context);
            context.ContextGroup = group;
            _groups[connectionString] = group;
        }

        /// <summary>
        /// 移除指定类型的数据上下文
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="contextType">数据上下文类型</param>
        public void Remove(string connectionString, Type contextType)
        {
            DbContextGroup group = _groups.GetOrDefault(connectionString);
            DbContextBase context = group?.DbContexts.FirstOrDefault(m => m.GetType() == contextType);
            if (context == null)
            {
                return;
            }
            group.DbContexts.Remove(context);
            context.ContextGroup = null;
            if (group.DbContexts.Count == 0)
            {
                _groups.TryRemove(connectionString, out group);
                return;
            }
            _groups[connectionString] = group;
        }

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        public void BeginOrUseTransaction(DbConnection connection)
        {
            DbContextGroup group = _groups.GetOrDefault(connection.ConnectionString);
            if (group == null)
            {
                throw new OsharpException("开启事务时，指定连接对象的上下文组无法找到");
            }
            group.BeginOrUseTransaction(connection);
        }

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cancellationToken">异步取消标记</param>
        /// <returns></returns>
        public Task BeginOrUseTransactionAsync(DbConnection connection, CancellationToken cancellationToken = default(CancellationToken))
        {
            DbContextGroup group = _groups.GetOrDefault(connection.ConnectionString);
            if (group == null)
            {
                throw new OsharpException("开启事务时，指定连接对象的上下文组无法找到");
            }
            return group.BeginOrUseTransactionAsync(connection, cancellationToken);
        }

        /// <summary>
        /// 提交所有事务
        /// </summary>
        public void Commit()
        {
            foreach (DbContextGroup group in _groups.Values)
            {
                group.Commit();
            }
        }

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        public void Rollback()
        {
            foreach (DbContextGroup group in _groups.Values)
            {
                group.Rollback();
            }
        }

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            foreach (var group in _groups.Values)
            {
                group.Dispose();
            }
            _groups.Clear();
        }

        #endregion

    }
}