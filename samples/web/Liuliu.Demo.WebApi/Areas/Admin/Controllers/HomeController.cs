// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Hosting.Apis.Areas.Admin.Controllers;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [Description("管理-主页")]
    public class HomeController : AdminApiControllerBase
    {
        /// <summary>
        /// 获取后台管理主菜单
        /// </summary>
        /// <returns>菜单信息</returns>
        [HttpGet]
        [Description("主菜单")]
        public ActionResult MainMenu()
        {
            return Content("MainMenu");
        }


    }
}
