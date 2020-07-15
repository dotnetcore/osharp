// -----------------------------------------------------------------------
//  <copyright file="HomeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-13 1:15</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using Liuliu.Demo.Web.Areas.Admin.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using OSharp.Authorization.Modules;
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

        [ModuleInfo]
        [Description("管理首页")]
        public IActionResult Index()
        {
            return View();
        }

        [ModuleInfo]
        [Description("信息汇总")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("admin/home/init")]
        [AllowAnonymous]
        public IActionResult Init()
        {
            HomeInfo home = new HomeInfo() { Title = "主页", Icon = "layui-icon layui-icon-home", Href = Url.Action("Dashboard") };
            LogoInfo logo = new LogoInfo() { Title = "OSHARP", Image = "/images/logo.png", Href = Url.Action("Index") };
            List<MenuInfo> menus = GetMenuInfos();
            InitModel model = new InitModel() { HomeInfo = home, LogoInfo = logo, MenuInfo = menus };
            return Json(model, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private List<MenuInfo> GetMenuInfos()
        {
            List<MenuInfo> menus = new List<MenuInfo>();
            menus.Add(new MenuInfo()
            {
                Title = "业务模块",
                Icon = "layui-icon layui-icon-home",
                Child = new List<MenuInfo>()
            });
            menus.Add(new MenuInfo()
            {
                Title = "基础模块",
                Child = new List<MenuInfo>()
                {
                    new MenuInfo()
                    {
                        Title = "权限模块",
                        Icon = "layui-icon layui-icon-auz",
                        Child = new List<MenuInfo>()
                        {
                            new MenuInfo(){Title = "用户管理", Icon = "layui-icon layui-icon-username", Target = "_self", Href = Url.Action("Index","User")},
                            new MenuInfo(){Title = "角色管理", Icon = "layui-icon layui-icon-user", Target = "_self", Href = Url.Action("Index","Role")},
                            new MenuInfo(){Title = "用户角色管理", Icon = "layui-icon layui-icon-transfer", Target = "_self", Href = Url.Action("Index","UserRole")},
                            new MenuInfo(){Title = "模块管理", Icon = "layui-icon layui-icon-app", Target = "_self", Href = Url.Action("Index", "Module")},
                            new MenuInfo(){Title = "功能管理", Icon = "layui-icon layui-icon-templeate-1", Target = "_self", Href = Url.Action("Index", "Function")},
                            new MenuInfo(){Title = "数据实体管理", Icon = "layui-icon layui-icon-component", Target = "_self", Href = Url.Action("Index", "EntityInfo")},
                            new MenuInfo(){Title = "数据权限管理", Icon = "layui-icon layui-icon-cols", Target = "_self", Href = Url.Action("Index", "RoleEntity")},
                        }
                    },
                    new MenuInfo()
                    {
                        Title = "系统管理",
                        Icon = "layui-icon layui-icon-website",
                        Child = new List<MenuInfo>()
                        {
                            new MenuInfo(){Title = "系统设置", Icon = "layui-icon layui-icon-set", Target = "_self", Href = Url.Action("Settings", "Systems")},
                            new MenuInfo(){Title = "操作审计", Icon = "layui-icon layui-icon-chart-screen", Target = "_self", Href = Url.Action("AuditOperations", "Systems")},
                            new MenuInfo(){Title = "数据审计", Icon = "layui-icon layui-icon-chart", Target = "_self", Href = Url.Action("AuditEntities", "Systems")},
                            new MenuInfo(){Title = "模块包管理", Icon = "layui-icon layui-icon-app", Target = "_self", Href = Url.Action("Index", "Pack")},
                        }
                    }
                }
            });

            return menus;
        }
    }
}
