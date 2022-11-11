// -----------------------------------------------------------------------
//  <copyright file="RoleFunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:49</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Authorization;
using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 3, Position = "Auth", PositionName = "权限授权模块")]
[Description("管理-角色功能")]
public class RoleFunctionController : AdminApiControllerBase
{
    private readonly IServiceProvider _provider;

    public RoleFunctionController(IServiceProvider provider) : base(provider)
    {
        _provider = provider;
    }
        
    public RoleManager<Role> RoleManager => _provider.GetRequiredService<RoleManager<Role>>();

    /// <summary>
    /// 读取角色信息
    /// </summary>
    /// <returns>角色信息</returns>
    [HttpPost]
    [ModuleInfo]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        request.FilterGroup.Rules.Add(new FilterRule("IsLocked", false, FilterOperate.Equal));
        Expression<Func<Role, bool>> predicate = FilterService.GetExpression<Role>(request.FilterGroup);
        PageResult<RoleOutputDto2> page = RoleManager.Roles.ToPage<Role, RoleOutputDto2>(predicate, request.PageCondition);
        return new AjaxResult(page.ToPageData());
    }

    /// <summary>
    /// 读取角色功能信息
    /// </summary>
    /// <returns>角色功能信息</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [Description("读取功能")]
    public AjaxResult ReadFunctions(long roleId, [FromBody] PageRequest request)
    {
        var empty = new PageData<FunctionOutputDto2>();
        if (roleId == 0)
        {
            return new AjaxResult(empty);
        }

        FunctionAuthManager functionAuthManager = _provider.GetRequiredService<FunctionAuthManager>();
        long[] moduleIds = functionAuthManager.GetRoleModuleIds(roleId);
        long[] functionIds = functionAuthManager.ModuleFunctions.Where(m => moduleIds.Contains(m.ModuleId))
            .Select(m => m.FunctionId).Distinct().ToArray();
        if (functionIds.Length == 0)
        {
            return new AjaxResult(empty);
        }

        Expression<Func<Function, bool>> funcExp = FilterService.GetExpression<Function>(request.FilterGroup);
        funcExp = funcExp.And(m => functionIds.Contains(m.Id));
        if (request.PageCondition.SortConditions.Length == 0)
        {
            request.PageCondition.SortConditions = new[] { new SortCondition("Area"), new SortCondition("Controller") };
        }

        var page = functionAuthManager.Functions.ToPage<Function, FunctionOutputDto2>(funcExp, request.PageCondition);
        return new AjaxResult(page.ToPageData());
    }
}
