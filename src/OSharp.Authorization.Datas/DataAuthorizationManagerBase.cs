// -----------------------------------------------------------------------
//  <copyright file="DataAuthorizationManagerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-26 23:15</last-date>
// -----------------------------------------------------------------------


namespace OSharp.Authorization.DataAuthorization;

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

    protected IRepository<TEntityInfo, Guid> EntityInfoRepository => _provider.GetRequiredService<IRepository<TEntityInfo, Guid>>();

    protected IRepository<TEntityRole, Guid> EntityRoleRepository => _provider.GetRequiredService<IRepository<TEntityRole, Guid>>();

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
    public virtual Task<bool> CheckEntityInfoExists(Expression<Func<TEntityInfo, bool>> predicate, Guid id = default(Guid))
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
        Check2.Validate<TEntityInfoInputDto, Guid>(dtos, nameof(dtos));
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
    public virtual Task<bool> CheckEntityRoleExists(Expression<Func<TEntityRole, bool>> predicate, Guid id = default(Guid))
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
    public virtual FilterGroup[] GetEntityRoleFilterGroups(TRoleKey roleId, Guid entityId, DataAuthOperation operation)
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
        Check2.Validate<TEntityRoleInputDto, Guid>(dtos, nameof(dtos));

        DataAuthCacheRefreshEventData eventData = new DataAuthCacheRefreshEventData();
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

                OperationResult checkResult = CheckFilterGroup(dto.FilterGroup, entityInfo);
                if (!checkResult.Succeeded)
                {
                    throw new OsharpException($"数据规则验证失败：{checkResult.Message}");
                }

                if (!dto.IsLocked)
                {
                    eventData.SetItems.Add(new DataAuthCacheItem()
                    {
                        RoleName = role.Name,
                        EntityTypeFullName = entityInfo.TypeName,
                        Operation = dto.Operation,
                        FilterGroup = dto.FilterGroup
                    });
                }
            });
        if (result.Succeeded && eventData.HasData())
        {
            await EventBus.PublishAsync(eventData);
        }

        return result;
    }

    /// <summary>
    /// 更新实体角色信息
    /// </summary>
    /// <param name="dtos">包含更新信息的实体角色信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> UpdateEntityRoles(params TEntityRoleInputDto[] dtos)
    {
        Check2.Validate<TEntityRoleInputDto, Guid>(dtos, nameof(dtos));

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

                OperationResult checkResult = CheckFilterGroup(dto.FilterGroup, entityInfo);
                if (!checkResult.Succeeded)
                {
                    throw new OsharpException($"数据规则验证失败：{checkResult.Message}");
                }

                DataAuthCacheItem cacheItem = new DataAuthCacheItem()
                {
                    RoleName = role.Name,
                    EntityTypeFullName = entityInfo.TypeName,
                    Operation = dto.Operation,
                    FilterGroup = dto.FilterGroup
                };
                if (dto.IsLocked)
                {
                    eventData.RemoveItems.Add(cacheItem);
                }
                else
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
    /// 删除实体角色信息
    /// </summary>
    /// <param name="ids">要删除的实体角色信息编号</param>
    /// <returns>业务操作结果</returns>
    public virtual async Task<OperationResult> DeleteEntityRoles(params Guid[] ids)
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
