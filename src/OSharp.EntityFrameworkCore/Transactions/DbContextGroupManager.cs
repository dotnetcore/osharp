// -----------------------------------------------------------------------
//  <copyright file="DbContextGroupManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-27 5:23</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OSharp.Extensions;


namespace OSharp.Entity.Transactions
{
    /// <summary>
    /// 数据上下文组管理器
    /// </summary>
    public class DbContextGroupManager : IDisposable
    {
        private readonly IDictionary<string, DbContextGroup> _groups;

        /// <summary>
        /// 初始化一个<see cref="DbContextGroupManager"/>类型的新实例
        /// </summary>
        public DbContextGroupManager()
        {
            _groups = new Dictionary<string, DbContextGroup>();
        }

        /// <summary>
        /// 获取指定数据库连接字符串的上下文组信息
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>上下文组信息</returns>
        public DbContextGroup Get(string connectionString)
        {
            return _groups.GetOrDefault(connectionString);
        }

        /// <summary>
        /// 设置上下文组信息
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="group">上下文组信息</param>
        public void Set(string connectionString, DbContextGroup group)
        {
            _groups[connectionString] = group;
        }

        /// <summary>
        /// 对指定数据上下文开启或使用已存在事务
        /// </summary>
        /// <param name="context">上下文</param>
        public void BeginOrUseTransaction(DbContext context)
        {
            string connString = context.Database.GetDbConnection().ConnectionString;
            DbContextGroup group = Get(connString);
            if (group == null)
            {
                return;
            }
            group.BeginOrUseTransaction(context);
        }

        /// <summary>
        /// 异步对指定数据上下文开启或使用已存在事务
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="cancellationToken">异步取消标记</param>
        public async Task BeginOrUseTransactionAsync(DbContext context, CancellationToken cancellationToken)
        {
            string connString = context.Database.GetDbConnection().ConnectionString;
            DbContextGroup group = Get(connString);
            if (group == null)
            {
                return;
            }
            await group.BeginOrUseTransactionAsync(context, cancellationToken);
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public void Commit()
        {
            foreach (DbContextGroup group in _groups.Values)
            {
                group.Commit();
            }
        }

        #region IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            foreach (DbContextGroup group in _groups.Values)
            {
                group.Dispose();
            }
            _groups.Clear();
        }

        #endregion
    }
}