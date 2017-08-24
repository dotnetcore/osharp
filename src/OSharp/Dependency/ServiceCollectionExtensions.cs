// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 23:14</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;


namespace OSharp
{
    /// <summary>
    /// ServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static readonly bool _added = false;

        /// <summary>
        /// 将应用程序服务添加到<see cref="IServiceCollection"/> 
        /// 检索程序集，查找实现了<see cref="ITransientDependency"/>，<see cref="IScopeDependency"/>，<see cref="ISingletonDependency"/> 接口的所有服务，分别按生命周期类型进行添加
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services, AppServiceAdderOptions options = null)
        {
            if (_added)
            {
                throw new InvalidOperationException("services.AddAppServices 扩展方法只能调用1次，不能多次调用。");
            }
            if (options == null)
            {
                options = new AppServiceAdderOptions();
            }
            return new AppServiceAdder(options).AddServices(services);
        }

        /// <summary>
        /// 将应用程序服务添加到<see cref="IServiceCollection"/> 
        /// 检索程序集，查找实现了<see cref="ITransientDependency"/>，<see cref="IScopeDependency"/>，<see cref="ISingletonDependency"/> 接口的所有服务，分别按生命周期类型进行添加
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services, IAppServiceAdder adder)
        {
            if (_added)
            {
                throw new InvalidOperationException("services.AddAppServices 扩展方法只能调用1次，不能多次调用。");
            }
            if (adder == null)
            {
                adder = new AppServiceAdder();
            }
            return adder.AddServices(services);
        }
    }
}