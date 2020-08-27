// -----------------------------------------------------------------------
//  <copyright file="SiteControllerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-16 9:04</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;


namespace Liuliu.Demo.Web.Controllers
{
    /// <summary>
    /// 站点根节点的MVC控制器基类，使用OSharpPolicy权限策略
    /// </summary>
    [DisplayName("网站")]
    [Authorize( Policy = FunctionRequirement.OsharpPolicy)]
    public class SiteControllerBase : BaseController
    {
        protected static readonly Random Random = new Random();
    }
}
