// -----------------------------------------------------------------------
//  <copyright file="UserController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:49</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Authorization;
using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Common.Dtos;
using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 1, Position = "Identity", PositionName = "身份认证模块")]
[Description("管理-用户信息")]
public class UserController : AdminApiControllerBase
{
    private readonly IServiceProvider _provider;

    public UserController(IServiceProvider provider) : base(provider)
    {
        _provider = provider;
    }

    public IIdentityContract IdentityContract => _provider.GetRequiredService<IIdentityContract>();

    public UserManager<User> UserManager => _provider.GetRequiredService<UserManager<User>>();

    public FunctionAuthManager FunctionAuthManager => _provider.GetRequiredService<FunctionAuthManager>();

    /// <summary>
    /// 读取用户列表信息
    /// </summary>
    /// <returns>用户列表信息</returns>
    [HttpPost]
    [ModuleInfo]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        Check.NotNull(request, nameof(request));
        IFunction function = this.GetExecuteFunction();

        Func<User, bool> updateFunc = FilterService.GetDataFilterExpression<User>(null, DataAuthOperation.Update).Compile();
        Func<User, bool> deleteFunc = FilterService.GetDataFilterExpression<User>(null, DataAuthOperation.Delete).Compile();
        Expression<Func<User, bool>> predicate = FilterService.GetExpression<User>(request.FilterGroup);

        //查询某一角色的所有用户
        //var roleId = request.FilterGroup.Rules.FirstOrDefault(m => m.Field == "RoleId")?.CastTo(0);
        //if (roleId != 0)
        //{
        //    predicate = predicate.And(m => m.UserRoles.Any(n => n.RoleId == roleId));
        //}

        var page = UserManager.Users.ToPage(predicate, request.PageCondition, m => new
        {
            D = m,
            Roles = m.UserRoles.Select(n => n.Role.Name)
        }).ToPageResult(data => data.Select(m => new UserOutputDto(m.D)
        {
            Roles = m.Roles.ToArray(),
            Updatable = updateFunc(m.D),
            Deletable = deleteFunc(m.D)
        }).ToArray());

        return new AjaxResult(page.ToPageData());
    }

    /// <summary>
    /// 读取用户节点信息
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    [HttpPost]
    [Description("读取节点")]
    public AjaxResult ReadNode(FilterGroup group)
    {
        Check.NotNull(group, nameof(group));
        IFunction function = this.GetExecuteFunction();
        Expression<Func<User, bool>> exp = FilterService.GetExpression<User>(group);
        ListNode[] nodes = CacheService.ToCacheArray<User, ListNode>(UserManager.Users,
            exp,
            m => new ListNode()
            {
                Id = m.Id,
                Name = m.NickName
            },
            function);
        return new AjaxResult(nodes);
    }

    /// <summary>
    /// 新增用户信息
    /// </summary>
    /// <param name="dtos">用户信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("新增")]
    public async Task<AjaxResult> Create(UserInputDto[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));
        List<string> names = new List<string>();
        foreach (var dto in dtos)
        {
            User user = dto.MapTo<User>();
            IdentityResult result = dto.Password.IsMissing()
                ? await UserManager.CreateAsync(user)
                : await UserManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return result.ToOperationResult().ToAjaxResult();
            }
            names.Add(user.UserName);
        }
        return new AjaxResult($"用户“{names.ExpandAndToString()}”创建成功");
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="dtos">用户信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("更新")]
    public async Task<AjaxResult> Update(UserInputDto[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));
        List<string> names = new List<string>();
        foreach (var dto in dtos)
        {
            User user = await UserManager.FindByIdAsync(dto.Id.ToString());
            user = dto.MapTo(user);
            IdentityResult result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result.ToOperationResult().ToAjaxResult();
            }
            names.Add(user.UserName);
        }
        return new AjaxResult($"用户“{names.ExpandAndToString()}”更新成功");
    }

    /// <summary>
    /// 删除用户信息
    /// </summary>
    /// <param name="ids">用户信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("删除")]
    public async Task<AjaxResult> Delete(long[] ids)
    {
        Check.NotNull(ids, nameof(ids));
        List<string> names = new List<string>();
        foreach (var id in ids)
        {
            User user = await UserManager.FindByIdAsync(id.ToString());
            IdentityResult result = await UserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return result.ToOperationResult().ToAjaxResult();
            }
            names.Add(user.UserName);
        }
        return new AjaxResult($"用户“{names.ExpandAndToString()}”删除成功");
    }

    /// <summary>
    /// 设置用户角色
    /// </summary>
    /// <param name="dto">用户角色信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [DependOnFunction(nameof(RoleController.ReadUserRoles), Controller = nameof(RoleController))]
    [UnitOfWork]
    [Description("设置角色")]
    public async Task<AjaxResult> SetRoles(UserSetRoleDto dto)
    {
        OperationResult result = await IdentityContract.SetUserRoles(dto.UserId, dto.RoleIds);
        return result.ToAjaxResult();
    }

    /// <summary>
    /// 设置用户模块
    /// </summary>
    /// <param name="dto">用户模块信息</param>
    /// <returns>JSON操作结果</returns>
    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [DependOnFunction(nameof(ModuleController.ReadUserModules), Controller = nameof(ModuleController))]
    [UnitOfWork]
    [Description("设置模块")]
    public async Task<AjaxResult> SetModules(UserSetModuleDto dto)
    {
        OperationResult result = await FunctionAuthManager.SetUserModules(dto.UserId, dto.ModuleIds);
        return result.ToAjaxResult();
    }
}
