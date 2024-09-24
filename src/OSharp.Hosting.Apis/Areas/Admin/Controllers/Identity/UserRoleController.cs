// -----------------------------------------------------------------------
//  <copyright file="UserRoleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:49</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 3, Position = "Identity", PositionName = "身份认证模块")]
[Description("管理-用户角色信息")]
public class UserRoleController : AdminApiControllerBase
{
    private readonly IServiceProvider _provider;

    public UserRoleController(IServiceProvider provider) : base(provider)
    {
        _provider = provider;
    }

    private IIdentityContract IdentityContract => _provider.GetRequiredService<IIdentityContract>();

    /// <summary>
    /// 读取用户角色信息
    /// </summary>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        Expression<Func<UserRole, bool>> predicate = FilterService.GetExpression<UserRole>(request.FilterGroup);
        Func<UserRole, bool> updateFunc = FilterService.GetDataFilterExpression<UserRole>(null, DataAuthOperation.Update).Compile();
        Func<UserRole, bool> deleteFunc = FilterService.GetDataFilterExpression<UserRole>(null, DataAuthOperation.Delete).Compile();

        PageResult<UserRoleOutputDto> page = IdentityContract.UserRoles.ToPage(predicate, request.PageCondition, m => new
        {
            D = m,
            UserName = m.User.UserName,
            RoleName = m.Role.Name,
        }).ToPageResult(data => data.Select(m => new UserRoleOutputDto(m.D)
        {
            UserName = m.UserName,
            RoleName = m.RoleName,
            IsLocked = m.D.IsLocked,
            Updatable = updateFunc(m.D),
            Deletable = deleteFunc(m.D)
        }).ToArray());
        return new AjaxResult(page.ToPageData());
    }

    /// <summary>
    /// 更新用户角色信息
    /// </summary>
    /// <param name="dtos">用户角色信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("更新")]
    public async Task<AjaxResult> Update(UserRoleInputDto[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));

        OperationResult result = await IdentityContract.UpdateUserRoles(dtos);
        return result.ToAjaxResult();
    }

    /// <summary>
    /// 删除用户角色信息
    /// </summary>
    /// <param name="ids">要删除的用户角色编号</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("删除")]
    public async Task<AjaxResult> Delete(long[] ids)
    {
        Check.NotNull(ids, nameof(ids));

        OperationResult result = await IdentityContract.DeleteUserRoles(ids);
        return result.ToAjaxResult();
    }

}
