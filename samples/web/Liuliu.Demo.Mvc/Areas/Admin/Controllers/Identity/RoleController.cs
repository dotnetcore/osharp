// -----------------------------------------------------------------------
//  <copyright file="RoleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-14 21:36</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 2, Position = "Identity", PositionName = "身份认证模块")]
    [Description("管理-角色信息")]
    public class RoleController : AdminControllerBase
    {
        [Description("角色列表")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
