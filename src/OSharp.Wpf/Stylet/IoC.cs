// -----------------------------------------------------------------------
//  <copyright file="IoC.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 1:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using StyletIoC;


namespace OSharp.Wpf.Stylet
{
    /// <summary>
    /// 依赖注入ServiceLocator模式
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// 获取指定服务类型的单个实例
        /// </summary>
        public static Func<Type, string, object> GetInstance = (service, key) => throw new InvalidOperationException("IoC is not initialized");

        /// <summary>
        /// 获取指定服务类型的多个实例
        /// </summary>
        public static Func<Type, IEnumerable<object>> GetAllInstances = service => throw new InvalidOperationException("IoC is not initialized");

        public static Action<object> BuildUp = instance => throw new InvalidOperationException("IoC is not initialized");

        /// <summary>
        /// 获取指定服务类型的单个实例
        /// </summary>
        /// <returns></returns>
        public static T Get<T>(string key = null)
        {
            return (T)GetInstance(typeof(T), key);
        }

        /// <summary>
        /// 获取指定服务类型的多个实例
        /// </summary>
        public static IEnumerable<T> GetAll<T>()
        {
            return GetAllInstances(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// 初始化 stylet 的内置IoC
        /// </summary>
        /// <param name="container">依赖注入容器</param>
        public static void Initialize(IContainer container)
        {
            GetInstance = container.Get;
            GetAllInstances = type => container.GetAll(type);
            BuildUp = container.BuildUp;
        }

        /// <summary>
        /// 初始化 .net core 的IoC
        /// </summary>
        public static void Initialize(IServiceProvider provider)
        {
            GetInstance = (type, key) => provider.GetService(type);
            GetAllInstances = provider.GetServices;
        }
    }
}