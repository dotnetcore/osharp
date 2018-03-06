// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-01-14 22:35</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;


namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Description("管理-主页")]
    public class HomeController : Controller
    {
        [Description("首页")]
        public ActionResult Index()
        {
            return Content("Admin-Home-Index");
        }
    }
}