// -----------------------------------------------------------------------
//  <copyright file="DependencyPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-29 1:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Core.Packs;
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
        /// 初始化一个<see cref="DependencyPack"/>类型的新实例
        /// </summary>
        public DependencyPack()
        {
            ScanOptions = new ServiceScanOptions();
        }

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
        /// 获取或设置 服务搜索选项
        /// </summary>
        protected virtual ServiceScanOptions ScanOptions { get; }

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            ServiceLocator.Instance.SetServiceCollection(services);

            services.AddScoped<ScopedDictionary>();
            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));

            //添加即时生命周期类型的服务
            Type[] dependencyTypes = ScanOptions.TransientTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, ServiceLifetime.Transient);

            //添加作用域生命周期类型的服务
            dependencyTypes = ScanOptions.ScopedTypeFinder.FindAll();
            AddTypeWithInterfaces(services, dependencyTypes, ServiceLifetime.Scoped);

            //添加单例生命周期类型的服务
            dependencyTypes = ScanOptions.SingletonTypeFinder.FindAll();
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

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            ServiceLocator.Instance.SetApplicationServiceProvider(provider);
            IsEnabled = true;
        }
    }
}