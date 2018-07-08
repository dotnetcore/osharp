// -----------------------------------------------------------------------
//  <copyright file="EventBusBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-01 17:55</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using OSharp.Data;
using OSharp.EventBuses.Internal;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Reflection;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 事件总线基类
    /// </summary>
    public abstract class EventBusBase : IEventBus
    {
        /// <summary>
        /// 事件仓储
        /// </summary>
        protected readonly IEventStore _EventStore;
        /// <summary>
        /// 日志对象
        /// </summary>
        protected readonly ILogger _Logger;

        /// <summary>
        /// 初始化一个<see cref="EventBusBase"/>类型的新实例
        /// </summary>
        protected EventBusBase(IEventStore eventStore, ILogger logger)
        {
            _EventStore = eventStore;
            _Logger = logger;
        }

        #region Implementation of IEventSubscriber

        /// <summary>
        /// 订阅指定事件数据的事件处理类型
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <typeparam name="TEventHandler">事件处理器类型</typeparam>
        public virtual void Subscribe<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new()
        {
            _EventStore.Add<TEventData, TEventHandler>();
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

            _EventStore.Add(eventType, eventHandler);
        }

        /// <summary>
        /// 遍历程序集类型，自动订阅所有事件数据及其处理类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        public virtual void SubscribeAll(Assembly assembly)
        {
            assembly.CheckNotNull("assembly");

            Type[] handlerTypes = assembly.GetTypes().Where(type => type.IsDeriveClassFrom(typeof(IEventHandler<>))).ToArray();
            if (handlerTypes.Length == 0)
            {
                return;
            }
            foreach (Type handlerType in handlerTypes)
            {
                Type handlerInterface = handlerType.GetInterface("IEventHandler`1"); //获取该类实现的泛型接口
                if (handlerInterface == null)
                {
                    continue;
                }
                Type eventType = handlerInterface.GetGenericArguments()[0]; //泛型的EventData类型
                IEventHandlerFactory factory = new IocEventHandlerFactory(handlerType);
                _EventStore.Add(eventType, factory);
                _Logger.LogDebug($"创建事件“{eventType}”到处理器“{handlerType}”的订阅配对");
            }
            _Logger.LogInformation($"程序集“{assembly.GetName().Name}”创建了{handlerTypes.Length}个处理器的事件订阅");
        }

        /// <summary>
        /// 取消订阅指定事件数据的事件处理委托
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="action">事件处理委托</param>
        public virtual void Unsubscribe<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Check.NotNull(action, nameof(action));

            _EventStore.Remove(action);
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
            _EventStore.Remove(eventType, eventHandler);
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
            _EventStore.RemoveAll(eventType);
        }

        #endregion

        #region Implementation of IEventPublisher

        /// <summary>
        /// 同步发布指定事件
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual void Publish<TEventData>(TEventData eventData, bool wait = true) where TEventData : IEventData
        {
            Publish<TEventData>(null, eventData, wait);
        }

        /// <summary>
        /// 同步发布指定事件，并指定事件源
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual void Publish<TEventData>(object eventSource, TEventData eventData, bool wait = true) where TEventData : IEventData
        {
            Publish(typeof(TEventData), eventSource, eventData, wait);
        }

        /// <summary>
        /// 同步发布指定事件
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual void Publish(Type eventType, IEventData eventData, bool wait = true)
        {
            Publish(eventType, null, eventData, wait);
        }

        /// <summary>
        /// 同步发布指定事件，并指定事件源
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual void Publish(Type eventType, object eventSource, IEventData eventData, bool wait = true)
        {
            eventData.EventSource = eventSource;

            IDictionary<Type, IEventHandlerFactory[]> dict = _EventStore.GetHandlers(eventType);
            foreach (var typeItem in dict)
            {
                foreach (IEventHandlerFactory factory in typeItem.Value)
                {
                    InvokeHandler(factory, eventType, eventData, wait);
                }
            }
        }

        /// <summary>
        /// 异步发布指定事件
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual Task PublishAsync<TEventData>(TEventData eventData, bool wait = true) where TEventData : IEventData
        {
            return PublishAsync<TEventData>(null, eventData, wait);
        }

        /// <summary>
        /// 异步发布指定事件，并指定事件源
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual Task PublishAsync<TEventData>(object eventSource, TEventData eventData, bool wait = true) where TEventData : IEventData
        {
            return PublishAsync(typeof(TEventData), eventSource, eventData, wait);
        }

        /// <summary>
        /// 异步发布指定事件
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual Task PublishAsync(Type eventType, IEventData eventData, bool wait = true)
        {
            return PublishAsync(eventType, null, eventData, wait);
        }

        /// <summary>
        /// 异步发布指定事件，并指定事件源
        /// </summary>
        /// <param name="eventType">事件数据类型</param>
        /// <param name="eventSource">事件源，触发事件的对象</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        public virtual async Task PublishAsync(Type eventType, object eventSource, IEventData eventData, bool wait = true)
        {
            eventData.EventSource = eventSource;

            IDictionary<Type, IEventHandlerFactory[]> dict = _EventStore.GetHandlers(eventType);
            foreach (var typeItem in dict)
            {
                foreach (IEventHandlerFactory factory in typeItem.Value)
                {
                    await InvokeHandlerAsync(factory, eventType, eventData, wait);
                }
            }
        }

        /// <summary>
        /// 重写以实现触发事件的执行
        /// </summary>
        /// <param name="factory">事件处理器工厂</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        protected void InvokeHandler(IEventHandlerFactory factory, Type eventType, IEventData eventData, bool wait = true)
        {
            IEventHandler handler = factory.GetHandler();
            if (handler == null)
            {
                _Logger.LogWarning($"事件源“{eventData.GetType()}”的事件处理器无法找到");
                return;
            }
            if (!handler.CanHandle(eventData))
            {
                return;
            }
            if (wait)
            {
                Run(factory, handler, eventType, eventData);
            }
            else
            {
                Task.Run(() =>
                {
                    Run(factory, handler, eventType, eventData);
                });
            }
        }

        /// <summary>
        /// 重写以实现异步触发事件的执行
        /// </summary>
        /// <param name="factory">事件处理器工厂</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="wait">是否等待结果返回</param>
        /// <returns></returns>
        protected virtual Task InvokeHandlerAsync(IEventHandlerFactory factory, Type eventType, IEventData eventData, bool wait = true)
        {
            IEventHandler handler = factory.GetHandler();
            if (handler == null)
            {
                _Logger.LogWarning($"事件源“{eventData.GetType()}”的事件处理器无法找到");
                return Task.FromResult(0);
            }
            if (!handler.CanHandle(eventData))
            {
                return Task.FromResult(0);
            }
            if (wait)
            {
                return RunAsync(factory, handler, eventType, eventData);
            }
            Task.Run(async () =>
            {
                await RunAsync(factory, handler, eventType, eventData);
            });
            return Task.FromResult(0);
        }

        private void Run(IEventHandlerFactory factory, IEventHandler handler, Type eventType, IEventData eventData)
        {
            try
            {
                handler.Handle(eventData);
            }
            catch (Exception ex)
            {
                string msg = $"执行事件“{eventType.Name}”的处理器“{handler.GetType()}”时引发异常：{ex.Message}";
                _Logger.LogError(ex, msg);
            }
            finally
            {
                factory.ReleaseHandler(handler);
            }
        }

        private Task RunAsync(IEventHandlerFactory factory, IEventHandler handler, Type eventType, IEventData eventData)
        {
            try
            {
                return handler.HandleAsync(eventData);
            }
            catch (Exception ex)
            {
                string msg = $"执行事件“{eventType.Name}”的处理器“{handler.GetType()}”时引发异常：{ex.Message}";
                _Logger.LogError(ex, msg);
            }
            finally
            {
                factory.ReleaseHandler(handler);
            }
            return Task.FromResult(0);
        }

        #endregion
    }
}