// -----------------------------------------------------------------------
//  <copyright file="CommonController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-21 19:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OSharp.AspNetCore.Mvc;
using OSharp.Collections;
using OSharp.Core.Packs;
using OSharp.Entity;
using OSharp.Reflection;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-通用")]
    [ModuleInfo(Order = 3)]
    public class CommonController : ApiController
    {
        [ModuleInfo]
        [Description("验证码")]
        public IActionResult VerifyCode()
        {
            return Json("");
        }

        [ModuleInfo]
        [Description("系统信息")]
        public IActionResult SystemInfo()
        {
            IServiceProvider provider = HttpContext.RequestServices;

            dynamic info = new ExpandoObject();
            OSharpPackManager packManager = provider.GetService<OSharpPackManager>();
            info.Packs = packManager.SourcePacks.OrderBy(m => m.Level).ThenBy(m => m.Order).ThenBy(m => m.GetType().FullName).Select(m => new
            {
                m.GetType().Name,
                Class = m.GetType().FullName,
                Level = m.Level.ToString(),
                m.Order,
                m.IsEnabled
            }).ToList();

            string version = Assembly.GetExecutingAssembly().GetProductVersion();

            MvcOptions mvcOps = provider.GetService<IOptions<MvcOptions>>().Value;

            info.Lines = new List<string>()
            {
                "WebApi 数据服务已启动",
                $"版本号：{version}",
                $"数据连接：{provider.GetOSharpOptions().GetDbContextOptions(typeof(DefaultDbContext)).ConnectionString}",
                $"MvcFilters：\r\n{mvcOps.Filters.ExpandAndToString(m => $"{m.ToString()}-{m.GetHashCode()}", "\r\n")}"
            };

            return Json(info);
        }
    }
}