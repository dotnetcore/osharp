// -----------------------------------------------------------------------
//  <copyright file="AreaApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-09 20:32</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore.Mvc.Filters;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// 区域的WebApi控制器基类，配置了操作审计和区域API路由
    /// </summary>
    [AuditOperation]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public abstract class AreaApiControllerBase : ControllerBase
    {
        /// <summary>
        /// 获取或设置 日志对象
        /// </summary>
        protected ILogger Logger => HttpContext.RequestServices.GetLogger(GetType());
    }
}
