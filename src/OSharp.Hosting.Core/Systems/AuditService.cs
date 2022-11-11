// -----------------------------------------------------------------------
//  <copyright file="AuditService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 15:25</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Systems;

/// <summary>
/// 服务实现：审计模块
/// </summary>
public class AuditService : IAuditContract
{
    private readonly IRepository<AuditOperation, long> _operationRepository;
    private readonly IRepository<AuditEntity, long> _entityRepository;
    private readonly IRepository<AuditProperty, long> _propertyRepository;

    /// <summary>
    /// 初始化一个<see cref="AuditService"/>类型的新实例
    /// </summary>
    public AuditService(IRepository<AuditOperation, long> operationRepository,
        IRepository<AuditEntity, long> entityRepository,
        IRepository<AuditProperty, long> propertyRepository)
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
    public Task<OperationResult> DeleteAuditOperations(params long[] ids)
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
    public Task<OperationResult> DeleteAuditEntities(params long[] ids)
    {
        return _entityRepository.DeleteAsync(ids);
    }

    #endregion
}
