// -----------------------------------------------------------------------
//  <copyright file="MvcControllerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-12 18:21</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore.Mvc.Filters;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// Mvc控制器基类，配置了操作审计
    /// </summary>
    [AuditOperation]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 获取或设置 日志对象
        /// </summary>
        protected ILogger Logger => HttpContext.RequestServices.GetLogger(GetType());
    }
}
