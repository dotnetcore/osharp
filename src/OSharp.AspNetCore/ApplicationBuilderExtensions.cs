// -----------------------------------------------------------------------
//  <copyright file="ApplicationBuilderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-19 1:56</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Routing;
using OSharp.Core.Packs;
using OSharp.Exceptions;


namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/>辅助扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// OSharp框架初始化，适用于AspNetCore环境
        /// </summary>
        public static IApplicationBuilder UseOSharp(this IApplicationBuilder app)
        {
            IServiceProvider provider = app.ApplicationServices;
            if (!(provider.GetService<IOsharpPackManager>() is IAspUsePack aspPackManager))
            {
                throw new OsharpException("接口 IOsharpPackManager 的注入类型不正确，该类型应同时实现接口 IAspUsePack");
            }
            aspPackManager.UsePack(app);

            return app;
        }

        /// <summary>
        /// 添加MVC并Area路由支持
        /// </summary>
        public static IApplicationBuilder UseMvcWithAreaRoute(this IApplicationBuilder app, bool area = true)
        {
            return app.UseMvc(builder =>
            {
                if (area)
                {
                    builder.MapRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                }
                builder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 添加Endpoint并Area路由支持
        /// </summary>
        public static IEndpointRouteBuilder MapControllersWithAreaRoute(this IEndpointRouteBuilder endpoints, bool area = true)
        {
            if (area)
            {
                endpoints.MapControllerRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            }

            endpoints.MapDefaultControllerRoute();
            return endpoints;
        }
    }
}