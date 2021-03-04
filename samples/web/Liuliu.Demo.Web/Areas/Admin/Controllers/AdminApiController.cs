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


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [AreaInfo("Admin", Display = "管理")]
    [RoleLimit]
    [ApiAuthorize]
    public abstract class AdminApiController : AreaApiControllerBase
    { }
}
