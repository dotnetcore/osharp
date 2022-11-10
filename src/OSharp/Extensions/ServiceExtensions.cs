// -----------------------------------------------------------------------
//  <copyright file="ServiceExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-25 1:53</last-date>
// -----------------------------------------------------------------------


namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions2
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
    /// 如果指定服务不存在，创建实例并添加
    /// </summary>
    public static TServiceType GetOrAddSingletonInstance<TServiceType>(this IServiceCollection services, Func<TServiceType> factory) where TServiceType : class
    {
        TServiceType item = services.GetSingletonInstanceOrNull<TServiceType>();
        if (item == null)
        {
            item = factory();
            services.AddSingleton<TServiceType>(item);
            services.ServiceLogDebug(typeof(TServiceType), item.GetType(), "ServiceExtensions");
        }
        return item;
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
        ILogger logger = provider.GetLogger(typeof(ServiceExtensions2));
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
    /// 开启一个事务处理
    /// </summary>
    /// <param name="rootProvider">根依赖注入服务提供程序</param>
    /// <param name="action">要执行的业务委托</param>
    public static void BeginUnitOfWorkTransaction(this IServiceProvider rootProvider, Action<IServiceProvider> action)
    {
        Check.NotNull(rootProvider, nameof(rootProvider));
        Check.NotNull(action, nameof(action));

        using IServiceScope scope = rootProvider.CreateScope();
        IServiceProvider scopeProvider = scope.ServiceProvider;
        IUnitOfWork unitOfWork = scopeProvider.GetUnitOfWork(true);
        action(scopeProvider);
        unitOfWork.Commit();
    }

    /// <summary>
    /// 开启一个事务处理
    /// </summary>
    /// <param name="rootProvider">根依赖注入服务提供程序</param>
    /// <param name="func">要执行的业务委托</param>
    public static TResult BeginUnitOfWorkTransaction<TResult>(this IServiceProvider rootProvider, Func<IServiceProvider, TResult> func)
    {
        Check.NotNull(rootProvider, nameof(rootProvider));
        Check.NotNull(func, nameof(func));

        using IServiceScope scope = rootProvider.CreateScope();
        IServiceProvider scopeProvider = scope.ServiceProvider;
        IUnitOfWork unitOfWork = scopeProvider.GetUnitOfWork(true);
        TResult result = func(scopeProvider);
        unitOfWork.Commit();
        return result;
    }

    /// <summary>
    /// 开启一个事务处理
    /// </summary>
    /// <param name="rootProvider">根依赖注入服务提供程序</param>
    /// <param name="funcAsync">要执行的业务委托</param>
    public static async Task BeginUnitOfWorkTransactionAsync(this IServiceProvider rootProvider,
        Func<IServiceProvider, Task> funcAsync)
    {
        Check.NotNull(rootProvider, nameof(rootProvider));
        Check.NotNull(funcAsync, nameof(funcAsync));

        using IServiceScope scope = rootProvider.CreateScope();
        IServiceProvider scopeProvider = scope.ServiceProvider;

        IUnitOfWork unitOfWork = scopeProvider.GetUnitOfWork(true);
        await funcAsync(scopeProvider);
        await unitOfWork.CommitAsync();
    }

    /// <summary>
    /// 开启一个事务处理
    /// </summary>
    /// <param name="rootProvider">根依赖注入服务提供程序</param>
    /// <param name="funcAsync">要执行的业务委托</param>
    public static async Task<TResult> BeginUnitOfWorkTransactionAsync<TResult>(this IServiceProvider rootProvider,
        Func<IServiceProvider, Task<TResult>> funcAsync)
    {
        Check.NotNull(rootProvider, nameof(rootProvider));
        Check.NotNull(funcAsync, nameof(funcAsync));

        using IServiceScope scope = rootProvider.CreateScope();
        IServiceProvider scopeProvider = scope.ServiceProvider;

        IUnitOfWork unitOfWork = scopeProvider.GetUnitOfWork(true);
        TResult result = await funcAsync(scopeProvider);
        await unitOfWork.CommitAsync();
        return result;
    }
    #endregion

}