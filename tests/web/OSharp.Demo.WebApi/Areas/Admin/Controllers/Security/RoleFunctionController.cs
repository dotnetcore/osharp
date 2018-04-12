// -----------------------------------------------------------------------
//  <copyright file="RoleFunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-13 2:03</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Demo.Security;
using OSharp.Filter;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Description("管理-角色功能")]
    public class RoleFunctionController : AreaApiController
    {
        private readonly SecurityManager _securityManager;

        public RoleFunctionController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [Description("读取")]
        public IActionResult Read()
        {
            return Json(new PageData<object>());
        }
    }
}