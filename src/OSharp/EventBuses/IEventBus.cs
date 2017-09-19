// -----------------------------------------------------------------------
//  <copyright file="IEventBus.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-18 12:48</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 定义事件总线
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 将指定事件源数据与相应处理器注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <typeparam name="TEventHandler">事件处理器类型</typeparam>
        void Register<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new();

        /// <summary>
        /// 将指定事件源数据与相应处理器委托实现注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="action">处理器委托实现</param>
        void Register<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// 将指定事件源数据与相应处理器注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventHandler">事件处理器实例</param>
        void Register<TEventData>(IEventHandler eventHandler) where TEventData : IEventData;

        /// <summary>
        /// 将指定事件源数据与相应处理器注册到事件总线
        /// </summary>
        /// <param name="eventType">事件源数据类型</param>
        /// <param name="eventHandler">事件处理器实例</param>
        void Register(Type eventType, IEventHandler eventHandler);
        
        /// <summary>
        /// 遍历程序集类型，自动注册事件处理器及所属事件源数据到事件总线
        /// </summary>
        /// <param name="assembly">程序集</param>
        void RegisterAllEventHandlers(Assembly assembly);

        /// <summary>
        /// 注销指定事件源数据的处理器委托实现
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="action">处理器委托实现</param>
        void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// 注销指定事件源数据的处理器委托实例
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="handler">处理器委托实例</param>
        void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        /// 注销指定事件源数据的处理器委托实例
        /// </summary>
        /// <param name="eventType">事件源数据类型</param>
        /// <param name="handler">处理器委托实例</param>
        void Unregister(Type eventType, IEventHandler handler);

        /// <summary>
        /// 注销指定事件源数据的所有处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        void UnregisterAll<TEventData>() where TEventData : IEventData;

        /// <summary>
        /// 注销指定事件源数据的所有处理器
        /// </summary>
        /// <param name="eventType">事件源数据类型</param>
        void UnregisterAll(Type eventType);

        #region Trigger

        /// <summary>
        /// 触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventData">事件源数据</param>
        void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// 触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// 触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件源数据</param>
        void Trigger(Type eventType, IEventData eventData);

        /// <summary>
        /// 触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        void Trigger(Type eventType, object eventSource, IEventData eventData);

        /// <summary>
        /// 异步触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventData">事件源数据</param>
        /// <returns></returns>
        Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// 异步触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// 异步触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件源数据</param>
        Task TriggerAsync(Type eventType, IEventData eventData);

        /// <summary>
        /// 异步触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        Task TriggerAsync(Type eventType, object eventSource, IEventData eventData);


        #endregion
    }
}