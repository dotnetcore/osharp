// -----------------------------------------------------------------------
//  <copyright file="HttpContextServiceScopeFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-20 23:29</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 基于当前HttpContext的<see cref="IServiceScope"/>工厂，如果当前操作处于HttpRequest作用域中，直接使用HttpRequest的作用域，否则创建新的作用域
    /// </summary>
    public class HttpContextServiceScopeFactory : IHybridServiceScopeFactory
    {
        /// <summary>
        /// 初始化一个<see cref="HttpContextServiceScopeFactory"/>类型的新实例
        /// </summary>
        public HttpContextServiceScopeFactory(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            ServiceScopeFactory = serviceScopeFactory;
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取 包装的<see cref="IServiceScopeFactory"/>
        /// </summary>
        public IServiceScopeFactory ServiceScopeFactory { get; }

        /// <summary>
        /// 获取 当前请求的<see cref="IHttpContextAccessor"/>对象
        /// </summary>
        protected IHttpContextAccessor HttpContextAccessor { get; }

        #region Implementation of IServiceScopeFactory

        /// <summary>
        /// 创建依赖注入服务的作用域，如果当前操作处于HttpRequest作用域中，直接使用HttpRequest的作用域，否则创建新的作用域
        /// </summary>
        /// <returns></returns>
        public virtual IServiceScope CreateScope()
        {
            HttpContext httpContext = HttpContextAccessor?.HttpContext;
            //不在HttpRequest作用域中
            if (httpContext == null)
            {
                return ServiceScopeFactory.CreateScope();
            }

            return new NonDisposedHttpContextServiceScope(httpContext.RequestServices);
        }

        #endregion

        /// <summary>
        /// 当前HttpRequest的<see cref="IServiceScope"/>的包装，保持HttpContext.RequestServices的可传递性，并且不释放
        /// </summary>
        protected class NonDisposedHttpContextServiceScope : IServiceScope
        {
            /// <summary>
            /// 初始化一个<see cref="NonDisposedHttpContextServiceScope"/>类型的新实例
            /// </summary>
            public NonDisposedHttpContextServiceScope(IServiceProvider serviceProvider)
            {
                ServiceProvider = serviceProvider;
            }

            /// <summary>
            /// 获取 当前HttpRequest的<see cref="IServiceProvider"/>
            /// </summary>
            public IServiceProvider ServiceProvider { get;  }

            /// <summary>因为是HttpContext的，啥也不做，避免在using使用时被释放</summary>
            public void Dispose()
            { }
        }
    }
}