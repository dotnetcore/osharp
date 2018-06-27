// -----------------------------------------------------------------------
//  <copyright file="ServiceLocatorExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-29 4:31</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;

using OSharp.Dependency;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 服务定位器扩展方法
    /// </summary>
    public static class ServiceLocatorExtensions
    {
        /// <summary>
        /// 获取HttpContext的实例
        /// </summary>
        public static HttpContext HttpContext(this ServiceLocator locator)
        {
            IHttpContextAccessor accessor = locator.GetService<IHttpContextAccessor>();
            return accessor?.HttpContext;
        }
    }
}