// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-10 16:05</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-主页")]
    public class HomeController : Controller
    {
        [Description("首页")]
        public IActionResult Index()
        {
            return Content("网站首页");
        }
    }
}