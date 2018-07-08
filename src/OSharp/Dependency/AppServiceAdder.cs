// -----------------------------------------------------------------------
//  <copyright file="AppServiceAdder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 22:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Core.Options;
using OSharp.Reflection;

namespace OSharp.Dependency
{
    /// <summary>
    /// 应用程序服务添加者
    /// </summary>
    public class AppServiceAdder : IAppServiceAdder
    {
        private readonly AppServiceScanOptions _options;

        /// <summary>
        /// 初始化一个<see cref="AppServiceAdder"/>类型的新实例
        /// </summary>
        public AppServiceAdder()
            : this(new AppServiceScanOptions())
        { }

        /// <summary>
        /// 初始化一个<see cref="AppServiceAdder"/>类型的新实例
        /// </summary>
        public AppServiceAdder(AppServiceScanOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public IServiceCollection AddServices(IServiceCollection services)
        {
            //添加即时生命周期类型的服务
            Type[] dependencyTypes = _options.TransientTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, ServiceLifetime.Transient);

            //添加作用域生命周期类型的服务
            dependencyTypes = _options.ScopedTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, ServiceLifetime.Scoped);

            //添加单例生命周期类型的服务
            dependencyTypes = _options.SingletonTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, ServiceLifetime.Singleton);

            return services;
        }

        /// <summary>
        /// 以类型实现的接口进行服务添加，需排除
        /// <see cref="IDisposable"/>等非业务接口，如无接口则注册自身
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        /// <param name="implementationTypes">要注册的实现类型集合</param>
        /// <param name="lifetime">注册的生命周期类型</param>
        protected virtual IServiceCollection AddTypeWithInterfaces(IServiceCollection services, Type[] implementationTypes, ServiceLifetime lifetime)
        {
            foreach (Type implementationType in implementationTypes)
            {
                if (implementationType.IsAbstract || implementationType.IsInterface)
                {
                    continue;
                }
                Type[] interfaceTypes = GetImplementedInterfaces(implementationType);
                if (interfaceTypes.Length == 0)
                {
                    services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime));
                    continue;
                }
                for (int i = 0; i < interfaceTypes.Length; i++)
                {
                    Type interfaceType = interfaceTypes[i];
                    if (lifetime == ServiceLifetime.Transient)
                    {
                        services.TryAddEnumerable(new ServiceDescriptor(interfaceType, implementationType, lifetime));
                        continue;
                    }
                    bool multiple = interfaceType.HasAttribute<MultipleDependencyAttribute>();
                    if (i == 0)
                    {
                        if (multiple)
                        {
                            services.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));
                        }
                        else
                        {
                            services.TryAdd(new ServiceDescriptor(interfaceType, implementationType, lifetime));
                        }
                    }
                    else
                    {
                        //有多个接口时，后边的接口注册使用第一个接口的实例，保证同个实现类的多个接口获得同一个实例
                        Type firstInterfaceType = interfaceTypes[0];
                        if (multiple)
                        {
                            services.Add(new ServiceDescriptor(interfaceType, provider => provider.GetService(firstInterfaceType), lifetime));
                        }
                        else
                        {
                            services.TryAdd(new ServiceDescriptor(interfaceType, provider => provider.GetService(firstInterfaceType), lifetime));
                        }
                    }
                }
            }
            return services;
        }

        private static Type[] GetImplementedInterfaces(Type type)
        {
            Type[] exceptInterfaces = { typeof(IDisposable) };
            Type[] interfaceTypes = type.GetInterfaces().Where(t => !exceptInterfaces.Contains(t) && !t.HasAttribute<IgnoreDependencyAttribute>()).ToArray();
            for (int index = 0; index < interfaceTypes.Length; index++)
            {
                Type interfaceType = interfaceTypes[index];
                if (interfaceType.IsGenericType && !interfaceType.IsGenericTypeDefinition && interfaceType.FullName == null)
                {
                    interfaceTypes[index] = interfaceType.GetGenericTypeDefinition();
                }
            }
            return interfaceTypes;
        }
    }
}