// -----------------------------------------------------------------------
//  <copyright file="EventBusServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-18 21:48</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

using OSharp.EventBuses;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// EventBus服务注册扩展方法
    /// </summary>
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 将EventBus服务注册到<see cref="IServiceCollection"/>
        /// </summary>
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus>(provider =>
            {
                if (EventBus.Default.Logger == null)
                {
                    ILogger<EventBus> logger = provider.GetService<ILogger<EventBus>>();
                    EventBus.Default.Logger = logger;
                }
                return EventBus.Default;
            });

            return services;
        }
    }
}