// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-13 1:15</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Liuliu.Demo.Web.Areas.Admin.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OSharp.Hosting.Authorization;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminControllerBase
    {
        private readonly FunctionAuthManager _functionAuthManager;

        /// <summary>
        /// 初始化一个<see cref="HomeController"/>类型的新实例
        /// </summary>
        public HomeController(FunctionAuthManager functionAuthManager)
        {
            _functionAuthManager = functionAuthManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Description("信息汇总")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("admin/home/init")]
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public InitModel Init()
        {
            HomeInfo home = new HomeInfo() { Title = "信息汇总", Href = Url.Action("Dashboard", new { Id3 = "Gmf" }) };
            LogoInfo logo = new LogoInfo() { Title = "OSHARP", Image = "images/logo.png", Href = Url.Action("Index") };
            MenuInfo menu = new MenuInfo();
            return new InitModel() { HomeInfo = home, LogoInfo = logo, MenuInfo = menu };
        }

        private MenuInfo GetMenuInfo()
        {
            return null;
        }
    }
}
