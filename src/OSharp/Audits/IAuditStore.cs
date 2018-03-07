// -----------------------------------------------------------------------
//  <copyright file="IAuditStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-31 1:26</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace OSharp.Audits
{
    /// <summary>
    /// 定义Audit数据存储功能
    /// </summary>
    public interface IAuditStore
    {
        /// <summary>
        /// 设置实体审计数据
        /// </summary>
        /// <param name="auditDatas">实体审计数据</param>
        void SetAuditDatas(IEnumerable<AuditEntity> auditDatas);

        /// <summary>
        /// 异步设置实体审计数据
        /// </summary>
        /// <param name="auditDatas">实体审计数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns></returns>
        Task SetAuditDatasAsync(IEnumerable<AuditEntity>auditDatas, CancellationToken cancelToken = default(CancellationToken));
    }


    /// <summary>
    /// 空的Audit存储，什么也不做
    /// </summary>
    public class NullAuditStore : IAuditStore
    {
        /// <summary>
        /// 设置实体审计数据
        /// </summary>
        /// <param name="auditDatas"></param>
        public void SetAuditDatas(IEnumerable<AuditEntity> auditDatas)
        { }

        /// <summary>
        /// 异步设置实体审计数据
        /// </summary>
        /// <param name="auditDatas">实体审计数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns></returns>
        public Task SetAuditDatasAsync(IEnumerable<AuditEntity> auditDatas, CancellationToken cancelToken = default(CancellationToken))
        {
            return Task.FromResult(0);
        }
    }
}