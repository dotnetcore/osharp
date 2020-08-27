// -----------------------------------------------------------------------
//  <copyright file="EntityInfoController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-15 20:54</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 5, Position = "Auth", PositionName = "权限授权模块")]
    [Description("管理-实体信息")]
    public class EntityInfoController : AdminControllerBase
    {
        [ModuleInfo]
        [Description("数据实体列表")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
