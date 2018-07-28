// -----------------------------------------------------------------------
//  <copyright file="IEventStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-22 1:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 定义事件订阅存储
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// 将事件源类型与事件处理类型添加到存储，这里使用的是类型，应当使用即时的处理器工厂来存储
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <typeparam name="TEventHandler">数据处理器类型</typeparam>
        void Add<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new();
        
        /// <summary>
        /// 将事件源类型与事件处理器实例添加到存储，这里使用的是处理器实例，应当使用单例的处理器工厂来存储
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        /// <param name="eventHandler">事件处理器实例</param>
        void Add(Type eventType, IEventHandler eventHandler);

        /// <summary>
        /// 将事件源与事件处理器工厂添加到存储
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        /// <param name="factory">事件处理器工厂</param>
        void Add(Type eventType, IEventHandlerFactory factory);

        /// <summary>
        /// 移除指定事件源的处理委托实现
        /// </summary>
        /// <typeparam name="TEventData">事件源类型</typeparam>
        /// <param name="action">事件处理委托</param>
        void Remove<TEventData>(Action<TEventData> action) where TEventData : IEventData;
        
        /// <summary>
        /// 移除指定事件源与处理器实例
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        /// <param name="eventHandler">处理器实例</param>
        void Remove(Type eventType, IEventHandler eventHandler);
        
        /// <summary>
        /// 移除指定事件源的所有处理器
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        void RemoveAll(Type eventType);
        
        /// <summary>
        /// 获取指定事件类型的所有事件处理器工厂
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <returns>指定事件类型及其派生事件类型的处理器集合的字典</returns>
        IDictionary<Type, IEventHandlerFactory[]> GetHandlers(Type eventType);
    }
}