// -----------------------------------------------------------------------
//  <copyright file="ModuleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-15 20:49</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "Auth", PositionName = "权限授权模块")]
    [Description("管理-模块信息")]
    public class ModuleController : AdminControllerBase
    {
        [ModuleInfo]
        [Description("模块列表")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
