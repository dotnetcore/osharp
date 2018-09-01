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
using OSharp.Entity;
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
        public static IServiceCollection AddOSharp<TOsharpPackManager>(this IServiceCollection services, Action<IOsharpBuilder> builderAction = null)
            where TOsharpPackManager : IOsharpPackManager, new()
        {
            Check.NotNull(services, nameof(services));

            //初始化所有程序集查找器，如需更改程序集查找逻辑，请事先赋予自定义查找器的实例
            if (Singleton<IAllAssemblyFinder>.Instance == null)
            {
                Singleton<IAllAssemblyFinder>.Instance = new AppDomainAllAssemblyFinder();
            }

            IOsharpBuilder builder = new OsharpBuilder();
            if (builderAction != null)
            {
                builderAction(builder);
            }
            Singleton<IOsharpBuilder>.Instance = builder;
            TOsharpPackManager manager = new TOsharpPackManager();
            services.AddSingleton<IOsharpPackManager>(manager);
            manager.LoadPacks(services);
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

        /// <summary>
        /// 获取指定实体类的上下文所在工作单元
        /// </summary>
        public static IUnitOfWork GetUnitOfWork<TEntity, TKey>(this IServiceProvider provider) where TEntity : IEntity<TKey>
        {
            IUnitOfWorkManager unitOfWorkManager = provider.GetService<IUnitOfWorkManager>();
            return unitOfWorkManager.GetUnitOfWork<TEntity, TKey>();
        }

        /// <summary>
        /// OSharp框架初始化，适用于非AspNetCore环境
        /// </summary>
        public static IServiceProvider UseOsharp(this IServiceProvider provider)
        {
            IOsharpPackManager packManager = provider.GetService<IOsharpPackManager>();
            packManager.UsePack(provider);
            return provider;
        }
    }
}