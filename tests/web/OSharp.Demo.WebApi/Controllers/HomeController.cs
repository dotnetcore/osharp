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

using OSharp.Collections;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Demo.Security.Dtos;
using OSharp.Demo.Security.Entities;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-主页")]
    public class HomeController : Controller
    {
        private readonly OSharpModuleManager _moduleManager;

        public HomeController(OSharpModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        [Description("首页")]
        public IActionResult Index()
        {
            List<string> lines = new List<string>();
            lines.Add("WebApi网络服务已启动");
            ViewBag.Lines = lines;

            ViewBag.Modules = _moduleManager.SourceModules.OrderBy(m => m.Level).ThenByDescending(m => m.IsAutoLoad);

            return View();
        }
    }
}