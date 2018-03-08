// -----------------------------------------------------------------------
//  <copyright file="EventBusModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 19:51</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.EventBuses.Internal;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 事件总线模块
    /// </summary>
    public class EventBusModule : OSharpModule
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IEventSubscriber, PassThroughEventBus>();
            services.AddSingleton<IEventPublisher, PassThroughEventBus>();
            services.AddSingleton<IEventBus, PassThroughEventBus>();
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            services.AddSingleton<IEventBusBuilder, EventBusBuilder>();

            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public override void UseModule(IServiceProvider provider)
        {
            IEventBusBuilder builder = provider.GetService<IEventBusBuilder>();
            builder.Build();
        }
    }
}