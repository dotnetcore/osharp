// -----------------------------------------------------------------------
//  <copyright file="PackController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-13 15:00</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Systems.Dtos;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 4, Position = "Systems", PositionName = "系统管理模块")]
[Description("管理-模块包信息")]
public class PackController : AdminApiControllerBase
{
    /// <summary>
    /// 初始化一个<see cref="PackController"/>类型的新实例
    /// </summary>
    public PackController(IServiceProvider provider)
        : base(provider)
    { }

    /// <summary>
    /// 读取模块包列表信息
    /// </summary>
    /// <param name="request">页请求</param>
    /// <returns></returns>
    [HttpPost]
    [ModuleInfo]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        request.AddDefaultSortCondition(
            new SortCondition("Level"),
            new SortCondition("Order")
        );
        IFunction function = this.GetExecuteFunction();
        Expression<Func<PackOutputDto, bool>> exp = FilterService.GetExpression<PackOutputDto>(request.FilterGroup);
        IServiceProvider provider = HttpContext.RequestServices;
        IQueryable<PackOutputDto> query = provider.GetAllPacks().Select(m => new PackOutputDto()
        {
            Name = m.GetType().Name,
            Display = m.GetType().GetDescription(true),
            Class = m.GetType().FullName,
            Level = m.Level,
            Order = m.Order,
            IsEnabled = m.IsEnabled
        }).AsQueryable();

        var page = CacheService.ToPageCache(query,
            exp,
            request.PageCondition,
            m => m,
            function);
        return new AjaxResult("数据读取成功", AjaxResultType.Success, page.ToPageData());
    }
}
