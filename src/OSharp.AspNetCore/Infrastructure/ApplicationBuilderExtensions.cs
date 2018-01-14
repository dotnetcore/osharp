// -----------------------------------------------------------------------
//  <copyright file="ApplicationBuilderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-19 1:56</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.EventBuses;
using OSharp.Infrastructure;


namespace OSharp
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/>辅助扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// OSharp框架初始化
        /// </summary>
        public static IApplicationBuilder UseOSharp(this IApplicationBuilder app)
        {
            IServiceProvider serviceProvider = app.ApplicationServices;
            //应用程序级别的服务定位器
            ServiceLocator.Instance.TrySetApplicationServiceProvider(serviceProvider);

            //事件总线
            IEventBusBuilder eventBusBuilder = serviceProvider.GetService<IEventBusBuilder>();
            eventBusBuilder.Build();

            //实体信息初始化
            IEntityInfoHandler entityInfoHandler = serviceProvider.GetService<IEntityInfoHandler>();
            entityInfoHandler.Initialize();

            //功能信息初始化
            IFunctionHandler[] functionHandlers = serviceProvider.GetServices<IFunctionHandler>().ToArray();
            foreach (IFunctionHandler functionHandler in functionHandlers)
            {
                functionHandler.Initialize();
            }

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
    }
}