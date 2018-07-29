# 事件总线模块：EventBusPack

* 级别：PackLevel.Core
* 启动顺序：2
* 位置：OSharp.dll

> - [事件总线详解](#01)
>     - [事件数据](#011)
>     - [事件处理器](#012)
>     - [事件总线](#013)
>         - [事件发布](#0131)
>         - [事件订阅](#0132)
>     - [事件处理器工厂](#014)
>         - [单例生命周期的事件处理器工厂，用于指定事件处理器实例的存储方式](#0141)
>         - [即时生命周期的事件处理器工厂，用于指定处理器类型的存储方式](#0142)
>         - [依赖注入的事件处理器工厂](#0143)
>     - [事件存储](#015)
>         - [事件存储接口](#0151)
>         - [事件存储实现](#0152)
>             - [事件分类存储](#01521)
>             - [取消事件订阅](#01522)
>             - [获取指定事件数据的所有订阅](#01523)
>     - [执行事件](#016)
> - [事件总线初始化](#02)
> - [事件总线使用示例](#03)
>     - [声明一个事件源：登录事件数据](#031)
>     - [声明一个事件处理器：用户登录记录日志事件处理器](#032)
>     - [发布事件，触发事件](#033)

---

`EventBusPack`模块是一个简单的`发布-订阅`模式的`事件总线`系统，为业务提供简单的`发布-订阅`服务。

## <a id="01"/>事件总线详解

### <a id="011"/>事件数据
事件数据（`EventData`）是事件的数据源，是业务传递到事件处理器（`EventHandler`）的输入数据。
事件数据派生自接口`IEventData`
```
/// <summary>
/// 定义事件数据，所有事件都要实现该接口
/// </summary>
public interface IEventData
{
    /// <summary>
    /// 获取 事件编号
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 获取 事件发生的时间
    /// </summary>
    DateTime EventTime { get; }

    /// <summary>
    /// 获取或设置 事件源，触发事件的对象
    /// </summary>
    object EventSource { get; set; }
}
```
为简化使用，框架中还定义了一个事件数据基类`EventDataBase`，实际应用中，事件数据派生自此类即可
```
    /// <summary>
    /// 事件源数据信息基类
    /// </summary>
    public abstract class EventDataBase : IEventData
    {
        /// <summary>
        /// 初始化一个<see cref="EventDataBase"/>类型的新实例
        /// </summary>
        protected EventDataBase()
        {
            Id = Guid.NewGuid();
            EventTime = DateTime.Now;
        }

        /// <summary>
        /// 获取 事件编号
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// 获取 事件发生时间
        /// </summary>
        public DateTime EventTime { get; }

        /// <summary>
        /// 获取或设置 触发事件的对象
        /// </summary>
        public object EventSource { get; set; }
    }
```

### <a id="012"/>事件处理器
事件处理器`EventHandler`是执行事件业务的类，事件处理器接收事件数据`EventData`，按照特定的需求对事件数据进行处理。
事件处理器派生自接口`IEventHandler`
```
/// <summary>
/// 定义事件处理器，所有事件处理都要实现该接口
/// EventBus中，Handler的调用是同步执行的，如果需要触发就不管的异步执行，可以在实现EventHandler的Handle逻辑时使用Task.Run
/// </summary>
[IgnoreDependency]
public interface IEventHandler
{
    /// <summary>
    /// 是否可处理指定事件
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <returns>是否可处理</returns>
    bool CanHandle(IEventData eventData);

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    void Handle(IEventData eventData);

    /// <summary>
    /// 异步事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <param name="cancelToken">异步取消标识</param>
    /// <returns></returns>
    Task HandleAsync(IEventData eventData, CancellationToken cancelToken = default(CancellationToken));
}

/// <summary>
/// 定义泛型事件处理器
/// EventBus中，Handler的调用是同步执行的，如果需要触发就不管的异步执行，可以在实现EventHandler的Handle逻辑时使用Task.Run
/// </summary>
/// <typeparam name="TEventData">事件源数据</typeparam>
[IgnoreDependency]
public interface IEventHandler<in TEventData> : IEventHandler where TEventData : IEventData
{
    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    void Handle(TEventData eventData);

    /// <summary>
    /// 异步事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <param name="cancelToken">异步取消标识</param>
    Task HandleAsync(TEventData eventData, CancellationToken cancelToken = default(CancellationToken));
}
```
为简化使用，框架中还定义了一个事件处理器基类EventDataBase，实际应用中，事件数据派生自此类即可
```
/// <summary>
/// 事件处理器基类
/// </summary>
public abstract class EventHandlerBase<TEventData> : IEventHandler<TEventData> where TEventData : IEventData
{
    /// <summary>
    /// 是否可处理指定事件
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <returns>是否可处理</returns>
    public virtual bool CanHandle(IEventData eventData)
    {
        return eventData.GetType() == typeof(TEventData);
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    public virtual void Handle(IEventData eventData)
    {
        if (!CanHandle(eventData))
        {
            return;
        }
        Handle((TEventData)eventData);
    }

    /// <summary>
    /// 异步事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <param name="cancelToken">异步取消标识</param>
    /// <returns></returns>
    public virtual Task HandleAsync(IEventData eventData, CancellationToken cancelToken = default(CancellationToken))
    {
        if (!CanHandle(eventData))
        {
            return Task.FromResult(0);
        }
        return HandleAsync((TEventData)eventData, cancelToken);
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    public abstract void Handle(TEventData eventData);

    /// <summary>
    /// 异步事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <param name="cancelToken">异步取消标识</param>
    /// <returns>是否成功</returns>
    public virtual Task HandleAsync(TEventData eventData, CancellationToken cancelToken = default(CancellationToken))
    {
        return Task.Run(() => Handle(eventData), cancelToken);
    }
}

```

### <a id="013"/>事件总线
事件总线`EventBus`包含事件发布`IEventPublisher`和事件订阅`IEventSubscriber`
```
/// <summary>
/// 定义线程总线
/// </summary>
public interface IEventBus : IEventSubscriber, IEventPublisher
{ }
```
目前框架中的`IEventBus`的实现`PassThroughEventBus`，只是简单的内存传递事件，并没有做诸如`错误重试`等保障机制，需要时可自行实现，需要做的事如下：
* 将EventStore由内存模式更改为持久化存储模式
* 在EventBus中增加保障机制

#### <a id="0131"/>事件发布
业务方定义好事件数据之后，通过发布事件数据`EventData`来触发事件处理器执行
```
/// <summary>
/// 定义事件发布者
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// 同步发布指定事件
    /// </summary>
    /// <typeparam name="TEventData">事件数据类型</typeparam>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    void Publish<TEventData>(TEventData eventData, bool wait = true) where TEventData : IEventData;

    /// <summary>
    /// 同步发布指定事件，并指定事件源
    /// </summary>
    /// <typeparam name="TEventData">事件数据类型</typeparam>
    /// <param name="eventSource">事件源，触发事件的对象</param>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    void Publish<TEventData>(object eventSource, TEventData eventData, bool wait = true) where TEventData : IEventData;

    /// <summary>
    /// 同步发布指定事件
    /// </summary>
    /// <param name="eventType">事件数据类型</param>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    void Publish(Type eventType, IEventData eventData, bool wait = true);

    /// <summary>
    /// 同步发布指定事件，并指定事件源
    /// </summary>
    /// <param name="eventType">事件数据类型</param>
    /// <param name="eventSource">事件源，触发事件的对象</param>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    void Publish(Type eventType, object eventSource, IEventData eventData, bool wait = true);

    /// <summary>
    /// 异步发布指定事件
    /// </summary>
    /// <typeparam name="TEventData">事件数据类型</typeparam>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    Task PublishAsync<TEventData>(TEventData eventData, bool wait = true) where TEventData : IEventData;

    /// <summary>
    /// 异步发布指定事件，并指定事件源
    /// </summary>
    /// <typeparam name="TEventData">事件数据类型</typeparam>
    /// <param name="eventSource">事件源，触发事件的对象</param>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    Task PublishAsync<TEventData>(object eventSource, TEventData eventData, bool wait = true) where TEventData : IEventData;

    /// <summary>
    /// 异步发布指定事件
    /// </summary>
    /// <param name="eventType">事件数据类型</param>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    Task PublishAsync(Type eventType, IEventData eventData, bool wait = true);

    /// <summary>
    /// 异步发布指定事件，并指定事件源
    /// </summary>
    /// <param name="eventType">事件数据类型</param>
    /// <param name="eventSource">事件源，触发事件的对象</param>
    /// <param name="eventData">事件数据</param>
    /// <param name="wait">是否等待结果返回</param>
    Task PublishAsync(Type eventType, object eventSource, IEventData eventData, bool wait = true);
}
```

#### <a id="0132"/>事件订阅
事件处理器通过订阅指定事件数据类型`TEventData`来执行事件处理器`EventHandler`
```
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
    /// 取消指定事件数据的所有处理器
    /// </summary>
    /// <typeparam name="TEventData">事件数据类型</typeparam>
    void UnsubscribeAll<TEventData>() where TEventData : IEventData;

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
```

### <a id="014"/>事件处理器工厂
为保证每次处理事件的独立性，每个事件处理器对象`EventHandler`都有可能（指定了处理器实例除外）是不一样的，因此需要使用工厂模式来获取事件处理器实例。
```
/// <summary>
/// 定义获取<see cref="IEventHandler"/>实例的方式
/// </summary>
public interface IEventHandlerFactory
{
    /// <summary>
    /// 获取事件处理器实例
    /// </summary>
    /// <returns></returns>
    IEventHandler GetHandler();

    /// <summary>
    /// 释放事件处理器实例
    /// </summary>
    /// <param name="handler"></param>
    void ReleaseHandler(IEventHandler handler);
}
```

处理器工厂实现，分`单例、即时、依赖注入`三种生命周期的形式

#### <a id="0141"/>单例生命周期的事件处理器工厂，用于指定事件处理器实例的存储方式
```
/// <summary>
/// 单例生命周期的事件处理器实例获取方式
/// </summary>
internal class SingletonEventHandlerFactory : IEventHandlerFactory
{
    /// <summary>
    /// 初始化一个<see cref="SingletonEventHandlerFactory"/>类型的新实例
    /// </summary>
    public SingletonEventHandlerFactory(IEventHandler handler)
    {
        HandlerInstance = handler;
    }

    public IEventHandler HandlerInstance { get; }

    /// <summary>
    /// 获取事件处理器实例
    /// </summary>
    /// <returns></returns>
    public IEventHandler GetHandler()
    {
        return HandlerInstance;
    }

    /// <summary>
    /// 释放事件处理器实例
    /// </summary>
    /// <param name="handler"></param>
    public void ReleaseHandler(IEventHandler handler)
    { }
}
```

#### <a id="0142"/>即时生命周期的事件处理器工厂，用于指定处理器类型的存储方式
```
/// <summary>
/// 即时生命周期的事件处理器实例获取方式
/// </summary>
internal class TransientEventHandlerFactory<TEventHandler> : IEventHandlerFactory
    where TEventHandler : IEventHandler, new()
{
    /// <summary>
    /// 获取事件处理器实例
    /// </summary>
    /// <returns></returns>
    public IEventHandler GetHandler()
    {
        return new TEventHandler();
    }

    /// <summary>
    /// 释放事件处理器实例
    /// </summary>
    /// <param name="handler"></param>
    public void ReleaseHandler(IEventHandler handler)
    {
        (handler as IDisposable)?.Dispose();
    }
}
```

#### <a id="0143"/>依赖注入的事件处理器工厂
从依赖注入容器中解析出事件处理器实例，实例的生命周期**由依赖注册时的生命周期**决定
```
/// <summary>
/// 依赖注入事件处理器实例获取方式
/// </summary>
internal class IocEventHandlerFactory : IEventHandlerFactory
{
    private readonly Type _handlerType;

    /// <summary>
    /// 初始化一个<see cref="IocEventHandlerFactory"/>类型的新实例
    /// </summary>
    /// <param name="handlerType">事件处理器类型</param>
    public IocEventHandlerFactory(Type handlerType)
    {
        _handlerType = handlerType;
    }

    /// <summary>
    /// 获取事件处理器实例
    /// </summary>
    /// <returns></returns>
    public IEventHandler GetHandler()
    {
        return ServiceLocator.Instance.GetService(_handlerType) as IEventHandler;
    }

    /// <summary>
    /// 释放事件处理器实例
    /// </summary>
    /// <param name="handler"></param>
    public void ReleaseHandler(IEventHandler handler)
    { }
}
```

### <a id="015"/>事件存储 EventStore
事件数据`EventData`与事件处理器`EventHandler`是通过 事件数据与事件处理器工厂`IEventHandlerFactory`的一对多映射关系存储到事件存储`EventStore`中的。

#### <a id="0151"/>事件存储接口
```
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
```

#### <a id="0152"/>事件存储实现

##### <a id="01521"/>事件分类存储
OSharp框架中默认实现了基于内存的事件存储`InMemoryEventStore`。存储内部通过一个`{typeof(TEventData), List<IEventHandlerFactory>}` 的以`事件数据类型为键、事件处理器工厂集合为值`的字典来存储事件数据与事件处理器的`一对多`的映射关系。
按照事件处理器工厂的类型设定，存储分以下几种方式：
* 事件数据类型与事件处理器实例的映射，使用`单例生命周期`工厂存储
```
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
```
* 事件数据类型与事件处理器类型的映射，使用`即时生命周期`工厂存储
```
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
```
* 对于依赖注入方式订阅的事件处理器，则使用`依赖注入`工厂存储

##### <a id="01522"/>取消事件订阅
取消事件订阅，将其映射关系从存储字典中移除即可。移除后，下次发布事件，就不会再触发已经移除的事件订阅了。
```
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
    });
}

/// <summary>
/// 移除指定事件源的所有处理器
/// </summary>
/// <param name="eventType">事件源类型</param>
public void RemoveAll(Type eventType)
{
    GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
}
```

##### <a id="01523"/>获取指定事件数据的所有订阅
当发布指定事件数据的事件时，需获取该事件数据的所有事件处理器工厂来创建事件处理器实例，来执行事件业务。
```
/// <summary>
/// 获取指定事件源的所有处理器工厂
/// </summary>
/// <returns></returns>
public IDictionary<Type, IEventHandlerFactory[]> GetHandlers(Type eventType)
{
    return _handlerFactories.Where(item => item.Key == eventType || item.Key.IsAssignableFrom(eventType))
        .ToDictionary(item => item.Key, item => item.Value.ToArray());
}
```

### <a id="016"/>执行事件

事件执行流程如下：
* 由事件数据类型，从事件存储`IEventStore`中获取到该事件的所有事件处理器工厂对象
* 获取到的处理器工厂，逐个获取处理器实例
* 如果等待结果(`wait`参数)，则正常执行
* 如果不等待结果，则使用`Task.Run()`触发执行后立即返回

> 这里只展现同步的事件执行，异步事件执行也同理

```
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

    IDictionary<Type, IEventHandlerFactory[]> dict = EventStore.GetHandlers(eventType);
    foreach (var typeItem in dict)
    {
        foreach (IEventHandlerFactory factory in typeItem.Value)
        {
            InvokeHandler(factory, eventType, eventData, wait);
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
        Logger.LogWarning($"事件源“{eventData.GetType()}”的事件处理器无法找到");
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

private void Run(IEventHandlerFactory factory, IEventHandler handler, Type eventType, IEventData eventData)
{
    try
    {
        handler.Handle(eventData);
    }
    catch (Exception ex)
    {
        string msg = $"执行事件“{eventType.Name}”的处理器“{handler.GetType()}”时引发异常：{ex.Message}";
        Logger.LogError(ex, msg);
    }
    finally
    {
        factory.ReleaseHandler(handler);
    }
}
```

## <a id="02"/>事件总线初始化

除了通过直接通过代码订阅事件处理器，大多数事件的订阅方式都是通过在系统初始化时，反射程序集获取到事件处理器类型，通过依赖注入的注册方式来订阅。此方式主要通过`EventBusBuilder`初始化类来完成。
```
/// <summary>
/// EventBus初始化
/// </summary>
internal class EventBusBuilder : IEventBusBuilder
{
    private readonly IEventHandlerTypeFinder _typeFinder;
    private readonly IEventBus _eventBus;

    /// <summary>
    /// 初始化一个<see cref="EventBusBuilder"/>类型的新实例
    /// </summary>
    public EventBusBuilder(IEventHandlerTypeFinder typeFinder, IEventBus eventBus)
    {
        _typeFinder = typeFinder;
        _eventBus = eventBus;
    }

    /// <summary>
    /// 初始化EventBus
    /// </summary>
    public void Build()
    {
        Type[] types = _typeFinder.FindAll(true);
        if (types.Length == 0)
        {
            return;
        }
        _eventBus.SubscribeAll(types);
    }
}
```
获取到所有处理器类型之后，反射订阅
```
/// <summary>
/// 自动订阅所有事件数据及其处理类型
/// </summary>
/// <param name="eventHandlerTypes">事件处理器类型集合</param>
public virtual void SubscribeAll(Type[] eventHandlerTypes)
{
    Check.NotNull(eventHandlerTypes, nameof(eventHandlerTypes));
    
    foreach (Type handlerType in eventHandlerTypes)
    {
        Type handlerInterface = handlerType.GetInterface("IEventHandler`1"); //获取该类实现的泛型接口
        if (handlerInterface == null)
        {
            continue;
        }
        Type eventType = handlerInterface.GetGenericArguments()[0]; //泛型的EventData类型
        IEventHandlerFactory factory = new IocEventHandlerFactory(handlerType);
        EventStore.Add(eventType, factory);
        Logger.LogDebug($"创建事件“{eventType}”到处理器“{handlerType}”的订阅配对");
    }
    Logger.LogInformation($"共从程序集创建了 {eventHandlerTypes.Length} 个事件处理器的事件订阅");
}
```

## <a id="03"/>事件总线使用示例

下面以一个 `用户登录成功后记录登录日志` 的示例来说明事件总线的使用。

#### <a id="031"/>声明一个事件源：登录事件数据`LoginEventData`

登录事件数据，需要提供用户登录的客户端信息和登录用户信息
```
/// <summary>
/// 登录事件数据
/// </summary>
public class LoginEventData : EventDataBase
{
    /// <summary>
    /// 获取或设置 登录信息，包含用户登录的客户端信息
    /// </summary>
    public LoginDto LoginDto { get; set; }

    /// <summary>
    /// 获取或设置 登录用户
    /// </summary>
    public User User { get; set; }
}
```

#### <a id="032"/>声明一个事件处理器：用户登录记录日志事件处理器`LoginLoginLogEventHandler`

* 一个用户登录，可能会触发很多事件，比如 `异常登录发送邮件`、`登录发消息通知好友`等等，记录日志只是其中的一个订阅，因而事件处理器一定要做到 `单一职责`，不要在一个处理器中做很多件事。
* 这个用户登录记录日志事件处理器，将调用数据仓储 `Repository`，将登录日志持久化到数据库中。
* 事件处理器实现了接口`IEventHandler<TEventData>`(基类`EventHandlerBase<LoginEventData>`实现了该接口)，事件总线模块初始化时，将自动添加`LoginLoginLogEventHandler`对`LoginEventData`的事件订阅。
* 事件处理器实现了接口`ITransientDependency`，框架的依赖注入功能在初始化的时候，将自动将此事件处理器注册到依赖注入服务中，生命周期类型为`ServiceLifetime.Transient`
```
/// <summary>
/// 用户登录事件：登录日志
/// </summary>
public class LoginLoginLogEventHandler : EventHandlerBase<LoginEventData>, ITransientDependency
{
    private readonly IRepository<LoginLog, Guid> _loginLogRepository;

    /// <summary>
    /// 初始化一个<see cref="LoginLoginLogEventHandler"/>类型的新实例
    /// </summary>
    public LoginLoginLogEventHandler(IRepository<LoginLog, Guid> loginLogRepository)
    {
        _loginLogRepository = loginLogRepository;
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    public override void Handle(LoginEventData eventData)
    {
        LoginLog log = new LoginLog()
        {
            Ip = eventData.LoginDto.Ip,
            UserAgent = eventData.LoginDto.UserAgent,
            UserId = eventData.User.Id
        };
        _loginLogRepository.Insert(log);
    }

    /// <summary>
    /// 异步事件处理
    /// </summary>
    /// <param name="eventData">事件源数据</param>
    /// <param name="cancelToken">异步取消标识</param>
    /// <returns>是否成功</returns>
    public override Task HandleAsync(LoginEventData eventData, CancellationToken cancelToken = default(CancellationToken))
    {
        LoginLog log = new LoginLog()
        {
            Ip = eventData.LoginDto.Ip,
            UserAgent = eventData.LoginDto.UserAgent,
            UserId = eventData.User.Id
        };
        return _loginLogRepository.InsertAsync(log);
    }
}
```

#### <a id="033"/>发布事件，触发事件

在用户成功登录系统后，调用 事件总线`EventBus` 发布事件。
```
/// <summary>
/// 业务实现：身份认证模块
/// </summary>
public class IdentityService : IdentityContract
{
    private readonly IEventBus _eventBus;

    //注入事件总线 IEventBus
    public IdentityService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    /// <summary>
    /// 使用账号登录
    /// </summary>
    /// <param name="dto">登录信息</param>
    /// <returns>业务操作结果</returns>
    public async Task<OperationResult<User>> Login(LoginDto dto)
    {
        // 用户登录逻辑，登录成功
        // ...

        //发布登录事件
        LoginEventData loginEventData = new LoginEventData() { LoginDto = dto, User = user };
        await _eventBus.PublishAsync(loginEventData);
    }
}

```

发布登录事件后，事件总线`EventBus`将自动查找`LoginEventData`的所有订阅的事件处理器并执行。