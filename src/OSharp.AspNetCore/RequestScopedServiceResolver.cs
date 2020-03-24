// -----------------------------------------------------------------------
//  <copyright file="RequestScopedServiceResolver.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 20:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// Request的<see cref="ServiceLifetime.Scoped"/>服务解析器
    /// </summary>
    public class RequestScopedServiceResolver : IScopedServiceResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 初始化一个<see cref="RequestScopedServiceResolver"/>类型的新实例
        /// </summary>
        public RequestScopedServiceResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取 是否可解析
        /// </summary>
        public bool ResolveEnabled => _httpContextAccessor.HttpContext != null;

        /// <summary>
        /// 获取 <see cref="ServiceLifetime.Scoped"/>生命周期的服务提供者
        /// </summary>
        public IServiceProvider ScopedProvider => _httpContextAccessor.HttpContext.RequestServices;

        /// <summary>
        /// 获取指定服务类型的实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService<T>();
        }

        /// <summary>
        /// 获取指定服务类型的实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService(serviceType);
        }

        /// <summary>
        /// 获取指定服务类型的所有实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetServices<T>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetServices<T>();
        }

        /// <summary>
        /// 获取指定服务类型的所有实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetServices(serviceType);
        }
    }
}