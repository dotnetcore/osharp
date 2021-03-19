// -----------------------------------------------------------------------
//  <copyright file="ServiceExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-25 1:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OSharp.Core.Builders;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Logging;
using OSharp.Reflection;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        #region IServiceCollection

        /// <summary>
        /// 创建OSharp构建器，开始构建OSharp服务
        /// </summary>
        public static IOsharpBuilder AddOSharp(this IServiceCollection services, Action<OsharpOptions> optionAction = null)
        {
            Check.NotNull(services, nameof(services));

            //初始化所有程序集查找器
            services.GetOrAddSingletonInstance(() => new StartupLogger());

            IOsharpBuilder builder = services.GetOrAddSingletonInstance<IOsharpBuilder>(() => new OsharpBuilder(services));
            builder.AddCorePack();

            optionAction?.Invoke(builder.Options);

            return builder;
        }

        /// <summary>
        /// 获取<see cref="IConfiguration"/>配置信息
        /// </summary>
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.GetSingletonInstanceOrNull<IConfiguration>();
        }

        /// <summary>
        /// 获取<see cref="OsharpOptions"/>配置信息
        /// </summary>
        public static OsharpOptions GetOsharpOptions(this IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            return configuration.GetOsharpOptions();
        }

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
        /// 如果指定服务不存在，创建实例并添加
        /// </summary>
        public static TServiceType GetOrAddSingletonInstance<TServiceType>(this IServiceCollection services, Func<TServiceType> factory) where TServiceType : class
        {
            TServiceType item = GetSingletonInstanceOrNull<TServiceType>(services);
            if (item == null)
            {
                item = factory();
                services.AddSingleton<TServiceType>(item);
                services.ServiceLogDebug(typeof(TServiceType), item.GetType(), nameof(ServiceExtensions));
            }
            return item;
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

        /// <summary>
        /// 添加服务调试日志
        /// </summary>
        public static IServiceCollection ServiceLogDebug(this IServiceCollection services, ServiceDescriptor[] oldDescriptors, string logName)
        {
            var list = services.Except(oldDescriptors);
            foreach (ServiceDescriptor desc in list)
            {
                if (desc.ImplementationType != null)
                {
                    services.ServiceLogDebug(desc.ServiceType, desc.ImplementationType, logName, desc.Lifetime);
                    continue;
                }

                if (desc.ImplementationInstance != null)
                {
                    services.ServiceLogDebug(desc.ServiceType, desc.ImplementationInstance.GetType(), logName, desc.Lifetime);
                }
            }

            return services;
        }

        /// <summary>
        /// 添加服务调试日志
        /// </summary>
        public static IServiceCollection ServiceLogDebug<TServiceType, TImplementType>(this IServiceCollection services, string logName, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            Type serviceType = typeof(TServiceType), implementType = typeof(TImplementType);
            return services.ServiceLogDebug(serviceType, implementType, logName, lifetime);
        }

        /// <summary>
        /// 添加服务调试日志
        /// </summary>
        public static IServiceCollection ServiceLogDebug(this IServiceCollection services, Type serviceType, Type implementType, string logName, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            string lifetimeType = lifetime == ServiceLifetime.Singleton ? "单例" : lifetime == ServiceLifetime.Scoped ? "作用域" : "瞬时";
            return services.LogDebug($"添加服务，{lifetimeType}，{serviceType.FullName} -> {implementType.FullName}", logName);
        }

        /// <summary>
        /// 添加启动调试日志
        /// </summary>
        public static IServiceCollection LogDebug(this IServiceCollection services, string message, string logName)
        {
            StartupLogger logger = services.GetOrAddSingletonInstance(() => new StartupLogger());
            logger.LogDebug(message, logName);
            return services;
        }

        /// <summary>
        /// 添加启动消息日志
        /// </summary>
        public static IServiceCollection LogInformation(this IServiceCollection services, string message, string logName)
        {
            StartupLogger logger = services.GetOrAddSingletonInstance(() => new StartupLogger());
            logger.LogInformation(message, logName);
            return services;
        }

        /// <summary>
        /// 加载事件处理器
        /// </summary>
        public static IServiceCollection AddEventHandler<T>(this IServiceCollection services) where T : class, IEventHandler
        {
            return services.AddTransient<T>();
        }

        /// <summary>
        /// 从Scoped字典中获取指定类型的值
        /// </summary>
        public static T GetValue<T>(this ScopedDictionary dict, string key) where T : class
        {
            if (dict.TryGetValue(key, out object obj))
            {
                return obj as T;
            }

            return default(T);
        }

        #endregion

        #region IServiceProvider

        /// <summary>
        /// 从服务提供者中获取OSharpOptions
        /// </summary>
        public static OsharpOptions GetOSharpOptions(this IServiceProvider provider)
        {
            return provider.GetService<IOptions<OsharpOptions>>()?.Value;
        }

        /// <summary>
        /// 从服务提供者获取 <see cref="IUnitOfWork"/>
        /// </summary>
        /// <param name="provider">服务提供者</param>
        /// <param name="enableTransaction">是否启用事务</param>
        /// <returns></returns>
        public static IUnitOfWork GetUnitOfWork(this IServiceProvider provider, bool enableTransaction = false)
        {
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            if (enableTransaction)
            {
                unitOfWork.EnableTransaction();
            }

            return unitOfWork;
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
            Check.NotNull(type, nameof(type));
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
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
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
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
        /// 获取指定实体类型的上下文对象
        /// </summary>
        public static IDbContext GetDbContext<TEntity, TKey>(this IServiceProvider provider) where TEntity : IEntity<TKey>
        {
            IUnitOfWork unitOfWork = provider.GetUnitOfWork();
            return unitOfWork.GetEntityDbContext<TEntity, TKey>();
        }

        /// <summary>
        /// 获取所有模块信息
        /// </summary>
        public static OsharpPack[] GetAllPacks(this IServiceProvider provider)
        {
            OsharpPack[] packs = provider.GetServices<OsharpPack>().OrderBy(m => m.Level).ThenBy(m => m.Order).ThenBy(m => m.GetType().FullName).ToArray();
            return packs;
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        public static ClaimsPrincipal GetCurrentUser(this IServiceProvider provider)
        {
            try
            {
                IPrincipal user = provider.GetService<IPrincipal>();
                return user as ClaimsPrincipal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// OSharp框架初始化，适用于非AspNetCore环境
        /// </summary>
        public static IServiceProvider UseOsharp(this IServiceProvider provider)
        {
            ILogger logger = provider.GetLogger(typeof(ServiceExtensions));
            logger.LogInformation("OSharp框架初始化开始");
            Stopwatch watch = Stopwatch.StartNew();

            OsharpPack[] packs = provider.GetServices<OsharpPack>().ToArray();
            foreach (OsharpPack pack in packs)
            {
                pack.UsePack(provider);
                logger.LogInformation($"模块{pack.GetType()}加载成功");
            }

            watch.Stop();
            logger.LogInformation($"OSharp框架初始化完毕，耗时：{watch.Elapsed}");

            return provider;
        }

        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑
        /// </summary>
        public static void ExecuteScopedWork(this IServiceProvider provider, Action<IServiceProvider> action)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                action(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 异步执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑
        /// </summary>
        public static async Task ExecuteScopedWorkAsync(this IServiceProvider provider, Func<IServiceProvider, Task> action)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑，并获取返回值
        /// </summary>
        public static TResult ExecuteScopedWork<TResult>(this IServiceProvider provider, Func<IServiceProvider, TResult> func)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                return func(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑，并获取返回值
        /// </summary>
        public static async Task<TResult> ExecuteScopedWorkAsync<TResult>(this IServiceProvider provider, Func<IServiceProvider, Task<TResult>> func)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                return await func(scope.ServiceProvider);
            }

        }

        /// <summary>
        /// 开启一个事务处理
        /// </summary>
        /// <param name="provider">信赖注入服务提供程序</param>
        /// <param name="action">要执行的业务委托</param>
        public static void BeginUnitOfWorkTransaction(this IServiceProvider provider, Action<IServiceProvider> action)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(action, nameof(action));

            using (IServiceScope scope = provider.CreateScope())
            {
                IServiceProvider scopeProvider = scope.ServiceProvider;
                IUnitOfWork unitOfWork = scopeProvider.GetUnitOfWork(true);
                action(scopeProvider);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// 开启一个事务处理
        /// </summary>
        /// <param name="provider">信赖注入服务提供程序</param>
        /// <param name="actionAsync">要执行的业务委托</param>
        public static async Task BeginUnitOfWorkTransactionAsync(this IServiceProvider provider,
            Func<IServiceProvider, Task> actionAsync)
        {
            Check.NotNull(provider, nameof(provider));
            Check.NotNull(actionAsync, nameof(actionAsync));

            using (IServiceScope scope = provider.CreateScope())
            {
                IServiceProvider scopeProvider = scope.ServiceProvider;

                IUnitOfWork unitOfWork = scopeProvider.GetUnitOfWork(true);
                await actionAsync(scopeProvider);
#if NET5_0
                await unitOfWork.CommitAsync();
#else
                unitOfWork.Commit();
#endif
            }
        }
        #endregion

    }
}