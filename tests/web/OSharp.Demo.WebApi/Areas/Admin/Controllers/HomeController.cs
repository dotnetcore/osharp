// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-09 20:37</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Demo.Security;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Description("管理-主页")] 
    public class HomeController : AdminApiController
    {
        private readonly SecurityManager _securityManager;

        public HomeController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [HttpGet]
        [Description("主菜单")]
        public ActionResult MainMenu()
        {















            return Content("MainMenu");
        }
    }
}