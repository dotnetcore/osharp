// -----------------------------------------------------------------------
//  <copyright file="IDbContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 14:52</last-date>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义数据上下文接口
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 提交数据上下文的变更
        /// </summary>
        /// <returns>操作影响的记录数</returns>
        int SaveChanges();

        /// <summary>
        /// 异步方式提交数据上下文的所有变更
        /// </summary>
        /// <param name="cancelToken">任务取消标识</param>
        /// <returns>操作影响的行数</returns>
        Task<int> SaveChangesAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}