// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-08 0:58</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Modules;
using OSharp.Dependency;
using OSharp.Options;
using OSharp.Reflection;


namespace OSharp
{
    /// <summary>
    /// 依赖注入服务集合扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 将模块服务注册到依赖注入容器中
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services, AppServiceScanOptions scanOptions = null)
        {
            if (scanOptions == null)
            {
                scanOptions = new AppServiceScanOptions();
            }
            services = new AppServiceAdder(scanOptions).AddServices(services);

            OSharpModuleManager moduleManager = new OSharpModuleManager(new AppDomainAllAssemblyFinder());
            moduleManager.LoadModules(services);
            services.AddSingleton(provider => moduleManager);

            ServiceLocator.Instance.TrySetServiceCollection(services);
            return services;
        }

        /// <summary>
        /// 将模块服务注册到依赖注入容器中
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