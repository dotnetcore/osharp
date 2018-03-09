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

using OSharp.Core.Builders;
using OSharp.Core.Modules;
using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Reflection;


namespace OSharp
{
    /// <summary>
    /// 依赖注入服务集合扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 将OSharp服务添加到容器
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services, Action<IOSharpBuilder> builderAction = null, AppServiceScanOptions scanOptions = null)
        {
            Check.NotNull(services, nameof(services));

            IOSharpBuilder builder = new OSharpBuilder();
            if (builderAction != null)
            {
                builderAction(builder);
            }
            OSharpModuleManager manager = new OSharpModuleManager(builder, new AppDomainAllAssemblyFinder());
            manager.LoadModules(services);
            services.AddSingleton(provider => manager);
            if (scanOptions == null)
            {
                scanOptions = new AppServiceScanOptions();
            }
            services = new AppServiceAdder(scanOptions).AddServices(services);
            if (builder.OptionsAction != null)
            {
                services.Configure(builder.OptionsAction);
            }
            return services;
        }
    }
}