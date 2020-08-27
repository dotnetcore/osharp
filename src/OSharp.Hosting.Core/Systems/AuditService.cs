// -----------------------------------------------------------------------
//  <copyright file="AuditService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 15:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using OSharp.Hosting.Systems.Entities;

using OSharp.Data;
using OSharp.Entity;


namespace OSharp.Hosting.Systems
{
    /// <summary>
    /// 服务实现：审计模块
    /// </summary>
    public class AuditService : IAuditContract
    {
        private readonly IRepository<AuditOperation, Guid> _operationRepository;
        private readonly IRepository<AuditEntity, Guid> _entityRepository;
        private readonly IRepository<AuditProperty, Guid> _propertyRepository;

        /// <summary>
        /// 初始化一个<see cref="AuditService"/>类型的新实例
        /// </summary>
        public AuditService(IRepository<AuditOperation, Guid> operationRepository,
            IRepository<AuditEntity, Guid> entityRepository,
            IRepository<AuditProperty, Guid> propertyRepository)
        {
            _operationRepository = operationRepository;
            _entityRepository = entityRepository;
            _propertyRepository = propertyRepository;
        }

        #region Implementation of IAuditContract

        /// <summary>
        /// 获取 操作审计信息查询数据集
        /// </summary>
        public IQueryable<AuditOperation> AuditOperations => _operationRepository.QueryAsNoTracking();

        /// <summary>
        /// 删除操作审计信息信息
        /// </summary>
        /// <param name="ids">要删除的操作审计信息编号</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> DeleteAuditOperations(params Guid[] ids)
        {
            return _operationRepository.DeleteAsync(ids);
        }

        /// <summary>
        /// 获取 数据审计信息查询数据集
        /// </summary>
        public IQueryable<AuditEntity> AuditEntities => _entityRepository.QueryAsNoTracking();

        /// <summary>
        /// 获取 数据审计信息查询数据集
        /// </summary>
        public IQueryable<AuditProperty> AuditProperties => _propertyRepository.QueryAsNoTracking();

        /// <summary>
        /// 删除数据审计信息信息
        /// </summary>
        /// <param name="ids">要删除的数据审计信息编号</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> DeleteAuditEntities(params Guid[] ids)
        {
            return _entityRepository.DeleteAsync(ids);
        }

        #endregion
    }
}