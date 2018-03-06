// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 11:56</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;


namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Description("管理-主页")]
    public class HomeController : Controller
    {
        [Description("首页")]
        public IActionResult Index()
        {
            return Content("Admin-Home-Index");
        }
    }
}