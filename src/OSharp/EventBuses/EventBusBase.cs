// -----------------------------------------------------------------------
//  <copyright file="EventBusBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-16 10:19</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Data;
using OSharp.EventBuses.Internal;
using OSharp.Threading;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 事件总线基类
    /// </summary>
    public abstract class EventBusBase : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="EventBusBase"/>类型的新实例
        /// </summary>
        protected EventBusBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            EventStore = serviceProvider.GetService<IEventStore>();
            Logger = serviceProvider.GetLogger(GetType());
        }

        /// <summary>
        /// 获取 事件仓储
        /// </summary>
        protected IEventStore EventStore { get; }

        /// <summary>
        /// 获取 日志对象
        /// </summary>
        protected ILogger Logger { get; }

        #region Implementation of IEventSubscriber

        /// <summary>
        /// 订阅指定事件数据的事件处理类型
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <typeparam name="TEventHandler">事件处理器类型</typeparam>
        public virtual void Subscribe<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new()
        {
            EventStore.Add<TEventData, TEventHandler>();
        }

        /// <summary>
        /// 订阅指定事件数据的事件处理委托
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="action">事件处理委托</param>
        public virtual void Subscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Check.NotNull(action, nameof(action));

            IEventHandler eventHandler = new ActionEventHandler<TEventData>(action);
            Subscribe<TEventData>(eventHandler);
        }

        /// <summary>
        /// 订阅指定事件数据的事件处理对象
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventHandler">事件处理对象</param>
        public virtual void Subscribe<TEventData>(IEventHandler eventHandler) where TEventData : IEventData
        {
            Check.NotNull(eventHandler, nameof(eventHandler));

            Subscribe(typeof(TEventData), eventHandler);
        }

        /// <summary>
        /// 订阅指定事件数据的事件处理对象
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventHandler">事件处理对象</param>
        public virtual void Subscribe(Type eventType, IEventHandler eventHandler)
        {
            Check.NotNull(eventType, nameof(eventType));
            Check.NotNull(eventHandler, nameof(eventHandler));

            EventStore.Add(eventType, eventHandler);
            Logger.LogDebug($"创建事件“{eventType}”到处理器“{eventHandler.GetType()}”的订阅配对");
        }

        /// <summary>
        /// 自动订阅所有事件数据及其处理类型
        /// </summary>
        /// <param name="eventHandlerTypes">事件处理器类型集合</param>
        public virtual void SubscribeAll(Type[] eventHandlerTypes)
        {
            Check.NotNull(eventHandlerTypes, nameof(eventHandlerTypes));

            foreach (Type eventHandlerType in eventHandlerTypes)
            {
                Type handlerInterface = eventHandlerType.GetInterface("IEventHandler`1"); //获取该类实现的泛型接口
                if (handlerInterface == null)
                {
                    continue;
                }

                Type eventDataType = handlerInterface.GetGenericArguments()[0]; //泛型的EventData类型
                IEventHandlerFactory factory = new IocEventHandlerFactory(_serviceProvider, eventHandlerType);
                EventStore.Add(eventDataType, factory);
                Logger.LogDebug($"创建事件“{eventDataType}”到处理器“{eventHandlerType}”的订阅配对");
            }

            Logger.LogInformation($"共从程序集创建了 {eventHandlerTypes.Length} 个事件处理器的事件订阅");
        }

        /// <summary>
        /// 取消订阅指定事件数据的事件处理委托
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="action">事件处理委托</param>
        public virtual void Unsubscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Check.NotNull(action, nameof(action));

            EventStore.Remove(action);
        }

        /// <summary>
        /// 取消订阅指定事件数据的事件处理对象
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventHandler">事件处理对象</param>
        public virtual void Unsubscribe<TEventData>(IEventHandler<TEventData> eventHandler) where TEventData : IEventData
        {
            Check.NotNull(eventHandler, nameof(eventHandler));

            Unsubscribe(typeof(TEventData), eventHandler);
        }

        /// <summary>
        /// 取消订阅指定事件数据的事件处理对象
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventHandler">事件处理对象</param>
        public virtual void Unsubscribe(Type eventType, IEventHandler eventHandler)
        {
            EventStore.Remove(eventType, eventHandler);
            Logger.LogDebug($"移除事件“{eventType}”到处理器“{eventHandler.GetType()}”的订阅配对");
        }

        /// <summary>
        /// 取消指定事件数据的所有处理器
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        public virtual void UnsubscribeAll<TEventData>() where TEventData : IEventData
        {
            UnsubscribeAll(typeof(TEventData));
        }

        /// <summary>
        /// 取消指定事件数据的所有处理器
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        public virtual void UnsubscribeAll(Type eventType)
        {
            EventStore.RemoveAll(eventType);
        }

        #endregion

        #region Implementation of IEventPublisher

        /// <summary>
        /// 同步发布指定事件
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventData">事件数据</param>
        public virtual void Publish<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            Publish<TEventData>(null, eventData);
        }

        /// <summary>
        /// 同步发布指定事件，并指定事件源
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        public virtual void Publish<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            Publish(typeof(TEventData), eventSource, eventData);
        }

        /// <summary>
        /// 同步发布指定事件
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventData">事件数据</param>
        public virtual void Publish(Type eventType, IEventData eventData)
        {
            Publish(eventType, null, eventData);
        }

        /// <summary>
        /// 同步发布指定事件，并指定事件源
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        public virtual void Publish(Type eventType, object eventSource, IEventData eventData)
        {
            eventData.EventSource = eventSource;

            IDictionary<Type, IEventHandlerFactory[]> dict = EventStore.GetHandlers(eventType);
            if (dict.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<Type, IEventHandlerFactory[]> typeItem in dict)
            {
                foreach (IEventHandlerFactory factory in typeItem.Value)
                {
                    InvokeHandler(factory, eventType, eventData);
                }
            }

            Logger.LogDebug($"触发 {eventType} 事件类型，事件源 {eventSource?.GetType()} 的总线事件");
        }

        /// <summary>
        /// 异步发布指定事件
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventData">事件数据</param>
        public virtual Task PublishAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return PublishAsync<TEventData>(null, eventData);
        }

        /// <summary>
        /// 异步发布指定事件，并指定事件源
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        public virtual Task PublishAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            return PublishAsync(typeof(TEventData), eventSource, eventData);
        }

        /// <summary>
        /// 异步发布指定事件
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventData">事件数据</param>
        public virtual Task PublishAsync(Type eventType, IEventData eventData)
        {
            return PublishAsync(eventType, null, eventData);
        }

        /// <summary>
        /// 异步发布指定事件，并指定事件源
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        public virtual async Task PublishAsync(Type eventType, object eventSource, IEventData eventData)
        {
            eventData.EventSource = eventSource;

            IDictionary<Type, IEventHandlerFactory[]> dict = EventStore.GetHandlers(eventType);
            if (dict.Count == 0)
            {
                return;
            }

            foreach (var typeItem in dict)
            {
                foreach (IEventHandlerFactory factory in typeItem.Value)
                {
                    await InvokeHandlerAsync(factory, eventType, eventData);
                }
            }

            Logger.LogDebug($"触发 {eventType} 事件类型，事件源 {eventSource?.GetType()} 的总线事件");
        }

        /// <summary>
        /// 重写以实现触发事件的执行
        /// </summary>
        /// <param name="factory">事件处理器工厂</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件数据</param>
        protected void InvokeHandler(IEventHandlerFactory factory, Type eventType, IEventData eventData)
        {
            EventHandlerDisposeWrapper handlerWrapper = factory.GetHandler();
            IEventHandler handler = handlerWrapper.EventHandler;
            try
            {
                if (handler == null)
                {
                    Logger.LogWarning($"事件源“{eventData.GetType()}”的事件处理器无法找到");
                    return;
                }

                Run(handler, eventType, eventData);
            }
            finally
            {
                handlerWrapper.Dispose();
            }
        }

        /// <summary>
        /// 重写以实现异步触发事件的执行
        /// </summary>
        /// <param name="factory">事件处理器工厂</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件数据</param>
        /// <returns></returns>
        protected virtual async Task InvokeHandlerAsync(IEventHandlerFactory factory, Type eventType, IEventData eventData)
        {
            EventHandlerDisposeWrapper handlerWrapper = factory.GetHandler();
            IEventHandler handler = handlerWrapper.EventHandler;
            try
            {
                if (handler == null)
                {
                    Logger.LogWarning($"事件源“{eventData.GetType()}”的事件处理器无法找到");
                    return;
                }

                await RunAsync(handler, eventType, eventData);
            }
            finally
            {
                handlerWrapper.Dispose();
            }
        }

        private void Run(IEventHandler handler, Type eventType, IEventData eventData)
        {
            try
            {
                handler.Handle(eventData);
            }
            catch (Exception ex)
            {
                string msg = $"执行事件“{eventType.Name}”的处理器“{handler.GetType()}”时引发异常：{ex.Message}";
                Logger.LogError(ex, msg);
                throw;
            }
        }

        private async Task RunAsync(IEventHandler handler, Type eventType, IEventData eventData)
        {
            try
            {
                ICancellationTokenProvider cancellationTokenProvider = _serviceProvider.GetService<ICancellationTokenProvider>();
                await handler.HandleAsync(eventData, cancellationTokenProvider.Token);
            }
            catch (Exception ex)
            {
                string msg = $"执行事件“{eventType.Name}”的处理器“{handler.GetType()}”时引发异常：{ex.Message}";
                Logger.LogError(ex, msg);
                throw;
            }
        }

        #endregion
    }
}