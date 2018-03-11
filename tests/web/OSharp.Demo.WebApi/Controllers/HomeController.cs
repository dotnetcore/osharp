// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-10 16:05</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Collections;
using OSharp.Core.Functions;
using OSharp.Demo.Security.Dtos;
using OSharp.Demo.Security.Entities;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-主页")]
    public class HomeController : Controller
    {
        private readonly IFunctionStore<Function, FunctionInputDto> _functionStore;
        private readonly IModuleStore<Module, ModuleInputDto, int> _moduleStore;

        public HomeController(IFunctionStore<Function, FunctionInputDto> functionStore,
            IModuleStore<Module, ModuleInputDto, int> moduleStore)
        {
            _functionStore = functionStore;
            _moduleStore = moduleStore;
        }

        [Description("首页")]
        public IActionResult Index()
        {
            List<string> list = new List<string>();

            list.Add($"functionStore:{_functionStore.GetType()}, hashCode:{_functionStore.GetHashCode()}");
            list.Add($"moduleStore:{_moduleStore.GetType()}, hashCode:{_moduleStore.GetHashCode()}");

            return Content(list.ExpandAndToString("\n\r"));
        }
    }
}