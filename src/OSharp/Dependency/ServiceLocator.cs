// -----------------------------------------------------------------------
//  <copyright file="ServiceLocator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-09 21:57</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Data;


namespace OSharp.Dependency
{
    /// <summary>
    /// 应用程序服务定位器，可随时正常解析<see cref="ServiceLifetime.Singleton"/>与<see cref="ServiceLifetime.Transient"/>生命周期类型的服务
    /// 如果当前处于HttpContext有效的范围内，可正常解析<see cref="ServiceLifetime.Scoped"/>的服务
    /// 注：服务定位器尚不能正常解析 RootServiceProvider.CreateScope() 生命周期内的 Scoped 的服务
    /// </summary>
    public class ServiceLocator : Disposable
    {
        private static readonly Lazy<ServiceLocator> InstanceLazy = new Lazy<ServiceLocator>(() => new ServiceLocator());
        private IServiceProvider _provider;

        private IServiceCollection _services;

        /// <summary>
        /// 初始化一个<see cref="ServiceLocator"/>类型的新实例
        /// </summary>
        private ServiceLocator()
        { }

        /// <summary>
        /// 获取 服务器定位器实例
        /// </summary>
        public static ServiceLocator Instance => InstanceLazy.Value;

        /// <summary>
        /// 获取 ServiceProvider是否为可用
        /// </summary>
        public bool IsProviderEnabled => _provider != null;

        /// <summary>
        /// 获取 <see cref="ServiceLifetime.Scoped"/>生命周期的服务提供者
        /// </summary>
        public IServiceProvider ScopedProvider
        {
            get
            {
                IScopedServiceResolver scopedResolver = _provider.GetService<IScopedServiceResolver>();
                return scopedResolver != null && scopedResolver.ResolveEnabled
                    ? scopedResolver.ScopedProvider
                    : null;
            }
        }

        /// <summary>
        /// 获取当前是否处于<see cref="ServiceLifetime.Scoped"/>生命周期中
        /// </summary>
        /// <returns></returns>
        public static bool InScoped()
        {
            return Instance.ScopedProvider != null;
        }

        /// <summary>
        /// 设置应用程序服务集合
        /// </summary>
        internal void SetServiceCollection(IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));
            _services = services;
        }

        /// <summary>
        /// 设置应用程序服务提供者
        /// </summary>
        internal void SetApplicationServiceProvider(IServiceProvider provider)
        {
            Check.NotNull(provider, nameof(provider));
            _provider = provider;
        }

        /// <summary>
        /// 获取所有已注册的<see cref="ServiceDescriptor"/>对象
        /// </summary>
        public IEnumerable<ServiceDescriptor> GetServiceDescriptors()
        {
            Check.NotNull(_services, nameof(_services));
            return _services;
        }

        /// <summary>
        /// 解析指定类型的服务实例
        /// </summary>
        public T GetService<T>()
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            IScopedServiceResolver scopedResolver = _provider.GetService<IScopedServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
            {
                return scopedResolver.GetService<T>();
            }
            return _provider.GetService<T>();
        }

        /// <summary>
        /// 解析指定类型的服务实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public object GetService(Type serviceType)
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            IScopedServiceResolver scopedResolver = _provider.GetService<IScopedServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
            {
                return scopedResolver.GetService(serviceType);
            }
            return _provider.GetService(serviceType);
        }

        /// <summary>
        /// 解析指定类型的所有服务实例
        /// </summary>
        public IEnumerable<T> GetServices<T>()
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            IScopedServiceResolver scopedResolver = _provider.GetService<IScopedServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
            {
                return scopedResolver.GetServices<T>();
            }
            return _provider.GetServices<T>();
        }

        /// <summary>
        /// 解析指定类型的所有服务实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            IScopedServiceResolver scopedResolver = _provider.GetService<IScopedServiceResolver>();
            if (scopedResolver != null && scopedResolver.ResolveEnabled)
            {
                return scopedResolver.GetServices(serviceType);
            }
            return _provider.GetServices(serviceType);
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <typeparam name="T">非静态强类型</typeparam>
        /// <returns>日志对象</returns>
        public ILogger<T> GetLogger<T>()
        {
            ILoggerFactory factory = GetService<ILoggerFactory>();
            return factory.CreateLogger<T>();
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns>日志对象</returns>
        public ILogger GetLogger(Type type)
        {
            ILoggerFactory factory = GetService<ILoggerFactory>();
            return factory.CreateLogger(type);
        }

        /// <summary>
        /// 获取指定名称的日志对象
        /// </summary>
        public ILogger GetLogger(string name)
        {
            ILoggerFactory factory = GetService<ILoggerFactory>();
            return factory.CreateLogger(name);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        public ClaimsPrincipal GetCurrentUser()
        {
            try
            {
                IPrincipal user = GetService<IPrincipal>();
                return user as ClaimsPrincipal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定节点的选项值
        /// </summary>
        public string GetConfiguration(string path)
        {
            IConfiguration config = GetService<IConfiguration>() ?? _services.GetConfiguration();
            return config?[path];
        }

        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑
        /// 1.当前处理<see cref="ServiceLifetime.Scoped"/>生命周期外，使用CreateScope创建<see cref="ServiceLifetime.Scoped"/>
        /// 生命周期的ServiceProvider来执行，并释放资源
        /// 2.当前处于<see cref="ServiceLifetime.Scoped"/>生命周期内，直接使用<see cref="ServiceLifetime.Scoped"/>的ServiceProvider来执行
        /// </summary>
        public void ExecuteScopedWork(Action<IServiceProvider> action, bool useHttpScope = true)
        {
            using (IServiceScope scope = useHttpScope
                ? _provider.GetService<IHybridServiceScopeFactory>().CreateScope()
                : _provider.CreateScope())
            {
                action(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 异步执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑
        /// 1.当前处理<see cref="ServiceLifetime.Scoped"/>生命周期外，使用CreateScope创建<see cref="ServiceLifetime.Scoped"/>
        /// 生命周期的ServiceProvider来执行，并释放资源
        /// 2.当前处于<see cref="ServiceLifetime.Scoped"/>生命周期内，直接使用<see cref="ServiceLifetime.Scoped"/>的ServiceProvider来执行
        /// </summary>
        public async Task ExecuteScopedWorkAsync(Func<IServiceProvider, Task> action, bool useHttpScope = true)
        {
            using (IServiceScope scope = useHttpScope
                ? _provider.GetService<IHybridServiceScopeFactory>().CreateScope()
                : _provider.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑，并获取返回值
        /// 1.当前处理<see cref="ServiceLifetime.Scoped"/>生命周期外，使用CreateScope创建<see cref="ServiceLifetime.Scoped"/>
        /// 生命周期的ServiceProvider来执行，并释放资源
        /// 2.当前处于<see cref="ServiceLifetime.Scoped"/>生命周期内，直接使用<see cref="ServiceLifetime.Scoped"/>的ServiceProvider来执行
        /// </summary>
        public TResult ExecuteScopedWork<TResult>(Func<IServiceProvider, TResult> func, bool useHttpScope = true)
        {
            using (IServiceScope scope = useHttpScope
                ? _provider.GetService<IHybridServiceScopeFactory>().CreateScope()
                : _provider.CreateScope())
            {
                return func(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑，并获取返回值
        /// 1.当前处理<see cref="ServiceLifetime.Scoped"/>生命周期外，使用CreateScope创建<see cref="ServiceLifetime.Scoped"/>
        /// 生命周期的ServiceProvider来执行，并释放资源
        /// 2.当前处于<see cref="ServiceLifetime.Scoped"/>生命周期内，直接使用<see cref="ServiceLifetime.Scoped"/>的ServiceProvider来执行
        /// </summary>
        public async Task<TResult> ExecuteScopedWorkAsync<TResult>(Func<IServiceProvider, Task<TResult>> func, bool useHttpScope = true)
        {
            using (IServiceScope scope = useHttpScope
                ? _provider.GetService<IHybridServiceScopeFactory>().CreateScope()
                : _provider.CreateScope())
            {
                return await func(scope.ServiceProvider);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _services = null;
            _provider = null;
            base.Dispose(disposing);
        }
    }
}