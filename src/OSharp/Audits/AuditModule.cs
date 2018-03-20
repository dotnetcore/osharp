// -----------------------------------------------------------------------
//  <copyright file="AuditModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 19:44</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.Core.Modules;
using OSharp.EventBuses;


namespace OSharp.Audits
{
    /// <summary>
    /// 审计模块
    /// </summary>
    [DependsOnModules(typeof(EventBusModule))]
    public class AuditModule : OSharpModule
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
            services.AddTransient<AuditEntityStoreEventHandler>();
            services.AddSingleton<IAuditStore, NullAuditStore>();

            return services;
        }
    }
}