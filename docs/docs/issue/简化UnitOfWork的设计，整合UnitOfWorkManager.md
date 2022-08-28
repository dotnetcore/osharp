[toc]

# 简化UnitOfWork的设计，整合UnitOfWorkManager

## 您的功能请求与现有问题有关吗？请描述

同为`Scoped`的`IUnitOfWorkManager`，`IUnitOfWork`，`DbContextBase`的设计存在重复，参考 #120 的建议进行简化

## 描述您想要的需求方案

### 需求分析

- 在一个`Scoped`生命周期中，使用一个`IUnitOfWork`管理所有的`DbContext`实例
- 多个`DbContext`实例以`DbConnection`分组，同一个连接对象`DbConnection`的多个上下文共享事务
- 建立相同`DbConnection`的数据上下文`DbContext`缓存，获取数据上下文实例时，优先从缓存获取，不存在再从`IServiceProvider`中解析
- `IUnitOfWork`需要调用`IUnitOfWork.EnableTransaction()`手动开启事务，才能执行手动事务流程，否则使用 EFCore 默认的自动事务
- 事务业务需要使用`IUnitOfWork.EnableTransaction()`和`IUnitOfWork.Commit()`进行包裹
- 调用`IUnitOfWork.EnableTransaction()`时，给事务层次打标记，以处理事务嵌套的问题，`IUnitOfWork.Commit()`提交事务时只提交最外一层的事务

### 工作单元设计 `IUnitOfWork`

``` csharp
   /// <summary>
    /// 定义一个单元操作内的功能，管理单元操作内涉及的所有上下文对象及其事务
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获取 是否已提交
        /// </summary>
        bool HasCommitted { get; }

        /// <summary>
        /// 启用事务，事务代码写在 UnitOfWork.EnableTransaction() 与 UnitOfWork.Commit() 之间
        /// </summary>
        void EnableTransaction();

        /// <summary>
        /// 获取指定数据上下文类型的实例
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
        IDbContext GetEntityDbContext<TEntity, TKey>() where TEntity : IEntity<TKey>;
        
        /// <summary>
        /// 获取指定数据实体的上下文实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体所属上下文实例</returns>
        IDbContext GetEntityDbContext(Type entityType);

        /// <summary>
        /// 获取指定类型的上下文实例
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <returns></returns>
        IDbContext GetDbContext(Type dbContextType);

        /// <summary>
        /// 对数据库连接开启事务或应用现有同连接对象的上下文事务
        /// </summary>
        /// <param name="context">数据上下文</param>
        void BeginOrUseTransaction(IDbContext context);

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        void Rollback();

#if NET5_0

        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="cancellationToken">异步取消标记</param>
        /// <returns></returns>
        Task BeginOrUseTransactionAsync(IDbContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步提交当前上下文的事务更改
        /// </summary>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步回滚所有事务
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync(CancellationToken cancellationToken = default);
#endif
    }
```

### 工作单元使用

#### 1. 使用`ServiceLifetime.Scoped`生命周期将`IUnitOfWork`注册到DI

``` csharp
//注册IUnitOfWork
services.TryAddScoped<IUnitOfWork, UnitOfWork>();
//注册数据上下文
services.AddOsharpDbContext<DefaultDbContext>();
```

### 2. 从DI解析`IUnitOfWork`对象，将事务代码包裹在 `IUnitOfWork.EnableTransction()` 与 `IUnitOfWork.Commit()` 之间

``` csharp
IUnitOfWork unitOfWork = serviceProvider.GetService<IUnitOfWork>();
// 如果需要事务操作，要手动启用事务
unitOfWork.EnableTransaction();

// do something 事务的业务操作

unitOfWork.Commit();
```

框架内提供了一个简化的获取`IUnitOfWork`的扩展方法

``` csharp
/// <summary>
/// 从服务提供者获取 <see cref="IUnitOfWork"/>
/// </summary>
/// <param name="provider">服务提供者</param>
/// <param name="enableTransaction">是否启用事务</param>
/// <returns></returns>
public static IUnitOfWork GetUnitOfWork(this IServiceProvider provider, bool enableTransaction = false)
{
    IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
    if (enableTransaction)
    {
        unitOfWork.EnableTransaction();
    }
    return unitOfWork;
}
```

调用时，按传入的`enableTransaction`决定是否启用事务

```csharp
IUnitOfWork unitOfWork = provider.GetUnitOfWork(enableTransaction: true);
```


> 注意：`IUnitOfWork.EnableTransction()` 与 `IUnitOfWork.Commit()` 必须成对出现，否则会出现事务无法正常提交的问题



### 工作单元嵌套

利益于`IUnitOfWork.EnableTransaction()`的设计，`IUnitOfWork`事务已经支持嵌套，只要保持层次结构中`IUnitOfWork.EnableTransction()` 与 `IUnitOfWork.Commit()` 成对出现，事务提交时，如果不是最外层工作单元，事务提交会跳过，直到最外层事务时，才会执行真正的`IUnitOfWork.Commit()`。因此，在业务实现时，可以按需要随意设计事务功能，在事务互相调用时，不会因为事务嵌套产生多次提交事务的冲突。

```csharp
public class Foo1Service
{
    public void FooMethod()
    {
        IUnitOfWork unitOfWork = serviceProvider.GetService<IUnitOfWork>();
        unitOfWork.EnableTransaction();

        // do something 事务的业务操作

        unitOfWork.Commit();
    }
}

public class Foo2Service
{
    public void FooMethod()
    {
        IUnitOfWork unitOfWork = serviceProvider.GetService<IUnitOfWork>();
        unitOfWork.EnableTransaction();

        // do something 事务的业务操作

        unitOfWork.Commit();
    }
}
```

在Controller中调用Service
```csharp
public class FooController
{
    private readonly Foo1Service _foo1Service;
    private readonly Foo2Service _foo2Service;

    public FooController(Foo1Service foo1Service, Foo2Service foo2Service)
    {
        _foo1Service = foo1Service;
        _foo2Service = foo2Service;
    }

    public void FooAction()
    {
        IUnitOfWork unitOfWork = serviceProvider.GetService<IUnitOfWork>();
        unitOfWork.EnableTransaction();

        // do something 事务的业务操作
        _foo1Service.FooMethod();
        _foo2Service.FooMethod();

        unitOfWork.Commit();
    }
}
```

如上，`FooController`调用了`Foo1Service`和`Foo2Service`形成事务嵌套，`Foo1Service`和`Foo2Service`的事务提交将被跳过，直到`FooController`中的`unitOfWork.Commit()`，事务提交才真正的执行。