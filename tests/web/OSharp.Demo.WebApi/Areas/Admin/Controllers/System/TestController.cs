// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-13 1:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Description("管理-测试")]
    public class TestController : AdminApiController
    {
        public string[] GetLines()
        {
            return GetType().Namespace.Split('.').Concat(new[] { DateTime.Now.ToJsGetTime() }).ToArray();
        }
    }
}