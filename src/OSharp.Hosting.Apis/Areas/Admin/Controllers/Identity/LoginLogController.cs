// -----------------------------------------------------------------------
//  <copyright file="LoginLogController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-22 13:54</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers;

[ModuleInfo(Order = 4, Position = "Identity", PositionName = "身份认证模块")]
[Description("管理-登录日志信息")]
public class LoginLogController : AdminApiControllerBase
{
    private readonly IServiceProvider _provider;

    public LoginLogController(IServiceProvider provider) : base(provider)
    {
        _provider = provider;
    }

    public IIdentityContract IdentityContract => _provider.GetRequiredService<IIdentityContract>();

    [HttpPost]
    [ModuleInfo]
    [Description("读取")]
    public AjaxResult Read(PageRequest request)
    {
        Check.NotNull(request, nameof(request));

        request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));
        Expression<Func<LoginLog, bool>> exp = FilterService.GetExpression<LoginLog>(request.FilterGroup);
        var page = IdentityContract.LoginLogs.ToPage(exp,
            request.PageCondition,
            m => new
            {
                D = m,
                User = new { m.User.UserName, m.User.NickName }
            }).ToPageResult(data => data.Select(m => new LoginLogOutputDto(m.D)
            {
                UserName = m.User.UserName,
                NickName = m.User.NickName,
            }).ToArray());

        return new AjaxResult(page.ToPageData());
    }

    //删除登录日志信息
    [HttpPost]
    [ModuleInfo]
    [UnitOfWork]
    [Description("删除")]
    public async Task<AjaxResult> Delete(long[] ids)
    {
        Check.NotNull(ids, nameof(ids));

        OperationResult result = await IdentityContract.DeleteLoginLogs(ids);
        return result.ToAjaxResult();
    }
}
