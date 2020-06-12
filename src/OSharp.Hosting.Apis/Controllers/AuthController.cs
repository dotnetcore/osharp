// -----------------------------------------------------------------------
//  <copyright file="AuthController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-12 12:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Authorization.Modules;
using OSharp.Hosting.Authorization;


namespace OSharp.Hosting.Apis.Controllers
{
    [Description("网站-授权")]
    [ModuleInfo(Order = 2)]
    public class AuthController : SiteApiControllerBase
    {
        private readonly FunctionAuthManager _functionAuthManager;

        public AuthController(FunctionAuthManager functionAuthManager)
        {
            _functionAuthManager = functionAuthManager;
        }

        /// <summary>
        /// 检查URL授权
        /// </summary>
        /// <param name="url">要检查的URL</param>
        /// <returns>是否有权</returns>
        [HttpGet]
        [UnitOfWork]
        [Description("检查URL授权")]
        public bool CheckUrlAuth(string url)
        {
            bool flag = this.CheckFunctionAuth(url);
            return flag;
        }

        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <returns>权限节点</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取授权信息")]
        public List<string> GetAuthInfo()
        {
            throw new NotImplementedException();
        }
    }
}
