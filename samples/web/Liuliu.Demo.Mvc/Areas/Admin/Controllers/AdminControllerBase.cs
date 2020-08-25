// -----------------------------------------------------------------------
//  <copyright file="AdminControllerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-13 1:12</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [AreaInfo("Admin", Display = "管理")]
    [RoleLimit]
    [Authorize(Policy = FunctionRequirement.OsharpPolicy)]
    public abstract class AdminControllerBase : AreaBaseController
    { }
}
