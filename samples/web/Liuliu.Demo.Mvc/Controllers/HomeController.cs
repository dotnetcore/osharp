// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-02 23:32</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;

using Liuliu.Demo.Web.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OSharp.Authorization.Modules;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-主页")]
    [ModuleInfo(Order = 1)]
    public class HomeController : SiteControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ModuleInfo]
        [Description("网站首页")]
        public IActionResult Index()
        {
            return View();
        }

        [Description("错误页")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
