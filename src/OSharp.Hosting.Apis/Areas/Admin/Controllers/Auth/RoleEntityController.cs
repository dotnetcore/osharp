// -----------------------------------------------------------------------
//  <copyright file="RoleEntityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-05 14:45</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Authorization;
using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Authorization.Entities;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 6, Position = "Auth", PositionName = "权限授权模块")]
[Description("管理-角色数据权限")]
public class RoleEntityController : AdminApiControllerBase
{
    private readonly DataAuthManager _dataAuthManager;

    public RoleEntityController(IServiceProvider provider) : base(provider)
    {
        _dataAuthManager = provider.GetRequiredService<DataAuthManager>();
    }

    /// <summary>
    /// 读取角色数据权限列表信息
    /// </summary>
    /// <param name="request">页请求信息</param>
    /// <returns>角色数据权限列表分页信息</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(EntityInfoController.ReadProperties), Controller = nameof(EntityInfoController))]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        Expression<Func<EntityRole, bool>> predicate = FilterService.GetExpression<EntityRole>(request.FilterGroup);
        request.AddDefaultSortCondition(new[]
        {
            new SortCondition("RoleId"),
            new SortCondition("EntityId"),
            new SortCondition("Operation")
        });
        RoleManager<Role> roleManager = HttpContext.RequestServices.GetService<RoleManager<Role>>();
        Func<EntityRole, bool> updateFunc = FilterService.GetDataFilterExpression<EntityRole>(null, DataAuthOperation.Update).Compile();
        Func<EntityRole, bool> deleteFunc = FilterService.GetDataFilterExpression<EntityRole>(null, DataAuthOperation.Delete).Compile();
        var page = _dataAuthManager.EntityRoles.ToPage(predicate,
            request.PageCondition,
            m => new
            {
                D = m,
                RoleName = roleManager.Roles.First(n => n.Id == m.RoleId).Name,
                EntityName = m.EntityInfo.Name,
                EntityType = m.EntityInfo.TypeName,
            }).ToPageResult(data => data.Select(m => new EntityRoleOutputDto(m.D)
        {
            RoleName = m.RoleName,
            EntityName = m.EntityName,
            EntityType = m.EntityType,
            Updatable = updateFunc(m.D),
            Deletable = deleteFunc(m.D)
        }).ToArray());
        return new AjaxResult(page.ToPageData());
    }

    /// <summary>
    /// 新增角色数据权限信息
    /// </summary>
    /// <param name="dtos">角色数据权限信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [DependOnFunction(nameof(RoleController.ReadNode), Controller = nameof(RoleController))]
    [DependOnFunction(nameof(EntityInfoController.ReadNode), Controller = nameof(EntityInfoController))]
    [UnitOfWork]
    [Description("新增")]
    public async Task<AjaxResult> Create(params EntityRoleInputDto[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));

        OperationResult result = await _dataAuthManager.CreateEntityRoles(dtos);
        return result.ToAjaxResult();
    }

    /// <summary>
    /// 更新角色数据权限信息
    /// </summary>
    /// <param name="dtos">角色数据权限信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [DependOnFunction(nameof(RoleController.ReadNode), Controller = nameof(RoleController))]
    [DependOnFunction(nameof(EntityInfoController.ReadNode), Controller = nameof(EntityInfoController))]
    [UnitOfWork]
    [Description("更新")]
    public async Task<AjaxResult> Update(params EntityRoleInputDto[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));
        OperationResult result = await _dataAuthManager.UpdateEntityRoles(dtos);
        return result.ToAjaxResult();
    }

    /// <summary>
    /// 设置角色数据权限的过滤条件组
    /// </summary>
    /// <param name="id">权限记录identity.api</param>
    /// <param name="group">过滤条件组</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("设置过滤条件")]
    public async Task<AjaxResult> SetFilterGroup(long id, [FromBody]FilterGroup group)
    {
        OperationResult result = await _dataAuthManager.SetFilterGroup(id, group);
        return result.ToAjaxResult();
    }

    /// <summary>
    /// 删除角色数据权限信息
    /// </summary>
    /// <param name="ids">角色数据权限信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("删除")]
    public async Task<AjaxResult> Delete(params long[] ids)
    {
        Check.NotNull(ids, nameof(ids));

        OperationResult result = await _dataAuthManager.DeleteEntityRoles(ids);
        return result.ToAjaxResult();
    }
}
