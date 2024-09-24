// -----------------------------------------------------------------------
//  <copyright file="DataAuthorizationManagerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-26 23:15</last-date>
// -----------------------------------------------------------------------

using OSharp.Json;


namespace OSharp.Authorization;

/// <summary>
/// 数据权限管理器
/// </summary>
/// <typeparam name="TEntityInfo">数据实体类型</typeparam>
/// <typeparam name="TEntityInfoInputDto">数据实体输入DTO类型</typeparam>
/// <typeparam name="TEntityRole">实体角色类型</typeparam>
/// <typeparam name="TEntityRoleInputDto">实体角色输入DTO类型</typeparam>
/// <typeparam name="TRole">角色类型</typeparam>
/// <typeparam name="TRoleKey">角色编号类型</typeparam>
public abstract class DataAuthorizationManagerBase<TEntityInfo, TEntityInfoInputDto, TEntityRole, TEntityRoleInputDto, TRole, TRoleKey>
    : IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>,
      IEntityRoleStore<TEntityRole, TEntityRoleInputDto, TRoleKey>
    where TEntityInfo : IEntityInfo
    where TEntityInfoInputDto : EntityInfoInputDtoBase
    where TEntityRole : EntityRoleBase<TRoleKey>
    where TEntityRoleInputDto : EntityRoleInputDtoBase<TRoleKey>
    where TRole : RoleBase<TRoleKey>
    where TRoleKey : IEquatable<TRoleKey>
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// 初始化一个 SecurityManager 类型的新实例
    /// </summary>
    protected DataAuthorizationManagerBase(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected IRepository<TEntityInfo, long> EntityInfoRepository => _provider.GetRequiredService<IRepository<TEntityInfo, long>>();

    protected IRepository<TEntityRole, long> EntityRoleRepository => _provider.GetRequiredService<IRepository<TEntityRole, long>>();

    protected IEventBus EventBus => _provider.GetRequiredService<IEventBus>();

    protected IRepository<TRole, TRoleKey> RoleRepository => _provider.GetRequiredService<IRepository<TRole, TRoleKey>>();

    protected IFilterService FilterService => _provider.GetRequiredService<IFilterService>();

    #region Implementation of IEntityInfoStore<TEntityInfo,in TEntityInfoInputDto>

    /// <summary>
    /// 获取 实体信息查询数据集
    /// </summary>
    public IQueryable<TEntityInfo> EntityInfos => EntityInfoRepository.QueryAsNoTracking();

    /// <summary>
    /// 检查实体信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的实体信息编号</param>
    /// <returns>实体信息是否存在</returns>
    public virtual Task<bool> CheckEntityInfoExists(Expression<Func<TEntityInfo, bool>> predicate, long id = default(long))
    {
        return EntityInfoRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 更新实体信息
    /// </summary>
    /// <param name="dtos">包含更新信息的实体信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public virtual Task<OperationResult> UpdateEntityInfos(params TEntityInfoInputDto[] dtos)
    {
        Check2.Validate<TEntityInfoInputDto, long>(dtos, nameof(dtos));
        return EntityInfoRepository.UpdateAsync(dtos);
    }

    #endregion Implementation of IEntityInfoStore<TEntityInfo,in TEntityInfoInputDto>

    #region Implementation of IEntityRoleStore<TEntityRole,in TEntityRoleInputDto,in TRoleKey>

    /// <summary>
    /// 获取 实体角色信息查询数据集
    /// </summary>
    public virtual IQueryable<TEntityRole> EntityRoles => EntityRoleRepository.QueryAsNoTracking();

    /// <summary>
    /// 检查实体角色信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的实体角色信息编号</param>
    /// <returns>实体角色信息是否存在</returns>
    public virtual Task<bool> CheckEntityRoleExists(Expression<Func<TEntityRole, bool>> predicate, long id = default(long))
    {
        return EntityRoleRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 获取指定角色和实体的过滤条件组
    /// </summary>
    /// <param name="roleId">角色编号</param>
    /// <param name="entityId">实体编号</param>
    /// <param name="operation">操作</param>
    /// <returns>过滤条件组</returns>
    public virtual FilterGroup[] GetEntityRoleFilterGroups(TRoleKey roleId, long entityId, DataAuthOperation operation)
    {
        return EntityRoleRepository.QueryAsNoTracking(m => m.RoleId.Equals(roleId) && m.EntityId == entityId && m.Operation == operation)
            .Select(m => m.FilterGroupJson).ToArray().Select(m => m.FromJsonString<FilterGroup>()).ToArray();
    }

    /// <summary>
    /// 添加实体角色信息
    /// </summary>
    /// <param name="dtos">要添加的实体角色信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> CreateEntityRoles(params TEntityRoleInputDto[] dtos)
    {
        Check2.Validate<TEntityRoleInputDto, long>(dtos, nameof(dtos));

        OperationResult result = await EntityRoleRepository.InsertAsync(dtos,
            async dto =>
            {
                TRole role = await RoleRepository.GetAsync(dto.RoleId);
                if (role == null)
                {
                    throw new OsharpException($"编号为“{dto.RoleId}”的角色信息不存在");
                }

                TEntityInfo entityInfo = await EntityInfoRepository.GetAsync(dto.EntityId);
                if (entityInfo == null)
                {
                    throw new OsharpException($"编号为“{dto.EntityId}”的数据实体信息不存在");
                }

                if (await CheckEntityRoleExists(m => m.RoleId.Equals(dto.RoleId) && m.EntityId == dto.EntityId && m.Operation == dto.Operation))
                {
                    throw new OsharpException($"角色“{role.Name}”和实体“{entityInfo.Name}”和操作“{dto.Operation}”的数据权限规则已存在，不能重复添加");
                }

            });

        return result;
    }

    /// <summary>
    /// 更新实体角色信息
    /// </summary>
    /// <param name="dtos">包含更新信息的实体角色信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> UpdateEntityRoles(params TEntityRoleInputDto[] dtos)
    {
        Check2.Validate<TEntityRoleInputDto, long>(dtos, nameof(dtos));

        DataAuthCacheRefreshEventData eventData = new DataAuthCacheRefreshEventData();
        OperationResult result = await EntityRoleRepository.UpdateAsync(dtos,
            async (dto, entity) =>
            {
                TRole role = await RoleRepository.GetAsync(dto.RoleId);
                if (role == null)
                {
                    throw new OsharpException($"编号为“{dto.RoleId}”的角色信息不存在");
                }

                TEntityInfo entityInfo = await EntityInfoRepository.GetAsync(dto.EntityId);
                if (entityInfo == null)
                {
                    throw new OsharpException($"编号为“{dto.EntityId}”的数据实体信息不存在");
                }

                if (await CheckEntityRoleExists(m => m.RoleId.Equals(dto.RoleId) && m.EntityId == dto.EntityId && m.Operation == dto.Operation,
                        dto.Id))
                {
                    throw new OsharpException($"角色“{role.Name}”和实体“{entityInfo.Name}”和操作“{dto.Operation}”的数据权限规则已存在，不能重复添加");
                }

                DataAuthCacheItem cacheItem = new DataAuthCacheItem()
                {
                    RoleName = role.Name,
                    EntityTypeFullName = entityInfo.TypeName,
                    Operation = dto.Operation,
                    FilterGroup = entity.FilterGroup
                };
                if (dto.IsLocked)
                {
                    eventData.RemoveItems.Add(cacheItem);
                }
                else if(entity.FilterGroup != null)
                {
                    eventData.SetItems.Add(cacheItem);
                }
            });

        if (result.Succeeded && eventData.HasData())
        {
            await EventBus.PublishAsync(eventData);
        }

        return result;
    }

    /// <summary>
    /// 设置角色数据权限的过滤条件组
    /// </summary>
    /// <param name="id">权限记录identity.api</param>
    /// <param name="group">过滤条件组</param>
    /// <returns>业务操作结果</returns>
    public async Task<OperationResult> SetFilterGroup(long id, FilterGroup group)
    {
        TEntityRole entityRole = await EntityRoleRepository.GetAsync(id);
        if (entityRole == null)
        {
            return new OperationResult(OperationResultType.QueryNull, $"编号为“{id}”的数据权限信息不存在");
        }

        TRole role = await RoleRepository.GetAsync(entityRole.RoleId);
        if (role == null)
        {
            return new OperationResult(OperationResultType.QueryNull, $"编号为“{entityRole.RoleId}”的角色信息不存在");
        }

        TEntityInfo entityInfo = await EntityInfoRepository.GetAsync(entityRole.EntityId);
        if (entityInfo == null)
        {
            return new OperationResult(OperationResultType.QueryNull, $"编号为“{entityRole.EntityId}”的数据实体信息不存在");
        }

        OperationResult checkResult = CheckFilterGroup(group, entityInfo);
        if (!checkResult.Succeeded)
        {
            return new OperationResult(OperationResultType.Error, $"数据规则验证失败：{checkResult.Message}");
        }

        IUnitOfWork unitOfWork = _provider.GetUnitOfWork(true);
        entityRole.FilterGroupJson = group.ToJsonString();
        int count = await EntityRoleRepository.UpdateAsync(entityRole);
        await unitOfWork.CommitAsync();
        if (count > 0)
        {
            DataAuthCacheRefreshEventData eventData = new DataAuthCacheRefreshEventData();
            eventData.SetItems.Add(new DataAuthCacheItem()
            {
                RoleName = role.Name,
                EntityTypeFullName = entityInfo.TypeName,
                Operation = entityRole.Operation,
                FilterGroup = entityRole.FilterGroup
            });
            if (eventData.HasData())
            {
                await EventBus.PublishAsync(eventData);
            }

            return new OperationResult(OperationResultType.Success, $"{role.Name} - {entityInfo.Name} - {entityRole.Operation} 的过滤条件设置成功");
        }

        return OperationResult.NoChanged;
    }

    /// <summary>
    /// 删除实体角色信息
    /// </summary>
    /// <param name="ids">要删除的实体角色信息编号</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> DeleteEntityRoles(params long[] ids)
    {
        DataAuthCacheRefreshEventData eventData = new DataAuthCacheRefreshEventData();
        OperationResult result = await EntityRoleRepository.DeleteAsync(ids,
            async entity =>
            {
                TRole role = await RoleRepository.GetAsync(entity.RoleId);
                TEntityInfo entityInfo = await EntityInfoRepository.GetAsync(entity.EntityId);
                if (role != null && entityInfo != null)
                {
                    eventData.RemoveItems.Add(new DataAuthCacheItem()
                    { RoleName = role.Name, EntityTypeFullName = entityInfo.TypeName, Operation = entity.Operation });
                }
            });
        if (result.Succeeded && eventData.HasData())
        {
            //移除数据权限缓存
            await EventBus.PublishAsync(eventData);
        }

        return result;
    }

    private OperationResult CheckFilterGroup(FilterGroup group, TEntityInfo entityInfo)
    {
        EntityProperty[] properties = entityInfo.Properties;

        foreach (FilterRule rule in group.Rules)
        {
            EntityProperty property = properties.FirstOrDefault(m => m.Name == rule.Field);
            if (property == null)
            {
                return new OperationResult(OperationResultType.Error, $"属性名“{rule.Field}”在实体“{entityInfo.Name}”中不存在");
            }

            if (rule.Value == null || rule.Value.ToString().IsNullOrWhiteSpace())
            {
                return new OperationResult(OperationResultType.Error, $"属性名“{property.Display}”操作“{rule.Operate.ToDescription()}”的值不能为空");
            }
        }

        if (group.Operate == FilterOperate.And)
        {
            List<IGrouping<string, FilterRule>> duplicate = group.Rules.GroupBy(m => m.Field + m.Operate).Where(m => m.Count() > 1).ToList();
            if (duplicate.Count > 0)
            {
                FilterRule[] rules = duplicate.SelectMany(m => m.Select(n => n)).DistinctBy(m => m.Field + m.Operate).ToArray();
                return new OperationResult(OperationResultType.Error,
                    $"组操作为“并且”的条件下，字段和操作“{rules.ExpandAndToString(m => $"{properties.First(n => n.Name == m.Field).Display}-{m.Operate.ToDescription()}", ", ")}”存在重复规则，请移除重复项");
            }
        }

        OperationResult result;
        if (group.Groups.Count > 0)
        {
            foreach (FilterGroup g in group.Groups)
            {
                result = CheckFilterGroup(g, entityInfo);
                if (!result.Succeeded)
                {
                    return result;
                }
            }
        }

        Type entityType = Type.GetType(entityInfo.TypeName);
        result = FilterService.CheckFilterGroup(group, entityType);
        if (!result.Succeeded)
        {
            return result;
        }

        return OperationResult.Success;
    }

    #endregion
}
