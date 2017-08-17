// -----------------------------------------------------------------------
//  <copyright file="IUnitOfWork.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-16 22:29</last-date>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;


namespace OSharp.Entity
{
    /// <summary>
    /// 业务单元操作接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 获取 是否开启事务提交
        /// </summary>
        bool TransactionEnabled { get; }

        /// <summary>
        /// 显式开启数据上下文事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        void Commit();

        /// <summary>
        /// 显式回滚事务，仅在显式开启事务后有用
        /// </summary>
        void Rollback();

        /// <summary>
        /// 提交当前单元操作的更改。
        /// </summary>
        /// <returns>操作影响的行数</returns>
        int SaveChanges();

        /// <summary>
        /// 异步提交当前单元操作的更改。
        /// </summary>
        /// <returns>操作影响的行数</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}