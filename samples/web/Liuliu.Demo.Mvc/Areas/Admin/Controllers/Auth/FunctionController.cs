// -----------------------------------------------------------------------
//  <copyright file="FunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-15 20:51</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 2, Position = "Auth", PositionName = "权限授权模块")]
    [Description("管理-功能信息")]
    public class FunctionController : AdminControllerBase
    {
        [ModuleInfo]
        [Description("功能列表")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
