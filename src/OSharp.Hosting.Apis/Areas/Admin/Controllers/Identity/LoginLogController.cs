// -----------------------------------------------------------------------
//  <copyright file="LoginLogController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-22 13:54</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 4, Position = "Identity", PositionName = "身份认证模块")]
    [Description("管理-登录日志信息")]
    public class LoginLogController : AdminApiControllerBase
    {
        private readonly IFilterService _filterService;
        private readonly IIdentityContract _identityContract;

        public LoginLogController(IFilterService filterService, IIdentityContract identityContract)
        {
            _filterService = filterService;
            _identityContract = identityContract;
        }

        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public AjaxResult Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            Expression<Func<LoginLog, bool>> exp = _filterService.GetExpression<LoginLog>(request.FilterGroup);
            var page = _identityContract.LoginLogs.ToPage(exp,
                request.PageCondition,
                m => new
                {
                    D = m,
                    User = new { m.User.UserName, m.User.NickName }
                }).ToPageResult(data => data.Select(m => new LoginLogOutputDto(m.D)
            {
                UserName = m.User.UserName,
                NickName = m.User.NickName
            }).ToArray());

            return new AjaxResult("ok", AjaxResultType.Success, page.ToPageData());
        }
    }   
}