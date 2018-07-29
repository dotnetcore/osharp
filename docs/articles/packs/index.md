# OSharp Pack 模块系统

> - [如何定义一个Pack](#01)
>     - [基类OSharpPack定义说明](#011)
>     - [依赖模块配置特性 DependsOnPacksAttribute](#012)
> - [Pack管理器 OSharpPackMamager](#02)
>     - [OSharpPackManager执行流程](#021)
>     - [Pack初始化流程图](#021)
---

OSharp 框架的各个功能模块，是由一个一个Pack有机组合而成的。

## <a id="01"/>如何定义一个Pack

每个Pack，都派生于一个特定的基类 [`OSharpPack`](http://docs.osharp.org/api/OSharp.Core.Packs.OsharpPack.html)，基类定义如下：
```
/// <summary>
/// OSharp模块基类
/// </summary>
public abstract class OsharpPack
{
    /// <summary>
    /// 获取 模块级别，级别越小越先启动
    /// </summary>
    public virtual PackLevel Level => PackLevel.Business;

    /// <summary>
    /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
    /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
    /// </summary>
    public virtual int Order => 0;

    /// <summary>
    /// 获取 是否已可用
    /// </summary>
    public bool IsEnabled { get; protected set; }

    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public virtual IServiceCollection AddServices(IServiceCollection services)
    {
        return services;
    }

    /// <summary>
    /// 应用模块服务
    /// </summary>
    /// <param name="app">应用程序构建器</param>
    public virtual void UsePack(IApplicationBuilder app)
    {
        IsEnabled = true;
    }

    /// <summary>
    /// 获取当前模块的依赖模块类型
    /// </summary>
    /// <returns></returns>
    internal Type[] GetDependModuleTypes()
    {
        DependsOnPacksAttribute depends = this.GetType().GetAttribute<DependsOnPacksAttribute>();
        return depends == null ? new Type[0] : depends.DependedModuleTypes;
    }
}
```
### <a id="011"/>基类OSharpPack定义说明
* `PackLevel`的属性，用于控制模块的启动级别
```
/// <summary>
/// 模块级别，级别越核心，优先启动
/// </summary>
public enum PackLevel
{
    /// <summary>
    /// 核心级别，表示系统的核心模块，
    /// 这些模块不涉及第三方组件，在系统运行中是不可替换的，核心模块将始终加载
    /// </summary>
    Core = 1,
    /// <summary>
    /// 框架级别，表示涉及第三方组件的基础模块
    /// </summary>
    Framework = 10,
    /// <summary>
    /// 应用级别，表示涉及应用数据的基础模块
    /// </summary>
    Application = 20,
    /// <summary>
    /// 业务级别，表示涉及真实业务处理的模块
    /// </summary>
    Business = 30
}
```
* `Order`数值属性，用于控制同一`PackLevel`级别内的模块启动顺序
  > `PackLevel`和`Order`两个属性结合使用，解决**模块启动顺序依赖**的问题。
* `AddServices`虚方法，用于向Pack模块中添加依赖注入服务映射，此方法将在启动类`Startup`的`ConfigureServices(IServiceCollection services)`方法内执行。
* `UsePack`方法，用于模块的初始化启动操作，此方法将在启动类`Startup`的`Configure(IApplicationBuilder app)`方法内执行。

### <a id="012"/>依赖模块配置特性 DependsOnPacksAttribute
如果一个Pack对于别一个Pack有着明显的依赖关系，可以通过特性`[DependsOnPacks(typeof(TBeDependOnPack))]`进行配置，例如：`SqlServerEntityFrameworkCorePack`显然是依赖于`EntityFrameworkCorePack`的，则进行如下配置，当在加载模块`SqlServerEntityFrameworkCorePack`的时候，系统会查找到基依赖模块`EntityFrameworkCorePack`进行加载。
```
/// <summary>
/// SqlServerEntityFrameworkCore模块
/// </summary>
[DependsOnPacks(typeof(EntityFrameworkCorePack))]
public class SqlServerEntityFrameworkCorePack : OsharpPack
{
    ...
}
```

## <a id="02"/>模块管理初始化

Pack模块的查找，加载，初始化等工作，都是由模块管理器[`OSharpPackManager`](http://docs.osharp.org/api/OSharp.Core.Packs.OSharpPackManager.html)执行的。

### <a id="021"/>OSharpPackManager执行流程
* 加载模块
    * 通过模块查找器`OSharpPackTypeFinder`，从程序集中查找出所有派生自模块基类`OSharpPack`的Pack模块类型
    * 如果初始化时指定只加载某些模块，则只加载这些模块
    > 要只加载某些模块，可通过 框架构建器[`IOSharpBuilder`](http://docs.osharp.org/api/OSharp.Core.Builders.IOSharpBuilder.html) 的`AddPack<TPack>()` 方法进行一个或多个模块的加载。
    * 不指定只加载某些模块，则默认加载所有查找出来的模块，并排除某些模块
    > 要排除某些模块，可通过 框架构建器[`IOSharpBuilder`](http://docs.osharp.org/api/OSharp.Core.Builders.IOSharpBuilder.html) 的`ExceptPack<TPack>()` 方法进行一个或多个模块的排除。
    * 对模块进行`PackLevel`和`Order`的排序
    * 按顺序执行各个`Pack`的`pack.AddServices(services)`方法，使用传入的`IServiceCollection`参数加载各个模块的依赖注入服务
* 应用模块
    * 按顺序执行各个`Pack`的`pack.UsePack(app)`，使用传入的`IApplicationBuilder`参数执行各个模块的初始化业务

### <a id="022"/>Pack初始化流程图
