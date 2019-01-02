// -----------------------------------------------------------------------
//  <copyright file="ServiceProviderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-19 23:15</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 服务解析扩展
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 获取HttpContext实例
        /// </summary>
        public static HttpContext HttpContext(this IServiceProvider provider)
        {
            IHttpContextAccessor accessor = provider.GetService<IHttpContextAccessor>();
            return accessor?.HttpContext;
        }

        /// <summary>
        /// 当前业务是否处于HttpRequest中
        /// </summary>
        public static bool InHttpRequest(this IServiceProvider provider)
        {
            var context = provider.HttpContext();
            return context != null;
        }
    }
}