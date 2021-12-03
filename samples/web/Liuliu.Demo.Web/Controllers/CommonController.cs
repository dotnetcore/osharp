// -----------------------------------------------------------------------
//  <copyright file="CommonController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Modules;
using OSharp.Data;
using OSharp.Drawing;
using OSharp.IO;
using OSharp.Reflection;

using AssemblyExtensions = OSharp.Reflection.AssemblyExtensions;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-通用")]
    [ModuleInfo(Order = 3)]
    //[ApiExplorerSettings(GroupName = "buss")]
    public class CommonController : SiteApiControllerBase
    {
        private readonly IVerifyCodeService _verifyCodeService;
        private readonly IWebHostEnvironment _environment;

        public CommonController(
            IVerifyCodeService verifyCodeService,
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
        /// 上传图片
        /// </summary>
        [HttpPost]
        [ModuleInfo]
        [Description("上传图片")]
        public async Task<AjaxResult> UploadImage(IFormFile file)
        {
            string fileName = file.FileName;
            fileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{DateTime.Now:MMddHHmmssff}{Path.GetExtension(fileName)}";
            string dir = Path.Combine(_environment.WebRootPath, "upload-files");
            DirectoryHelper.CreateIfNotExists(dir);
            string filePath = dir + $"\\{fileName}";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            return new AjaxResult("上传成功", AjaxResultType.Success, $"upload-files/{fileName}");
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
