// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 22:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Dependency
{
    /// <summary>
    /// <see cref="IServiceCollection"/>扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 如果指定服务不存在，添加指定服务
        /// </summary>
        public static ServiceDescriptor GetOrAdd(this IServiceCollection services, ServiceDescriptor toAdDescriptor)
        {
            ServiceDescriptor descriptor = services.FirstOrDefault(m => m.ServiceType == toAdDescriptor.ServiceType);
            if (descriptor != null)
            {
                return descriptor;
            }

            services.Add(toAdDescriptor);
            return toAdDescriptor;
        }

        /// <summary>
        /// 如果指定服务不存在，创建实例并添加
        /// </summary>
        public static TServiceType GetOrAddSingletonInstance<TServiceType>(this IServiceCollection services, Func<TServiceType> factory) where TServiceType : class
        {
            TServiceType item = (TServiceType)services.FirstOrDefault(m => m.ServiceType == typeof(TServiceType) && m.Lifetime == ServiceLifetime.Singleton)?.ImplementationInstance;
            if (item == null)
            {
                item = factory();
                services.AddSingleton<TServiceType>(item);
            }
            return item;
        }

        /// <summary>
        /// 获取单例注册服务对象
        /// </summary>
        public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)services.FirstOrDefault(d => d.ServiceType == typeof(T))?.ImplementationInstance;
        }

        /// <summary>
        /// 获取单例注册服务对象
        /// </summary>
        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            var instance = services.GetSingletonInstanceOrNull<T>();
            if (instance == null)
            {
                throw new InvalidOperationException($"无法找到单例服务：{typeof(T).AssemblyQualifiedName}");
            }

            return instance;
        }
    }
}