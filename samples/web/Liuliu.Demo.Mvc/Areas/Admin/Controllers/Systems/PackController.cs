// -----------------------------------------------------------------------
//  <copyright file="PackController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-07-14 20:18</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 4, Position = "Systems", PositionName = "系统管理模块")]
    [Description("管理-模块包信息")]
    public class PackController : AdminControllerBase
    {
        [ModuleInfo]
        [Description("模块包列表")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
