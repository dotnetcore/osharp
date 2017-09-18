// -----------------------------------------------------------------------
//  <copyright file="AppServiceLocator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-17 13:05</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Exceptions;


namespace OSharp
{
    /// <summary>
    /// 应用程序服务定位器，仅适合于<see cref="ServiceLifetime.Singleton"/>与<see cref="ServiceLifetime.Transient"/>生命周期类型的服务
    /// </summary>
    public sealed class ServiceLocator
    {
        private static readonly Lazy<ServiceLocator> InstanceLazy = new Lazy<ServiceLocator>(() => new ServiceLocator());

        private IServiceCollection _services;
        private IServiceProvider _provider;

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
        /// 设置应用程序服务集合
        /// </summary>
        internal void TrySetServiceCollection(IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));
            if (_services == null)
            {
                _services = services;
            }
        }

        /// <summary>
        /// 设置应用程序服务提供者
        /// </summary>
        public void TrySetApplicationServiceProvider(IServiceProvider provider)
        {
            Check.NotNull(provider, nameof(provider));
            if (_provider == null)
            {
                _provider = provider;
            }
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
        /// 解析指定类型的服务实例，仅适用于<see cref="ServiceLifetime.Singleton"/>与<see cref="ServiceLifetime.Transient"/>生命周期类型的服务
        /// </summary>
        public T GetService<T>()
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            ServiceDescriptor descriptor = _services.FirstOrDefault(m => m.ServiceType == typeof(T));
            if (descriptor == null)
            {
                return default(T);
            }
            if (descriptor.Lifetime == ServiceLifetime.Scoped)
            {
                throw new OsharpException($"不能从 root ServiceProvider 中解析 Scoped 类型的实例");
            }
            return _provider.GetService<T>();
        }

        /// <summary>
        /// 解析指定类型的服务实例，仅适用于<see cref="ServiceLifetime.Singleton"/>与<see cref="ServiceLifetime.Transient"/>生命周期类型的服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public object GetService(Type serviceType)
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            ServiceDescriptor descriptor = _services.FirstOrDefault(m => m.ServiceType == serviceType);
            if (descriptor == null)
            {
                return null;
            }
            if (descriptor.Lifetime == ServiceLifetime.Scoped)
            {
                throw new OsharpException($"不能从 root ServiceProvider 中解析 Scoped 类型的实例");
            }
            return _provider.GetService(serviceType);
        }

        /// <summary>
        /// 解析指定类型的所有服务实例，仅适用于<see cref="ServiceLifetime.Singleton"/>与<see cref="ServiceLifetime.Transient"/>生命周期类型的服务
        /// </summary>
        public IEnumerable<T> GetServices<T>()
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            ServiceDescriptor descriptor = _services.FirstOrDefault(m => m.ServiceType == typeof(T));
            if (descriptor == null)
            {
                throw new OsharpException($"不能从 root ServiceProvider 中解析 Scoped 类型的实例");
            }
            return _provider.GetServices<T>();
        }

        /// <summary>
        /// 解析指定类型的所有服务实例，仅适用于<see cref="ServiceLifetime.Singleton"/>与<see cref="ServiceLifetime.Transient"/>生命周期类型的服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            Check.NotNull(_services, nameof(_services));
            Check.NotNull(_provider, nameof(_provider));

            ServiceDescriptor descriptor = _services.FirstOrDefault(m => m.ServiceType == serviceType);
            if (descriptor == null)
            {
                throw new OsharpException($"不能从 root ServiceProvider 中解析 Scoped 类型的实例");
            }
            return _provider.GetServices(serviceType);
        }
    }
}