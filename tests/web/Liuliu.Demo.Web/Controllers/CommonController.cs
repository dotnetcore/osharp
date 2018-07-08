// -----------------------------------------------------------------------
//  <copyright file="CommonController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;

using Liuliu.Demo.Common;
using Liuliu.Demo.Security;
using Liuliu.Demo.Security.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Core.Modules;
using OSharp.Core.Packs;
using OSharp.Drawing;
using OSharp.Filter;
using OSharp.Reflection;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-通用")]
    [ModuleInfo(Order = 3)]
    public class CommonController : ApiController
    {
        private readonly ICommonContract _commonContract;
        private readonly SecurityManager _securityManager;

        public CommonController(ICommonContract commonContract, SecurityManager securityManager)
        {
            _commonContract = commonContract;
            _securityManager = securityManager;
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
            VerifyCodeHandler.SetCode(code, out string id);
            return VerifyCodeHandler.GetImageString(bitmap, id);
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
            return VerifyCodeHandler.CheckCode(code, id, false);
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

            info.Lines = new List<string>()
            {
                "WebApi 数据服务已启动",
                $"当前版本：{version}"
            };

            return info;
        }

        [HttpGet]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("测试")]
        public object Test()
        {
            EntityRoleInputDto dto = new EntityRoleInputDto()
            {
                RoleId = 3,
                EntityId = Guid.Parse("a0f5a8cf-f774-45e2-be2f-a9130053ab73"),
                FilterGroup = new FilterGroup()
            };
            dto.FilterGroup.AddRule(new FilterRule("Id", 5, FilterOperate.GreaterOrEqual));
            return _securityManager.CreateEntityRoles(dto);
        }
    }
}