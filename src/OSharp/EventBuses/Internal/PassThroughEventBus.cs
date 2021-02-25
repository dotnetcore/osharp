// -----------------------------------------------------------------------
//  <copyright file="PassThroughEventBus.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-01-12 15:31</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.EventBuses.Internal
{
    /// <summary>
    /// 一个事件总线，当有消息被派发到消息总线时，消息总线将不做任何处理与路由，而是直接将消息推送到订阅方
    /// </summary>
    internal class PassThroughEventBus : EventBusBase
    {
        /// <summary>
        /// 初始化一个<see cref="PassThroughEventBus"/>类型的新实例
        /// </summary>
        public PassThroughEventBus(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }
}