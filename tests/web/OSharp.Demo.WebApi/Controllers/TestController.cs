// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-11 11:33</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Demo.WebApi.Areas.Admin.Controllers;
using OSharp.Dependency;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-控制器")]
    public class TestController : Controller
    {
        public TestController()
        {

        }

        public IActionResult Index()
        {
            List<string> list = new List<string>();

            return Content(list.ExpandAndToString("\r\n"));
        }
    }
}