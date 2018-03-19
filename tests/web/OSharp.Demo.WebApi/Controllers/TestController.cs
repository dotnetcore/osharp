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

using OSharp.Collections;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-测试")]
    public class TestController : Controller
    {
        [Description("首页")]
        public IActionResult Index()
        {
            List<string> list = new List<string>();
            list.Add("test");
            return Content(list.ExpandAndToString("\r\n"));
        }
    }
}