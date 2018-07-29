# 依赖注入模块：DependencyPack

* 级别：PackLevel.Core
* 启动顺序：1
* 位置：OSharp.dll

---
依赖注入模块`DependencyPack`主要为系统提供自动依赖注入功能。

## 依赖注入模块组成

###依赖注入服务自动注册

依赖注入的服务和服务实现的自动检索机制
* 定义了三个空接口`ISingletonDependency`,`IScopeDependency`,`ITransientDependency`，分别表示单例的`ServiceLifetime.Singleton`,区域的`ServiceLifetime.Scoped`,即时的`ServiceLifetime.Transient`三种生命周期的注入类型，实现了相应的接口，即自动注入为相应的生命周期类型。
* 定义了一个特性`IgnoreDependencyAttribute`，当接口标注了`[IgnoreDependency]`特性后，在初始化依赖注入时，该接口将不作为`依赖注入服务`进行注册，即忽略该接口的注入。
* 定义了一个特性`MultipleDependencyAttribute`，当接口标注了`MultipleDependency`特性后，将允许在此接口的`依赖注入服务`注册多个服务实现。即：
    * 没有标注`[MultipleDependency]`特性的接口，使用`services.TryAdd`方式进行注册，即一个服务最多可以注册一个服务实现：
        ```
        services.TryAdd(new ServiceDescriptor(interfaceType, implementationType, lifetime));
        ```
    * 而标注了`[MultipleDependency]`特性的接口，则使用`services.Add`方式进行注册，即一个服务可以注册多于一个的服务实现。
        ```
        services.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));
        ```
### 服务定位器 ServiceLocator

  定义了一个`服务定位`模式的服务定位器`ServiceLocator`，用于在非注入的情况下使用依赖注入服务。
* `ServiceLocator`是个单例，通过`ServiceLocator.Instance`来使用
* 模块初始化时，将全局的`IServiceCollection`,`IServiceProvider`设置到定位器中
    ```
    ServiceLocator.Instance.SetServiceCollection(services);
    ServiceLocator.Instance.SetApplicationServiceProvider(app.ApplicationServices);
    ```
* 全局的`IServiceProvider`可以直接获取到`ServiceLifetime.Singleton`,`ServiceLifetime.Transient`两种生命周期的注入对象
* 对于`ServiceLifetime.Scoped`生命周期的对象，定位器中通过注入`IScopedServiceResolver`接口来获取该类型对象。当处于`HttpContext`的有效范围内，`IScopedServiceResolver`的服务实现是在`AspNetCorePack`模块初始化时注入的`RequestScopedServiceResolver`，即可获取到`HttpContext`范围内的`ServiceLifetime.Scoped`生命周期的对象
* 在`ServiceLifetime.Singleton`生命周期的服务中执行`ServiceLifetime.Scoped`生命周期的业务时，需要使用`provider.CreateScope()`创建一个`ServiceLifetime.Scoped`的作用域。
但有时`ServiceLifetime.Singleton`生命周期的服务是处于`HttpContext`的范围内的，这时Scoped的作用域就应直接使用HttpContext的作用域，为统一处理此类需求，定位器中提供了`void ExcuteScopedWork(Action<IServiceProvider> action)`方法来供使用
    ```
    /// <summary>
    /// 执行<see cref="ServiceLifetime.Scoped"/>生命周期的业务逻辑
    /// 1.当前处理<see cref="ServiceLifetime.Scoped"/>生命周期外，使用CreateScope创建<see cref="ServiceLifetime.Scoped"/>
    /// 生命周期的ServiceProvider来执行，并释放资源
    /// 2.当前处于<see cref="ServiceLifetime.Scoped"/>生命周期内，
    /// 直接使用<see cref="ServiceLifetime.Scoped"/>的ServiceProvider来执行
    /// </summary>
    public void ExcuteScopedWork(Action<IServiceProvider> action)
    {
        if (_provider == null)
        {
            throw new OsharpException("Root级别的IServiceProvider不存在，无法执行Scoped业务");
        }
        IServiceProvider scopedProvider = ScopedProvider;
        IServiceScope newScope = null;
        if (scopedProvider == null)
        {
            newScope = _provider.CreateScope();
            scopedProvider = newScope.ServiceProvider;
        }
        try
        {
            action(scopedProvider);
        }
        finally
        {
            newScope?.Dispose();
        }
    }
    ```
    使用起来很简单：
    ```
    ServiceLocator.Instance.ExcuteScopedWork(provider =>
    {
        IRepository<User, int>userRepository = provider.GetService<IRepository<User, int>>();
        //...
    });
    ```
    
## 依赖注入初始化

### 初始化流程

#### 依赖注入服务查找

* 通过`IAllAssemblyFinder`查找出引用的所有程序集
* 反射程序集，使用`SingletonDependencyTypeFinder`,`ScopedDependencyTypeFinder`,`TransientDependencyTypeFinder`三个查找类，分别查找出 单例的`ServiceLifetime.Singleton`,区域的`ServiceLifetime.Scoped`,即时的`ServiceLifetime.Transient`三种生命周期的注入类型，即实现了 `ISingletonDependency`,`IScopeDependency`,`ITransientDependency` 的所有**服务实现类型**。
    > 如需自定义查找行为，可通过重写`DependencyPack`指定`ServiceScanOptions`选项属性使用自定义的类型查找器

#### 服务注册

* 遍历查找到的服务实现类型
* 获取服务实现类型的所有接口，排除标记了`[IgnoreDependency]`特性的接口
* 如果找到的接口数量为 0，将服务实现类型自身注册为服务
    ```
    services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime));
    ```
* 将查找到的所有有效接口作为服务，服务实现类型作为服务实现，进行服务注册
    * 当接口标注了`[MultipleDependency]`特性时，允许多服务实现注册
        ```
        services.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));
        ```
    * 未标记该特性时，只尝试注册一个服务实现
        ```
        services.TryAdd(new ServiceDescriptor(interfaceType, implementationType, lifetime));
        ```
    * 当需要注册的接口服务存在多个时，只注册第一个接口，其余接口注册由第一个接口获取的实例，以保证实例一致性
        ```
        //获取第一个接口
        Type firstInterfaceType = interfaceTypes[0];
        //后边的接口注册由第一个接口获取的实例
        services.Add(new ServiceDescriptor(interfaceType, provider => provider.GetService(firstInterfaceType), lifetime));
        OR
        services.TryAdd(new ServiceDescriptor(interfaceType, provider => provider.GetService(firstInterfaceType), lifetime));
        ```
