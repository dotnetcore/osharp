// -----------------------------------------------------------------------
//  <copyright file="AspNetCoreModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-09 21:52</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Infrastructure;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Core.Modules;
using OSharp.Dependency;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// AspNetCore模块
    /// </summary>
    public class AspNetCoreModule : OSharpModule
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override ModuleLevel Level => ModuleLevel.Core;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IScopedServiceResolver, RequestScopedServiceResolver>();
            services.AddScoped<UnitOfWorkAttribute>();

            return services;
        }
    }
}