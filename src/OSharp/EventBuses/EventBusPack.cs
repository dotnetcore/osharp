// -----------------------------------------------------------------------
//  <copyright file="EventBusPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Core.Packs;
using OSharp.EventBuses.Internal;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 事件总线模块
    /// </summary>
    [Description("事件总线模块")]
    public class EventBusPack : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Core;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<IEventBusBuilder, EventBusBuilder>();
            services.TryAddSingleton<IEventStore, InMemoryEventStore>();
            services.TryAddSingleton<IEventBus, PassThroughEventBus>();

            //向服务容器注册所有事件处理器类型
            //Type[] eventHandlerTypes = AssemblyManager.FindTypesByBase(typeof(IEventHandler<>));
            //foreach (Type handlerType in eventHandlerTypes)
            //{
            //    services.TryAddTransient(handlerType);
            //}

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            IEventBusBuilder builder = provider.GetService<IEventBusBuilder>();
            builder.Build();
            IsEnabled = true;
        }
    }
}