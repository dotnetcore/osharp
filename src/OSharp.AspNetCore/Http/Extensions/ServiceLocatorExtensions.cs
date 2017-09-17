// -----------------------------------------------------------------------
//  <copyright file="ServiceLocatorExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-17 16:05</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace OSharp
{
    /// <summary>
    /// <see cref="ServiceLocator"/>扩展方法
    /// </summary>
    public static class ServiceLocatorExtensions
    {
        /// <summary>
        /// 获取<see cref="ServiceLifetime.Scoped"/>生命周期的服务实例
        /// </summary>
        public static T GetScopedService<T>(this ServiceLocator locator)
        {
            Check.NotNull(locator, nameof(locator));

            IHttpContextAccessor accessor = locator.GetService<IHttpContextAccessor>();
            if (accessor != null)
            {
                return accessor.HttpContext.RequestServices.GetService<T>();
            }
            return locator.GetService<T>();
        }

        /// <summary>
        /// 获取<see cref="ServiceLifetime.Scoped"/>生命周期的服务的所有实例
        /// </summary>
        public static IEnumerable<T> GetScopedServices<T>(this ServiceLocator locator)
        {
            Check.NotNull(locator, nameof(locator));

            IHttpContextAccessor accessor = locator.GetService<IHttpContextAccessor>();
            if (accessor != null)
            {
                return accessor.HttpContext.RequestServices.GetServices<T>();
            }
            return locator.GetServices<T>();
        }
    }
}