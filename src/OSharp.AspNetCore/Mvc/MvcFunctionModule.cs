// -----------------------------------------------------------------------
//  <copyright file="MvcFunctionModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-09 21:46</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Modules;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// MVC功能点模块
    /// </summary>
    public class MvcFunctionModule : OSharpModule
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override ModuleLevel Level => ModuleLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IMvcFunctionHandler, MvcFunctionHandler>();
            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public override void UseModule(IServiceProvider provider)
        {
            IMvcFunctionHandler handler = provider.GetService<IMvcFunctionHandler>();
            handler.Initialize();
            IsEnabled = true;
        }
    }
}