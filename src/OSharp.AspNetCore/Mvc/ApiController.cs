// -----------------------------------------------------------------------
//  <copyright file="ApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-04 20:30</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Dependency;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// WebApi控制器基类
    /// </summary>
    [AuditOperation]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class ApiController : Controller
    {
        /// <summary>
        /// 初始化一个<see cref="ApiController"/>类型的新实例
        /// </summary>
        protected ApiController()
        {
            Logger = ServiceLocator.Instance.GetService<ILoggerFactory>().CreateLogger(GetType());
        }

        /// <summary>
        /// 获取或设置 日志对象
        /// </summary>
        protected ILogger Logger { get; }
    }
}