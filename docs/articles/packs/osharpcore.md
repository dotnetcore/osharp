# 核心模块：OSharpCorePack

* 级别：PackLevel.Core
* 启动顺序：0
* 位置：OSharp.dll
---

`OSharpCorePack`模块是OSharp框架的核心模块，基职责如下：
- 初始化OSharp框架选项配置`OSharpOptions`的创建器`OSharpOptionsSetup`
> services.AddSingleton<IConfigureOptions<OSharpOptions>, OSharpOptionsSetup>();
- 初始化服务定位模式类`ServiceLocator`
> ServiceLocator.Instance.SetServiceCollection(services);
> ServiceLocator.Instance.SetApplicationServiceProvider(app.ApplicationServices);