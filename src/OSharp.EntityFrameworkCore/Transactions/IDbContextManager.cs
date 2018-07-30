// -----------------------------------------------------------------------
//  <copyright file="IDbContextManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-30 14:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;


namespace OSharp.Entity.Transactions
{
    /// <summary>
    /// 定义数据上下文管理器
    /// </summary>
    public interface IDbContextManager : IDisposable
    {
        /// <summary>
        /// 获取指定类型的数据上下文
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="contextType">数据上下文类型</param>
        /// <returns>数据上下文对象</returns>
        DbContextBase Get(string connectionString, Type contextType);

        /// <summary>
        /// 添加数据上下文
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="context">数据上下文对象</param>
        void Add(string connectionString, DbContextBase context);

        /// <summary>
        /// 移除指定类型的数据上下文
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="contextType">数据上下文类型</param>
        void Remove(string connectionString, Type contextType);

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        void BeginOrUseTransaction(DbConnection connection);

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cancellationToken">异步取消标记</param>
        /// <returns></returns>
        Task BeginOrUseTransactionAsync(DbConnection connection, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 提交所有事务
        /// </summary>
        void Commit();
    }
}