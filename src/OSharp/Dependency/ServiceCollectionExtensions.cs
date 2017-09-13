// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 23:14</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp;
using OSharp.Dependency;
using OSharp.Options;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 将应用程序服务添加到<see cref="IServiceCollection"/> 
        /// 检索程序集，查找实现了<see cref="ITransientDependency"/>，<see cref="IScopeDependency"/>，<see cref="ISingletonDependency"/> 接口的所有服务，分别按生命周期类型进行添加
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services, AppServiceScanOptions scanOptions = null)
        {
            if (scanOptions == null)
            {
                scanOptions = new AppServiceScanOptions();
            }
            return new AppServiceAdder(scanOptions).AddServices(services);
        }

        /// <summary>
        /// 将应用程序服务添加到<see cref="IServiceCollection"/> 
        /// 检索程序集，查找实现了<see cref="ITransientDependency"/>，<see cref="IScopeDependency"/>，<see cref="ISingletonDependency"/> 接口的所有服务，分别按生命周期类型进行添加
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services, Action<OSharpOptions> configureOptions, AppServiceScanOptions scanOptions = null)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(configureOptions, nameof(configureOptions));

            services.AddOSharp(scanOptions);
            services.Configure(configureOptions);
            return services;
        }
    }
}