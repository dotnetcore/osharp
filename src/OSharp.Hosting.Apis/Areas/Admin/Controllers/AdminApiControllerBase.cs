// -----------------------------------------------------------------------
//  <copyright file="AdminApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理区域API控制器基类，配置了角色访问限制和OsharpPolicy权限策略
    /// </summary>
    [AreaInfo("Admin", Display = "管理")]
    [RoleLimit]
    [ApiAuthorize]
    public abstract class AdminApiControllerBase : AreaApiControllerBase
    { }
}
