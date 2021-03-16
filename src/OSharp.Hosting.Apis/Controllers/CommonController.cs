// -----------------------------------------------------------------------
//  <copyright file="CommonController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-12 16:30</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.Authorization.Modules;
using OSharp.Drawing;
using OSharp.Reflection;

using AssemblyExtensions = OSharp.Reflection.AssemblyExtensions;


namespace OSharp.Hosting.Apis.Controllers
{
    [Description("网站-通用")]
    [ModuleInfo(Order = 3)]
    public class CommonController : SiteApiControllerBase
    {
        private readonly IVerifyCodeService _verifyCodeService;
        private readonly IWebHostEnvironment _environment;

        public CommonController(IVerifyCodeService verifyCodeService,
            IWebHostEnvironment environment)
        {
            _verifyCodeService = verifyCodeService;
            _environment = environment;
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns>验证码图片文件</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("验证码")]
        public string VerifyCode()
        {
            ValidateCoder coder = new ValidateCoder()
            {
                RandomColor = true,
                RandomItalic = true,
                RandomLineCount = 7,
                RandomPointPercent = 10,
                RandomPosition = true
            };
            Bitmap bitmap = coder.CreateImage(4, out string code);
            string id = _verifyCodeService.SetCode(code);
            return _verifyCodeService.GetImageString(bitmap, id);
        }

        /// <summary>
        /// 验证验证码的有效性，只作为前端Ajax验证，验证成功不移除验证码，验证码仍需传到后端进行再次验证
        /// </summary>
        /// <param name="code">验证码字符串</param>
        /// <param name="id">验证码编号</param>
        /// <returns>是否无效</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("验证验证码的有效性")]
        public bool CheckVerifyCode(string code, string id)
        {
            return _verifyCodeService.CheckCode(code, id, false);
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <returns>系统信息</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("系统信息")]
        public object SystemInfo()
        {
            IServiceProvider provider = HttpContext.RequestServices;

            dynamic info = new ExpandoObject();
            info.Packs = provider.GetAllPacks().Select(m => new
            {
                m.GetType().Name,
                Class = m.GetType().FullName,
                Level = m.Level.ToString(),
                m.Order,
                m.IsEnabled
            }).ToList();

            string cliVersion = AssemblyExtensions.GetCliVersion();
            string osharpVersion = Assembly.GetExecutingAssembly().GetProductVersion();

            info.Object = new
            {
                Message = "WebApi 数据服务已启动",
                CliVersion = cliVersion,
                OSharpVersion = osharpVersion
            };

            return info;
        }
    }
}
