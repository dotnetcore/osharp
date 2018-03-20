// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-10 16:05</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Demo.Security.Dtos;
using OSharp.Demo.Security.Entities;
using OSharp.Entity;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-主页")]
    public class HomeController : Controller
    {
        private readonly IServiceProvider _provider;
        private readonly OSharpModuleManager _moduleManager;

        public HomeController(IServiceProvider provider)
        {
            _provider = provider;
            _moduleManager = provider.GetService<OSharpModuleManager>();
        }

        [Description("首页")]
        public IActionResult Index()
        {
            List<string> lines = new List<string>();
            lines.Add("WebApi网络服务已启动");
            lines.Add($"数据连接：{_provider.GetOSharpOptions().GetDbContextOptions(typeof(DefaultDbContext)).ConnectionString}");
            ViewBag.Lines = lines;

            ViewBag.Modules = _moduleManager.SourceModules.OrderBy(m => m.Level).ThenBy(m => m.Order);

            return View();
        }
    }
}