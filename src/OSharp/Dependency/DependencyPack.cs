// -----------------------------------------------------------------------
//  <copyright file="DependencyPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-29 1:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Collections;
using OSharp.Core.Packs;
using OSharp.Extensions;
using OSharp.Reflection;


namespace OSharp.Dependency
{
    /// <summary>
    /// 依赖注入模块
    /// </summary>
    [Description("依赖注入模块")]
    public class DependencyPack : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Core;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            ServiceLocator.Instance.SetServiceCollection(services);

            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
            services.TryAddSingleton<IHybridServiceScopeFactory, DefaultServiceScopeFactory>();
            services.AddScoped<ScopedDictionary>();

            //查找所有自动注册的服务实现类型进行注册
            Type[] dependencyTypes = FindDependencyTypes();
            foreach (Type dependencyType in dependencyTypes)
            {
                AddToServices(services, dependencyType);
            }

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            ServiceLocator.Instance.SetApplicationServiceProvider(provider);
            IsEnabled = true;
        }

        /// <summary>
        /// 查找所有自动注册的服务实现类型
        /// </summary>
        /// <returns></returns>
        protected virtual Type[] FindDependencyTypes()
        {
            Type[] baseTypes = { typeof(ISingletonDependency), typeof(IScopeDependency), typeof(ITransientDependency) };
            return AssemblyManager.FindTypes(type => type.IsClass && !type.IsAbstract && !type.IsInterface
                && !type.HasAttribute<IgnoreDependencyAttribute>()
                && (baseTypes.Any(b => b.IsAssignableFrom(type)) || type.HasAttribute<DependencyAttribute>()));
        }

        /// <summary>
        /// 将服务实现类型注册到服务集合中
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="implementationType">要注册的服务实现类型</param>
        protected virtual void AddToServices(IServiceCollection services, Type implementationType)
        {
            if (implementationType.IsAbstract || implementationType.IsInterface)
            {
                return;
            }
            ServiceLifetime? lifetime = GetLifetimeOrNull(implementationType);
            if (lifetime == null)
            {
                return;
            }
            DependencyAttribute dependencyAttribute = implementationType.GetAttribute<DependencyAttribute>();
            Type[] serviceTypes = GetImplementedInterfaces(implementationType);

            //服务数量为0时注册自身
            if (serviceTypes.Length == 0)
            {
                services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime.Value));
                return;
            }

            //服务实现显示要求注册身处时，注册自身并且继续注册接口
            if (dependencyAttribute?.AddSelf == true)
            {
                services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime.Value));
            }

            if (serviceTypes.Length > 1)
            {
                List<string> orderTokens = new List<string>() { implementationType.Namespace.Substring("", ".", "") };
                orderTokens.AddIfNotExist("OSharp");
                serviceTypes = serviceTypes.OrderByPrefixes(m => m.FullName, orderTokens.ToArray()).ToArray();
            }

            //注册服务
            for (int i = 0; i < serviceTypes.Length; i++)
            {
                Type serviceType = serviceTypes[i];
                ServiceDescriptor descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime.Value);
                if (lifetime.Value == ServiceLifetime.Transient)
                {
                    services.TryAddEnumerable(descriptor);
                    continue;
                }

                bool multiple = serviceType.HasAttribute<MultipleDependencyAttribute>();
                if (i == 0)
                {
                    if (multiple)
                    {
                        services.Add(descriptor);
                    }
                    else
                    {
                        AddSingleService(services, descriptor, dependencyAttribute);
                    }
                }
                else
                {
                    if (multiple)
                    {
                        services.Add(descriptor);
                    }
                    else
                    {
                        //有多个接口，后边的接口注册使用第一个接口的实例，保证同个实现类的多个接口获得同一实例
                        Type firstServiceType = serviceTypes[0];
                        descriptor = new ServiceDescriptor(serviceType, provider => provider.GetService(firstServiceType), lifetime.Value);
                        AddSingleService(services, descriptor, dependencyAttribute);
                    }
                }
            }
        }

        /// <summary>
        /// 重写以实现 从类型获取要注册的<see cref="ServiceLifetime"/>生命周期类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>生命周期类型</returns>
        protected virtual ServiceLifetime? GetLifetimeOrNull(Type type)
        {
            DependencyAttribute attribute = type.GetAttribute<DependencyAttribute>();
            if (attribute != null)
            {
                return attribute.Lifetime;
            }

            if (type.IsDeriveClassFrom<ITransientDependency>())
            {
                return ServiceLifetime.Transient;
            }

            if (type.IsDeriveClassFrom<IScopeDependency>())
            {
                return ServiceLifetime.Scoped;
            }

            if (type.IsDeriveClassFrom<ISingletonDependency>())
            {
                return ServiceLifetime.Singleton;
            }

            return null;
        }

        /// <summary>
        /// 重写以实现 获取实现类型的所有可注册服务接口
        /// </summary>
        /// <param name="type">依赖注入实现类型</param>
        /// <returns>可注册的服务接口</returns>
        protected virtual Type[] GetImplementedInterfaces(Type type)
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

        private static void AddSingleService(IServiceCollection services,
            ServiceDescriptor descriptor,
            [CanBeNull] DependencyAttribute dependencyAttribute)
        {
            if (dependencyAttribute?.ReplaceExisting == true)
            {
                services.Replace(descriptor);
            }
            else if (dependencyAttribute?.TryAdd == true)
            {
                services.TryAdd(descriptor);
            }
            else
            {
                services.Add(descriptor);
            }
        }
    }
}