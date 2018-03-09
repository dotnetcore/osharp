// -----------------------------------------------------------------------
//  <copyright file="EntityInfoModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-09 21:30</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Modules;
using OSharp.Entity;


namespace OSharp.Core.EntityInfos
{
    /// <summary>
    /// 实体信息模块
    /// </summary>
    public class EntityInfoModule : OSharpModule
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IEntityTypeFinder, EntityTypeFinder>();
            services.AddSingleton<IEntityInfoHandler, EntityInfoHandler>();

            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public override void UseModule(IServiceProvider provider)
        {
            IEntityInfoHandler handler = provider.GetService<IEntityInfoHandler>();
            handler.Initialize();
            IsEnabled = true;
        }
    }
}