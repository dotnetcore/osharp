// -----------------------------------------------------------------------
//  <copyright file="MenuInfoController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-01 10:22</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Systems;
using OSharp.Hosting.Systems.Dtos;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 5, Position = "Systems", PositionName = "系统管理模块")]
[Description("管理-菜单信息")]
public class MenuController : AdminApiControllerBase
{
    private readonly IServiceProvider _provider;

    public MenuController(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    private ISystemsContract SystemsContract => _provider.GetRequiredService<ISystemsContract>();

    [HttpPost]
    [ModuleInfo]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        Check.NotNull(request, nameof(request));

        Expression<Func<Menu, bool>> predicate = FilterService.GetExpression<Menu>(request.FilterGroup);
        var page = SystemsContract.MenuInfos.ToPage<Menu, MenuOutputDto>(predicate, request.PageCondition);

        return new AjaxResult("数据读取成功", AjaxResultType.Success, page.ToPageData());
    }

    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read), Controller = nameof(MenuController))]
    [UnitOfWork]
    [Description("新增")]
    public async Task<AjaxResult> Create(MenuInputDto[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));

        OperationResult result = await SystemsContract.CreateMenuInfos(dtos);
        return result.ToAjaxResult();
    }

    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("更新")]
    public async Task<AjaxResult> Update(MenuInputDto[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));

        OperationResult result = await SystemsContract.UpdateMenuInfos(dtos);
        return result.ToAjaxResult();
    }

    [HttpPost]
    [ModuleInfo]
    [DependOnFunction(nameof(Read))]
    [UnitOfWork]
    [Description("删除")]
    public async Task<AjaxResult> Delete(long[] ids)
    {
        Check.NotNull(ids, nameof(ids));

        OperationResult result = await SystemsContract.DeleteMenuInfos(ids);
        return result.ToAjaxResult();
    }
}
