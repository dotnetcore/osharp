// -----------------------------------------------------------------------
//  <copyright file="SecurityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-11 2:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Mvc;
using OSharp.Collections;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Demo.Security;
using OSharp.Demo.Security.Entities;
using OSharp.Security;
using OSharp.Secutiry;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-授权")]
    [ModuleInfo(Order = 2)]
    public class SecurityController : ApiController
    {
        private readonly SecurityManager _securityManager;

        public SecurityController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }
         
        [ModuleInfo]
        [Description("检查URL授权")]
        public IActionResult CheckUrlAuth(string url)
        {
            bool ok = this.CheckFunctionAuth(url);
            return Json(ok);
        }

        [ModuleInfo]
        [Description("获取授权信息")]
        public IActionResult GetAuthInfo()
        {
            Module[] modules = _securityManager.Modules.ToArray();
            List<AuthItem> list = new List<AuthItem>();
            foreach (Module module in modules)
            {
                if (CheckFuncAuth(module, out bool empty))
                {
                    list.Add(new AuthItem { Code = GetModuleTreeCode(module, modules), HasFunc = !empty });
                }
            }
            List<string> codes = new List<string>();
            foreach (AuthItem item in list)
            {
                if (item.HasFunc)
                {
                    codes.Add(item.Code);
                }
                else if (list.Any(m => m.Code.Length > item.Code.Length && m.Code.Contains(item.Code) && m.HasFunc))
                {
                    codes.Add(item.Code);
                }
            }
            return Json(codes);
            //return Content(codes.ExpandAndToString("\r\n"));
        }


        private class AuthItem
        {
            public string Code { get; set; }

            public bool HasFunc { get; set; }
        }

        private bool CheckFuncAuth(Module module, out bool empty)
        {
            IServiceProvider services = HttpContext.RequestServices;
            IFunctionAuthorization authorization = services.GetService<IFunctionAuthorization>();
            Guid[] functionIds = _securityManager.ModuleFunctions.Where(m => m.ModuleId == module.Id).Select(m => m.FunctionId).ToArray();
            if (functionIds.Length == 0)
            {
                empty = true;
                return true;
            }
            empty = false;
            Function[] functions = _securityManager.Functions.Where(m => functionIds.Contains(m.Id)).ToArray();
            foreach (Function function in functions)
            {
                if (!authorization.Authorize(function, User).IsOk)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取模块的树形路径代码串
        /// </summary>
        public static string GetModuleTreeCode(Module module, Module[] source)
        {
            var pathIds = module.TreePathIds;
            string[] names = pathIds.Select(m => source.First(n => n.Id == m)).Select(m => m.Code).ToArray();
            return names.ExpandAndToString(".");
        }
    }
}