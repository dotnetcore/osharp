// -----------------------------------------------------------------------
//  <copyright file="ServiceExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-28 15:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Windows;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Reflection;
using OSharp.Wpf.FluentValidation;

using Stylet;


namespace OSharp.Wpf.Stylet
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 注册所有<see cref="IValidator"/>类型的实现类
        /// </summary>
        public static IServiceCollection AddValidators(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.IsNullOrEmpty())
            {
                assemblies = new[] { Assembly.GetExecutingAssembly() };
            }

            Type baseType = typeof(IValidator<>);
            Type[] types = assemblies.SelectMany(m => m.GetTypes()).Where(m => !m.IsAbstract && m.IsBaseOn(baseType)).Distinct().ToArray();
            foreach (Type type in types)
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault();
                if (interfaceType == null)
                {
                    continue;
                }

                services.AddTransient(interfaceType, type);
            }

            services.AddTransient(typeof(IModelValidator<>), typeof(FluentModelValidator<>));

            return services;
        }

        /// <summary>
        /// 注册所有<see cref="Screen"/>实现类视图模型
        /// </summary>
        public static IServiceCollection AddViewModels(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.IsNullOrEmpty())
            {
                assemblies = new[] { Assembly.GetExecutingAssembly() };
            }

            Type baseType = typeof(Screen);
            Type[] types = assemblies.SelectMany(m => m.GetTypes()).Where(m => !m.IsAbstract && m.IsBaseOn(baseType)).Distinct().ToArray();

            Type[] singletonTypes = types.Where(m => m.HasAttribute<SingletonAttribute>()).ToArray();
            foreach (Type type in singletonTypes)
            {
                services.AddSingleton(type);
            }

            types = types.Except(singletonTypes).ToArray();
            foreach (Type type in types)
            {
                services.AddTransient(type);
            }

            return services;
        }

        /// <summary>
        /// 注册所有<see cref="UIElement"/>实现的视图类
        /// </summary>
        public static IServiceCollection AddViews(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.IsNullOrEmpty())
            {
                assemblies = new[] { Assembly.GetExecutingAssembly() };
            }

            Type baseType = typeof(UIElement);
            Type[] types = assemblies.SelectMany(m => m.GetTypes()).Where(m => !m.IsAbstract && m.IsBaseOn(baseType)).Distinct().ToArray();
            foreach (Type type in types)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}