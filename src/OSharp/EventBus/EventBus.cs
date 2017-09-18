// -----------------------------------------------------------------------
//  <copyright file="EventBus.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-18 12:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;

using OSharp.Collections;
using OSharp.EventBus.Handlers;
using OSharp.EventBus.Handlers.Internal;
using OSharp.Reflection;


namespace OSharp.EventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public class EventBus : IEventBus
    {
        private static Lazy<EventBus> DefaultLazy => new Lazy<EventBus>(() => new EventBus());
        private static readonly object LockObj = new object();

        private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories;

        /// <summary>
        /// 初始化一个<see cref="EventBus"/>类型的新实例
        /// </summary>
        private EventBus()
        {
            _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        }

        /// <summary>
        /// 获取 默认事件总线
        /// </summary>
        public static EventBus Default => DefaultLazy.Value;

        /// <summary>
        /// 将指定事件源数据与相应处理器注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <typeparam name="TEventHandler">事件处理器类型</typeparam>
        public void Register<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new()
        {
            AddToDictIfExists(typeof(TEventData), new TransientEventHandlerFactory<TEventHandler>());
        }

        /// <summary>
        /// 将指定事件源数据与相应处理器委托实现注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="action">处理器委托实现</param>
        public void Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Register<TEventData>(new ActionEventHandler<TEventData>(action));
        }

        /// <summary>
        /// 将指定事件源数据与相应处理器注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventHandler">事件处理器实例</param>
        public void Register<TEventData>(IEventHandler eventHandler) where TEventData : IEventData
        {
            Register(typeof(TEventData), eventHandler);
        }

        /// <summary>
        /// 将指定事件源数据与相应处理器注册到事件总线
        /// </summary>
        /// <param name="eventDataType">事件源数据类型</param>
        /// <param name="eventHandler">事件处理器实例</param>
        public void Register(Type eventDataType, IEventHandler eventHandler)
        {
            AddToDictIfExists(eventDataType, new SingletonEventHandlerFactory(eventHandler));
        }

        /// <summary>
        /// 遍历程序集类型，自动注册事件处理器及所属事件源数据到事件总线
        /// </summary>
        /// <param name="assembly">程序集</param>
        public void RegisterAllEventHandlers(Assembly assembly)
        {
            Type[] handlerTypes = assembly.GetTypes().Where(type => type.IsDeriveClassFrom(typeof(IEventHandler<>))).ToArray();
            foreach (Type handlerType in handlerTypes)
            {
                Type handlerInterface = handlerType.GetInterface("IEventHandler`1");//获取该类实现的泛型接口
                if (handlerInterface == null)
                {
                    continue;
                }
                Type eventDataType = handlerInterface.GetGenericArguments()[0]; //泛型的EventData类型
                AddToDictIfExists(eventDataType, new IocEventHandlerFactory(handlerType));
            }
        }

        /// <summary>
        /// 注销指定事件源数据的处理器委托实现
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="action">处理器委托实现</param>
        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注销指定事件源数据的处理器委托实例
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="handler">处理器委托实例</param>
        public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注销指定事件源数据的处理器委托实例
        /// </summary>
        /// <param name="eventType">事件源数据类型</param>
        /// <param name="handler">处理器委托实例</param>
        public void Unregister(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注销指定事件源数据的所有处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注销指定事件源数据的所有处理器
        /// </summary>
        /// <param name="eventType">事件源数据类型</param>
        public void UnregisterAll(Type eventType)
        {
            throw new NotImplementedException();
        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Trigger<TEventData>(Type handlerType, TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public async Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public async Task TriggerAsync<TEventData>(Type handlerType, TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        private void AddToDictIfExists(Type eventDataType, IEventHandlerFactory eventHandlerFactory)
        {
            lock (LockObj)
            {
                if (_handlerFactories.TryGetValue(eventDataType, out List<IEventHandlerFactory> factories))
                {
                    factories.AddIfNotExist(eventHandlerFactory);
                }
                else
                {
                    factories = new List<IEventHandlerFactory>() { eventHandlerFactory };
                }
                _handlerFactories[eventDataType] = factories;
            }
        }
    }
}