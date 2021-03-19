// -----------------------------------------------------------------------
//  <copyright file="IUnitOfWork2.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-18 18:51</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义一个单元操作内的功能，管理单元操作内涉及的所有上下文对象及其事务
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获取 是否已提交
        /// </summary>
        bool HasCommitted { get; }

        /// <summary>
        /// 启用事务，事务代码写在 UnitOfWork.EnableTransaction() 与 UnitOfWork.Commit() 之间
        /// </summary>
        void EnableTransaction();

        /// <summary>
        /// 获取指定数据上下文类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
        IDbContext GetEntityDbContext<TEntity, TKey>() where TEntity : IEntity<TKey>;

        /// <summary>
        /// 获取指定数据实体的上下文实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体所属上下文实例</returns>
        IDbContext GetEntityDbContext(Type entityType);

        /// <summary>
        /// 获取指定类型的上下文实例
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <returns></returns>
        IDbContext GetDbContext(Type dbContextType);

        /// <summary>
        /// 对数据库连接开启事务或应用现有同连接对象的上下文事务
        /// </summary>
        /// <param name="context">数据上下文</param>
        void BeginOrUseTransaction(IDbContext context);

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        void Rollback();

#if NET5_0

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="cancellationToken">异步取消标记</param>
        /// <returns></returns>
        Task BeginOrUseTransactionAsync(IDbContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步提交当前上下文的事务更改
        /// </summary>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步回滚所有事务
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync(CancellationToken cancellationToken = default);
#endif
    }
}