// -----------------------------------------------------------------------
//  <copyright file="AuditDatabaseStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Hosting.Systems.Entities;

using OSharp.Audits;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Mapping;
using OSharp.Net;


namespace OSharp.Hosting.Systems
{
    /// <summary>
    /// 数据库审计存储
    /// </summary>
    public class AuditDatabaseStore : IAuditStore
    {
        private readonly IRepository<AuditOperation, Guid> _operationRepository;

        /// <summary>
        /// 初始化一个<see cref="AuditDatabaseStore"/>类型的新实例
        /// </summary>
        public AuditDatabaseStore(IRepository<AuditOperation, Guid> operationRepository)
        {
            _operationRepository = operationRepository;
        }

        /// <summary>
        /// 设置保存审计数据
        /// </summary>
        /// <param name="operationEntry">操作审计数据</param>
        public void Save(AuditOperationEntry operationEntry)
        {
            AuditOperation operation = BuildOperation(operationEntry);
            _operationRepository.Insert(operation);
        }

        /// <summary>
        /// 异步保存实体审计数据
        /// </summary>
        /// <param name="operationEntry">操作审计数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns></returns>
        public async Task SaveAsync(AuditOperationEntry operationEntry, CancellationToken cancelToken = default(CancellationToken))
        {
            AuditOperation operation = BuildOperation(operationEntry);
            await _operationRepository.InsertAsync(operation);
        }

        private static AuditOperation BuildOperation(AuditOperationEntry operationEntry)
        {
            AuditOperation operation = operationEntry.MapTo<AuditOperation>();
            if (operationEntry.UserAgent != null)
            {
                UserAgent userAgent = new UserAgent(operationEntry.UserAgent);
                operation.OperationSystem = userAgent.GetSystem();
                operation.Browser = userAgent.GetBrowser();
            }
            operation.Elapsed = (int)operationEntry.EndedTime.Subtract(operationEntry.CreatedTime).TotalMilliseconds;
            if (operation.ResultType == AjaxResultType.Success)
            {
                foreach (AuditEntityEntry entityEntry in operationEntry.EntityEntries)
                {
                    AuditEntity entity = entityEntry.MapTo<AuditEntity>();
                    operation.AuditEntities.Add(entity);
                    foreach (AuditPropertyEntry propertyEntry in entityEntry.PropertyEntries)
                    {
                        AuditProperty property = propertyEntry.MapTo<AuditProperty>();
                        entity.Properties.Add(property);
                    }
                }
            }
            return operation;
        }
    }
}