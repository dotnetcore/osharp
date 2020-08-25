// -----------------------------------------------------------------------
//  <copyright file="ApplicationBuilderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-19 1:56</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.Logging;
using OSharp.Reflection;


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
            ILogger logger = provider.GetLogger("ApplicationBuilderExtensions");
            logger.LogInformation(0, "OSharp框架初始化开始");

            // 输出注入服务的日志
            StartupLogger startupLogger = provider.GetService<StartupLogger>();
            startupLogger.Output(provider);

            Stopwatch watch = Stopwatch.StartNew();
            OsharpPack[] packs = provider.GetAllPacks();
            logger.LogInformation($"共有 {packs.Length} 个Pack模块需要初始化");
            foreach (OsharpPack pack in packs)
            {
                Type packType = pack.GetType();
                string packName = packType.GetDescription();
                logger.LogInformation($"正在初始化模块 “{packName} ({packType.Name})”");
                if (pack is AspOsharpPack aspPack)
                {
                    aspPack.UsePack(app);
                }
                else
                {
                    pack.UsePack(provider);
                }
                logger.LogInformation($"模块 “{packName} ({packType.Name})” 初始化完成\n");
            }

            watch.Stop();
            logger.LogInformation(0, $"OSharp框架初始化完成，耗时：{watch.Elapsed}\r\n");

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
                endpoints.MapControllerRoute(
                    name: "areas-router",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            }

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            return endpoints;
        }
    }
}
