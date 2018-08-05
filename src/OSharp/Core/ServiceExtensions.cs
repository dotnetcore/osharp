// -----------------------------------------------------------------------
//  <copyright file="ServiceExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-26 12:22</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OSharp.Core.Builders;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Reflection;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入服务集合扩展
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 将OSharp服务，各个<see cref="OsharpPack"/>模块的服务添加到服务容器中
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services, Action<IOSharpBuilder> builderAction = null)
        {
            Check.NotNull(services, nameof(services));

            IOSharpBuilder builder = new OSharpBuilder();
            if (builderAction != null)
            {
                builderAction(builder);
            }
            OSharpPackManager manager = new OSharpPackManager(builder, new AppDomainAllAssemblyFinder());
            manager.LoadPacks(services);
            services.AddSingleton(provider => manager);
            return services;
        }

        /// <summary>
        /// 从服务提供者中获取OSharpOptions
        /// </summary>
        public static OSharpOptions GetOSharpOptions(this IServiceProvider provider)
        {
            return provider.GetService<IOptions<OSharpOptions>>()?.Value;
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <typeparam name="T">非静态强类型</typeparam>
        /// <returns>日志对象</returns>
        public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger<T>();
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type">指定类型</param>
        /// <returns>日志对象</returns>
        public static ILogger GetLogger(this IServiceProvider provider, Type type)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(type);
        }

        /// <summary>
        /// 获取指定名称的日志对象
        /// </summary>
        public static ILogger GetLogger(this IServiceProvider provider, string name)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(name);
        }


    }
}