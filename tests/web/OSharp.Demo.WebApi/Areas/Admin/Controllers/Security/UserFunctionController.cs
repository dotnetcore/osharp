// -----------------------------------------------------------------------
//  <copyright file="UserFunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-13 2:09</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Description("管理-用户功能")]
    public class UserFunctionController : AreaApiController
    {
        [Description("读取")]
        public IActionResult Read()
        {
            return Json(new List<object>());
        }
    }
}