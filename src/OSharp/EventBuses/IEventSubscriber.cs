// -----------------------------------------------------------------------
//  <copyright file="IEventSubscriber.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-01-12 14:14</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 定义事件订阅者
    /// </summary>
    public interface IEventSubscriber
    {
        /// <summary>
        /// 订阅指定事件数据的事件处理类型
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <typeparam name="TEventHandler">事件处理器类型</typeparam>
        void Subscribe<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new();

        /// <summary>
        /// 订阅指定事件数据的事件处理委托
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="action">事件处理委托</param>
        void Subscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// 订阅指定事件数据的事件处理对象
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventHandler">事件处理对象</param>
        void Subscribe<TEventData>(IEventHandler eventHandler) where TEventData : IEventData;

        /// <summary>
        /// 订阅指定事件数据的事件处理对象
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventHandler">事件处理对象</param>
        void Subscribe(Type eventType, IEventHandler eventHandler);

        /// <summary>
        /// 自动订阅所有事件数据及其处理类型
        /// </summary>
        /// <param name="eventHandlerTypes">事件处理器类型集合</param>
        void SubscribeAll(Type[] eventHandlerTypes);

        /// <summary>
        /// 取消订阅指定事件数据的事件处理委托
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="action">事件处理委托</param>
        void Unsubscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// 取消订阅指定事件数据的事件处理对象
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventHandler">事件处理对象</param>
        void Unsubscribe<TEventData>(IEventHandler<TEventData> eventHandler) where TEventData : IEventData;

        /// <summary>
        /// 取消订阅指定事件数据的事件处理对象
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventHandler">事件处理对象</param>
        void Unsubscribe(Type eventType, IEventHandler eventHandler);

        /// <summary>
        /// 取消指定事件数据的所有处理器
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        void UnsubscribeAll<TEventData>() where TEventData : IEventData;

        /// <summary>
        /// 取消指定事件数据的所有处理器
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        void UnsubscribeAll(Type eventType);
    }
}