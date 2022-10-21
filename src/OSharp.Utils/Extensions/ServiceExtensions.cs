// -----------------------------------------------------------------------
//  <copyright file="ServiceExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-25 1:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using OSharp.Data;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        #region IServiceCollection
        
        /// <summary>
        /// 判断指定服务类型是否存在
        /// </summary>
        public static bool AnyServiceType(this IServiceCollection services, Type serviceType)
        {
            return services.Any(m => m.ServiceType == serviceType);
        }

        /// <summary>
        /// 替换服务
        /// </summary>
        public static IServiceCollection Replace<TService, TImplement>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(typeof(TService), typeof(TImplement), lifetime);
            services.Replace(descriptor);
            return services;
        }

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
        /// 获取单例注册服务对象
        /// </summary>
        public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            ServiceDescriptor descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T) && d.Lifetime == ServiceLifetime.Singleton);

            if (descriptor?.ImplementationInstance != null)
            {
                return (T)descriptor.ImplementationInstance;
            }

            if (descriptor?.ImplementationFactory != null)
            {
                return (T)descriptor.ImplementationFactory.Invoke(null);
            }

            return default;
        }

        /// <summary>
        /// 获取单例注册服务对象
        /// </summary>
        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            var instance = services.GetSingletonInstanceOrNull<T>();
            if (instance == null)
            {
                throw new InvalidOperationException($"无法找到已注册的单例服务：{typeof(T).AssemblyQualifiedName}");
            }

            return instance;
        }
        
        #endregion

        #region IServiceProvider
        
        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <typeparam name="T">非静态强类型</typeparam>
        /// <returns>日志对象</returns>
        public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
        {
            ILoggerFactory factory = provider.GetRequiredService<ILoggerFactory>();
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
            Check.NotNull(type, nameof(type));
            ILoggerFactory factory = provider.GetRequiredService<ILoggerFactory>();
            return factory.CreateLogger(type);
        }

        /// <summary>
        /// 获取指定对象类型的日志对象
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="instance">要获取日志的类型对象，一般指当前类，即this</param>
        public static ILogger GetLogger(this IServiceProvider provider, object instance)
        {
            Check.NotNull(instance, nameof(instance));
            ILoggerFactory factory = provider.GetRequiredService<ILoggerFactory>();
            return factory.CreateLogger(instance.GetType());
        }

        /// <summary>
        /// 获取指定名称的日志对象
        /// </summary>
        public static ILogger GetLogger(this IServiceProvider provider, string name)
        {
            ILoggerFactory factory = provider.GetRequiredService<ILoggerFactory>();
            return factory.CreateLogger(name);
        }
        
        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑
        /// </summary>
        public static void ExecuteScopedWork(this IServiceProvider rootProvider, Action<IServiceProvider> action)
        {
            using (IServiceScope scope = rootProvider.CreateScope())
            {
                action(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 异步执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑
        /// </summary>
        public static async Task ExecuteScopedWorkAsync(this IServiceProvider rootProvider, Func<IServiceProvider, Task> action)
        {
            using (IServiceScope scope = rootProvider.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑，并获取返回值
        /// </summary>
        public static TResult ExecuteScopedWork<TResult>(this IServiceProvider rootProvider, Func<IServiceProvider, TResult> func)
        {
            using (IServiceScope scope = rootProvider.CreateScope())
            {
                return func(scope.ServiceProvider);
            }
        }
        
        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑，并获取返回值
        /// </summary>
        public static async Task<TResult> ExecuteScopedWorkAsync<TResult>(this IServiceProvider rootProvider, Func<IServiceProvider, Task<TResult>> func)
        {
            using (IServiceScope scope = rootProvider.CreateScope())
            {
                return await func(scope.ServiceProvider);
            }
        }
        
        #endregion

    }
}
