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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using OSharp.Dependency;
using OSharp.EventBuses.Handlers;
using OSharp.EventBuses.Handlers.Internal;
using OSharp.Reflection;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public sealed class EventBus : ISingletonDependency
    {
        private readonly ConcurrentDictionary<Type, MethodInfo> _handlerMethodCache;
        private readonly IEventStore _eventStore;
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="EventBus"/>类型的新实例
        /// </summary>
        public EventBus(IEventStore eventStore, ILogger<EventBus> logger)
        {
            _eventStore = eventStore;
            _logger = logger;
            _handlerMethodCache = new ConcurrentDictionary<Type, MethodInfo>();
        }

        /// <summary>
        /// 获取 默认事件总线
        /// </summary>
        public static EventBus Default => ServiceLocator.Instance.GetService<EventBus>();
        
        /// <summary>
        /// 将指定事件源数据与相应处理器注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <typeparam name="TEventHandler">事件处理器类型</typeparam>
        public void Register<TEventData, TEventHandler>() where TEventData : IEventData where TEventHandler : IEventHandler, new()
        {
            _eventStore.Add<TEventData, TEventHandler>();
        }

        /// <summary>
        /// 将指定事件源数据与相应处理器委托实现注册到事件总线
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="action">处理器委托实现</param>
        public void Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            IEventHandler eventHandler = new ActionEventHandler<TEventData>(action);
            Register<TEventData>(eventHandler);
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
        /// <param name="eventType">事件源数据类型</param>
        /// <param name="eventHandler">事件处理器实例</param>
        public void Register(Type eventType, IEventHandler eventHandler)
        {
            _eventStore.Add(eventType, eventHandler);
        }

        /// <summary>
        /// 遍历程序集类型，自动注册事件处理器及所属事件源数据到事件总线
        /// </summary>
        /// <param name="assembly">程序集</param>
        public void RegisterAllEventHandlers(Assembly assembly)
        {
            Type[] handlerTypes = assembly.GetTypes().Where(type => type.IsDeriveClassFrom(typeof(IEventHandler<>))).ToArray();
            if (handlerTypes.Length == 0)
            {
                return;
            }
            foreach (Type handlerType in handlerTypes)
            {
                Type handlerInterface = handlerType.GetInterface("IEventHandler`1");//获取该类实现的泛型接口
                if (handlerInterface == null)
                {
                    continue;
                }
                Type eventType = handlerInterface.GetGenericArguments()[0]; //泛型的EventData类型
                IEventHandlerFactory factory = new IocEventHandlerFactory(handlerType);
                _eventStore.Add(eventType, factory);
            }
        }

        /// <summary>
        /// 注销指定事件源数据的处理器委托实现
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="action">处理器委托实现</param>
        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Check.NotNull(action, nameof(action));

            _eventStore.Remove(action);
        }

        /// <summary>
        /// 注销指定事件源数据的处理器委托实例
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventHandler">处理器委托实例</param>
        public void Unregister<TEventData>(IEventHandler<TEventData> eventHandler) where TEventData : IEventData
        {
            Unregister(typeof(TEventData), eventHandler);
        }

        /// <summary>
        /// 注销指定事件源数据的处理器委托实例
        /// </summary>
        /// <param name="eventType">事件源数据类型</param>
        /// <param name="eventHandler">处理器委托实例</param>
        public void Unregister(Type eventType, IEventHandler eventHandler)
        {
            _eventStore.Remove(eventType, eventHandler);
        }

        /// <summary>
        /// 注销指定事件源数据的所有处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
            UnregisterAll(typeof(TEventData));
        }

        /// <summary>
        /// 注销指定事件源数据的所有处理器 
        /// </summary>
        /// <param name="eventType">事件源数据类型</param>
        public void UnregisterAll(Type eventType)
        {
            _eventStore.RemoveAll(eventType);
        }

        /// <summary>
        /// 触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventData">事件源数据</param>
        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            Trigger<TEventData>(null, eventData);
        }

        /// <summary>
        /// 触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        public void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            Trigger(typeof(TEventData), eventSource, eventData);
        }

        /// <summary>
        /// 触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件源数据</param>
        public void Trigger(Type eventType, IEventData eventData)
        {
            Trigger(eventType, null, eventData);
        }

        /// <summary>
        /// 触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        public void Trigger(Type eventType, object eventSource, IEventData eventData)
        {
            List<Exception> exceptions = new List<Exception>();
            TriggerHandlingException(eventType, eventSource, eventData, exceptions);
            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }
                throw new AggregateException($"触发事件“{eventType}”时引发多个异常", exceptions);
            }
        }

        /// <summary>
        /// 异步触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventData">事件源数据</param>
        /// <returns></returns>
        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return TriggerAsync<TEventData>(null, eventData);
        }

        /// <summary>
        /// 异步触发指定事件源数据的处理器
        /// </summary>
        /// <typeparam name="TEventData">事件源数据类型</typeparam>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        public Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            return TriggerAsync(typeof(TEventData), eventSource, eventData);
        }

        /// <summary>
        /// 异步触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件源数据</param>
        public Task TriggerAsync(Type eventType, IEventData eventData)
        {
            return TriggerAsync(eventType, null, eventData);
        }

        /// <summary>
        /// 异步触发指定类型的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">事件触发源</param>
        /// <param name="eventData">事件源数据</param>
        public Task TriggerAsync(Type eventType, object eventSource, IEventData eventData)
        {
            ExecutionContext.SuppressFlow();
            Task task = Task.Factory.StartNew(() =>
            {
                try
                {
                    Trigger(eventType, eventSource, eventData);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }
            });
            ExecutionContext.RestoreFlow();
            return task;
        }

        private void TriggerHandlingException(Type eventType, object eventSource, IEventData eventData, List<Exception> exceptions)
        {
            eventData.EventSource = eventSource;
            IEnumerable<(Type EventType, IEnumerable<IEventHandlerFactory> HandlerFactories)> valueTuples = _eventStore.GetHandlers(eventType);
            foreach (var tuple in valueTuples)
            {
                foreach (IEventHandlerFactory handlerFactory in tuple.HandlerFactories)
                {
                    IEventHandler handler = handlerFactory.GetHandler();
                    try
                    {
                        if (handler == null)
                        {
                            exceptions.Add(new Exception($"注册的事件“{tuple.EventType.Name}”的处理器未实现接口“IEventHandler<{tuple.EventType.Name}>”"));
                            return;
                        }
                        //方法缓存
                        MethodInfo method = _handlerMethodCache.GetOrAdd(tuple.EventType,
                            type =>
                            {
                                Type handlerType = typeof(IEventHandler<>).MakeGenericType(type);
                                return handlerType.GetMethod("HandleEvent", new[] { tuple.EventType });
                            });
                        Func<object, object[], object> fastMethod = FastInvokeHandler.Create(method);
                        //异步方式执行EventHandler
                        Task.Run(() =>
                        {
                            try
                            {
                                fastMethod.Invoke(handler, new object[] { eventData });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, ex.Message);
                            }
                            finally
                            {
                                handlerFactory.ReleaseHandler(handler);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
        }
    }
}