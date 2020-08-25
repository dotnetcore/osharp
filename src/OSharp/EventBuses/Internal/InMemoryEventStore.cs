// -----------------------------------------------------------------------
//  <copyright file="InMemoryEventStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-22 1:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Data;
using OSharp.Reflection;


namespace OSharp.EventBuses.Internal
{
    /// <summary>
    /// 内存事件存储
    /// </summary>
    internal class InMemoryEventStore : IEventStore
    {
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories;

        /// <summary>
        /// 初始化一个<see cref="InMemoryEventStore"/>类型的新实例
        /// </summary>
        public InMemoryEventStore(IServiceProvider provider)
        {
            _logger = provider.GetLogger<InMemoryEventStore>();
            _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        }

        /// <summary>
        /// 将事件源类型与事件处理类型添加到存储，这里使用的是类型，应当使用即时的处理器工厂来存储
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <typeparam name="TEventHandler">数据处理器类型</typeparam>
        public void Add<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new()
        {
            IEventHandlerFactory factory = new TransientEventHandlerFactory<TEventHandler>();
            Add(typeof(TEventData), factory);
        }

        /// <summary>
        /// 将事件源类型与事件处理器实例添加到存储，这里使用的是处理器实例，应当使用单例的处理器工厂来存储
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        /// <param name="eventHandler">事件处理器实例</param>
        public void Add(Type eventType, IEventHandler eventHandler)
        {
            Check.NotNull(eventType, nameof(eventType));
            Check.NotNull(eventHandler, nameof(eventHandler));

            IEventHandlerFactory factory = new SingletonEventHandlerFactory(eventHandler);
            Add(eventType, factory);
        }

        /// <summary>
        /// 将事件源与事件处理器工厂添加到存储
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        /// <param name="factory">事件处理器工厂</param>
        public void Add(Type eventType, IEventHandlerFactory factory)
        {
            Check.NotNull(eventType, nameof(eventType));
            Check.NotNull(factory, nameof(factory));

            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.AddIfNotExist(factory));
            _logger.LogDebug($"添加事件类型“{eventType}”的处理器订阅到内存事件存储 InMemoryEventStore");
        }

        /// <summary>
        /// 移除指定事件源的处理委托实现
        /// </summary>
        /// <typeparam name="TEventData">事件源类型</typeparam>
        /// <param name="action">事件处理委托</param>
        public void Remove<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Check.NotNull(action, nameof(action));

            GetOrCreateHandlerFactories(typeof(TEventData)).Locking(factories =>
            {
                factories.RemoveAll(factory =>
                {
                    if (!(factory is SingletonEventHandlerFactory singletonFactory))
                    {
                        return false;
                    }
                    if (!(singletonFactory.HandlerInstance is ActionEventHandler<TEventData> handler))
                    {
                        return false;
                    }
                    return handler.Action == action;
                });
                _logger.LogDebug($"移除事件处理处理委托“{action}”的处理器订阅");
            });
        }

        /// <summary>
        /// 移除指定事件源与处理器实例
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        /// <param name="eventHandler">处理器实例</param>
        public void Remove(Type eventType, IEventHandler eventHandler)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories =>
            {
                factories.RemoveAll(factory => (factory as SingletonEventHandlerFactory)?.HandlerInstance == eventHandler);
                _logger.LogDebug($"移除事件处理类型“{eventType}”的“{eventHandler.GetType()}”处理器订阅");

            });
        }

        /// <summary>
        /// 移除指定事件源的所有处理器
        /// </summary>
        /// <param name="eventType">事件源类型</param>
        public void RemoveAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories =>
            {
                factories.Clear();
                _logger.LogDebug($"移除事件处理类型“{eventType}”的所有处理器订阅");
            });
        }

        /// <summary>
        /// 获取指定事件源的所有处理器工厂
        /// </summary>
        /// <returns></returns>
        public IDictionary<Type, IEventHandlerFactory[]> GetHandlers(Type eventType)
        {
            return _handlerFactories.Where(item => item.Key == eventType || item.Key.IsAssignableFrom(eventType))
                .ToDictionary(item => item.Key, item => item.Value.ToArray());
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return _handlerFactories.GetOrAdd(eventType, type => new List<IEventHandlerFactory>());
        }
    }
}