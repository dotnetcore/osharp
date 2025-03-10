# OSharp Framework

[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![depoly action](https://img.shields.io/github/actions/workflow/status/dotnetcore/osharp/ci.yml
)](https://github.com/dotnetcore/osharp/actions/workflows/ci.yml)
[![NuGet Badge](https://buildstats.info/nuget/osharp.core)](https://www.nuget.org/packages/osharpns/)
[![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/i66soft/osharp-ns20/master/LICENSE)

---

-   [OSharp 简介](#01)
-   [OSharp 特性](#02)
-   [快速开始](#03)
-   [项目进度](#04)
-   [更新记录](https://github.com/i66soft/osharp/releases)
-   [代码生成器 VSIX 插件](https://marketplace.visualstudio.com/items?itemName=LiuliuSoft.osharp)
-   [OSharp 文档中心](https://docs.osharp.org)

---

## <span id="01">OSharp 简介</span>

OSharp是一个基于.Net6.0+开发的一个.Net快速开发框架。这个框架使用最新稳定版的.Net SDK（当前是.NET 6.0），对 AspNetCore 的配置、依赖注入、日志、缓存、实体框架、Mvc(WebApi)、身份认证、权限授权等模块进行更高一级的自动化封装，并规范了一套业务实现的代码结构与操作流程，使 .Net Core 框架更易于应用到实际项目开发中。

### 项目地址
-   Github: [https://github.com/dotnetcore/osharp](https://github.com/dotnetcore/osharp)
-   Gitee(镜像): [https://gitee.com/i66soft/osharp](https://gitee.com/i66soft/osharp)


### 相关示例项目：

-   Vue 版本(vben): 
    -   [https://github.com/zionLZH/osharp-vben-admin](https://github.com/zionLZH/osharp-vben-admin)
    -   [https://github.com/gmf520/osharp-vben-template](https://github.com/gmf520/osharp-vben-template)
-   Mvc 版本(layui): [https://github.com/gmf520/osharp-layui](https://github.com/gmf520/osharp-layui)
-   Angular 版本(ng-alain): [https://github.com/dotnetcore/osharp/tree/releases/net6/samples/web/ui-clients/ng-alain8](https://github.com/dotnetcore/osharp/tree/releases/net6/samples/web/ui-clients/ng-alain8)

### 框架组件组织

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0007.png)

-   OSharp【框架核心组件】：框架的核心组件，包含一系列快速开发中经常用到的 Utility 辅助工具功能，框架各个组件的核心接口定义，部分核心功能的实现
-   OSharp.AspNetCore【AspNetCore 组件】：AspNetCore 组件，提供 AspNetCore 的服务端功能的封装
-   OSharp.Authorization.Datas 【OSharp 数据权限组件】：OSharp 数据权限组件，对应用中数据权限进行授权的设计实现
-   OSharp.Authorization.Functions【OSharp 功能权限组件】：OSharp 功能权限组件，API 功能权限授权的设计实现
-   OSharp.AutoMapper【对象映射组件】：AutoMapper 对象映射组件，封装基于 AutoMapper 的对象映射实现
-   OSharp.EntityFrameworkCore【EFCore 数据组件】：EFCore 数据访问组件，封装 EntityFrameworkCore 数据访问功能的实现
-   OSharp.EntityFrameworkCore.MySql【EFCore MySql 数据组件】：EFCore MySql 数据访问组件，封装 MySql 的 EntityFrameworkCore 数据访问功能的实现
-   OSharp.EntityFrameworkCore.SqlServer【EFCore SqlServer 数据组件】：EFCore SqlServer 数据访问组件，封装 SqlServer 的 EntityFrameworkCore 数据访问功能的实现
-   OSharp.EntityFrameworkCore.Sqlite【EFCore Sqlite 数据组件】：EFCore Sqlite 数据访问组件，封装 Sqlite 的 EntityFrameworkCore 数据访问功能的实现
-   OSharp.EntityFrameworkCore.PostgreSql【EFCore PostgreSql 数据组件】：EFCore PostgreSql 数据访问组件，封装 PostgreSql 的 EntityFrameworkCore 数据访问功能的实现
-   OSharp.EntityFrameworkCore.Oracle【EFCore PostgreSql 数据组件】：EFCore Oracle 数据访问组件，封装 Oracle 的 EntityFrameworkCore 数据访问功能的实现
-   OSharp.Hangfire【后台任务组件】：封装基于 Hangfire 后台任务的服务端实现
-   OSharp.Identity【身份认证组件】：使用 AspNetCore 的 Identity 为基础实现身份认证的封装
-   OSharp.IdentityServer【OSharp IdentityServer 组件】:OSharp IdentityServer 组件，基于 IdentityServer4 提供身份认证与客户端授权的实现
-   OSharp.IdentityServer.EntityConfiguration【OSharp IdentityServer EFCore 存储配置组件】:OSharp IdentityServer EFCore 存储配置组件，对 IdentityServer4 的存储进行 EFCore 映射配置
-   OSharp.Log4Net【日志组件】：基于 Log4Net 的日志记录组件
-   OSharp.MiniProfiler【MiniProfiler 组件】：基于 MiniProfiler 实现的性能监测组件
-   OSharp.NLog【OSharp NLog 组件】:OSharp NLog 组件，封装使用 nlog 组件来实现框架的日志输出功能
-   OSharp.Redis【缓存组件】：基于 Redis 的分布式缓存客户端组件
-   OSharp.Swagger【SwaggerAPI 组件】：基于 Swagger 生成 MVC 的 Action 的 API 测试接口信息
-   OSharp.Wpf【OSharp Wpf 客户端组件】：OSharp Wpf 客户端组件，封装 Wpf 客户端的辅助操作
-   OSharp.Hosting.Core【OSharp 框架非业务核心】：OSharp 框架业务核心，封装框架非业务如认证，权限，系统，消息等模块的接口与业务实现
-   OSharp.Hosting.EntityConfiguration【OSharp 框架非业务实体映射】：OSharp 框架非业务实体映射，封装框架非业务如认证，权限，系统，消息等模块的 EFCore 实体映射
-   OSharp.Hosting.Apis【OSharp 框架非业务 WebAPI 实现】：OSharp 框架非业务 WebAPI 实现，封装框架非业务如认证，权限，系统，消息等模块的 WebApi 实现

### Nuget Packages

|包名称|稳定版本|预览版本|下载数|
|----|----|----|----|
|[OSharp.Utils](https://www.nuget.org/packages/OSharp.Utils/)|[![OSharp.Utils](https://img.shields.io/nuget/v/OSharp.Utils.svg)](https://www.nuget.org/packages/OSharp.Utils/)|[![OSharp.Utils](https://img.shields.io/nuget/vpre/OSharp.Utils.svg)](https://www.nuget.org/packages/OSharp.Utils/)|[![OSharp.Utils](https://img.shields.io/nuget/dt/OSharp.Utils.svg)](https://www.nuget.org/packages/OSharp.Utils/)|
|[OSharp.Core](https://www.nuget.org/packages/OSharp.Core/)|[![OSharp.Core](https://img.shields.io/nuget/v/OSharp.Core.svg)](https://www.nuget.org/packages/OSharp.Core/)|[![OSharp.Core](https://img.shields.io/nuget/vpre/OSharp.Core.svg)](https://www.nuget.org/packages/OSharp.Core/)|[![OSharp.Core](https://img.shields.io/nuget/dt/OSharp.Core.svg)](https://www.nuget.org/packages/OSharp.Core/)|
|[OSharp.AspNetCore](https://www.nuget.org/packages/OSharp.AspNetCore/)|[![OSharp.AspNetCore](https://img.shields.io/nuget/v/OSharp.AspNetCore.svg)](https://www.nuget.org/packages/OSharp.AspNetCore/)|[![OSharp.AspNetCore](https://img.shields.io/nuget/vpre/OSharp.AspNetCore.svg)](https://www.nuget.org/packages/OSharp.AspNetCore/)|[![OSharp.AspNetCore](https://img.shields.io/nuget/dt/OSharp.AspNetCore.svg)](https://www.nuget.org/packages/OSharp.AspNetCore/)|
|[OSharp.Authorization.Datas](https://www.nuget.org/packages/OSharp.Authorization.Datas/)|[![OSharp.Authorization.Datas](https://img.shields.io/nuget/v/OSharp.Authorization.Datas.svg)](https://www.nuget.org/packages/OSharp.Authorization.Datas/)|[![OSharp.Authorization.Datas](https://img.shields.io/nuget/vpre/OSharp.Authorization.Datas.svg)](https://www.nuget.org/packages/OSharp.Authorization.Datas/)|[![OSharp.Authorization.Datas](https://img.shields.io/nuget/dt/OSharp.Authorization.Datas.svg)](https://www.nuget.org/packages/OSharp.Authorization.Datas/)|
|[OSharp.Authorization.Functions](https://www.nuget.org/packages/OSharp.Authorization.Functions/)|[![OSharp.Authorization.Functions](https://img.shields.io/nuget/v/OSharp.Authorization.Functions.svg)](https://www.nuget.org/packages/OSharp.Authorization.Functions/)|[![OSharp.Authorization.Functions](https://img.shields.io/nuget/vpre/OSharp.Authorization.Functions.svg)](https://www.nuget.org/packages/OSharp.Authorization.Functions/)|[![OSharp.Authorization.Functions](https://img.shields.io/nuget/dt/OSharp.Authorization.Functions.svg)](https://www.nuget.org/packages/OSharp.Authorization.Functions/)|
|[OSharp.AutoMapper](https://www.nuget.org/packages/OSharp.AutoMapper/)|[![OSharp.AutoMapper](https://img.shields.io/nuget/v/OSharp.AutoMapper.svg)](https://www.nuget.org/packages/OSharp.AutoMapper/)|[![OSharp.AutoMapper](https://img.shields.io/nuget/vpre/OSharp.AutoMapper.svg)](https://www.nuget.org/packages/OSharp.AutoMapper/)|[![OSharp.AutoMapper](https://img.shields.io/nuget/dt/OSharp.AutoMapper.svg)](https://www.nuget.org/packages/OSharp.AutoMapper/)|
|[OSharp.EntityFrameworkCore](https://www.nuget.org/packages/OSharp.EntityFrameworkCore/)|[![OSharp.EntityFrameworkCore](https://img.shields.io/nuget/v/OSharp.EntityFrameworkCore.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore/)|[![OSharp.EntityFrameworkCore](https://img.shields.io/nuget/vpre/OSharp.EntityFrameworkCore.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore/)|[![OSharp.EntityFrameworkCore](https://img.shields.io/nuget/dt/OSharp.EntityFrameworkCore.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore/)|
|[OSharp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.SqlServer/)|[![OSharp.EntityFrameworkCore.SqlServer](https://img.shields.io/nuget/v/OSharp.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.SqlServer/)|[![OSharp.EntityFrameworkCore.SqlServer](https://img.shields.io/nuget/vpre/OSharp.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.SqlServer/)|[![OSharp.EntityFrameworkCore.SqlServer](https://img.shields.io/nuget/dt/OSharp.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.SqlServer/)|
|[OSharp.EntityFrameworkCore.MySql](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.MySql/)|[![OSharp.EntityFrameworkCore.MySql](https://img.shields.io/nuget/v/OSharp.EntityFrameworkCore.MySql.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.MySql/)|[![OSharp.EntityFrameworkCore.MySql](https://img.shields.io/nuget/vpre/OSharp.EntityFrameworkCore.MySql.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.MySql/)|[![OSharp.EntityFrameworkCore.MySql](https://img.shields.io/nuget/dt/OSharp.EntityFrameworkCore.MySql.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.MySql/)|
|[OSharp.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.Sqlite/)|[![OSharp.EntityFrameworkCore.Sqlite](https://img.shields.io/nuget/v/OSharp.EntityFrameworkCore.Sqlite.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.Sqlite/)|[![OSharp.EntityFrameworkCore.Sqlite](https://img.shields.io/nuget/vpre/OSharp.EntityFrameworkCore.Sqlite.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.Sqlite/)|[![OSharp.EntityFrameworkCore.Sqlite](https://img.shields.io/nuget/dt/OSharp.EntityFrameworkCore.Sqlite.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.Sqlite/)|
|[OSharp.EntityFrameworkCore.PostgreSql](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.PostgreSql/)|[![OSharp.EntityFrameworkCore.PostgreSql](https://img.shields.io/nuget/v/OSharp.EntityFrameworkCore.PostgreSql.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.PostgreSql/)|[![OSharp.EntityFrameworkCore.PostgreSql](https://img.shields.io/nuget/vpre/OSharp.EntityFrameworkCore.PostgreSql.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.PostgreSql/)|[![OSharp.EntityFrameworkCore.PostgreSql](https://img.shields.io/nuget/dt/OSharp.EntityFrameworkCore.PostgreSql.svg)](https://www.nuget.org/packages/OSharp.EntityFrameworkCore.PostgreSql/)|
|[OSharp.Hangfire](https://www.nuget.org/packages/OSharp.Hangfire/)|[![OSharp.Hangfire](https://img.shields.io/nuget/v/OSharp.Hangfire.svg)](https://www.nuget.org/packages/OSharp.Hangfire/)|[![OSharp.Hangfire](https://img.shields.io/nuget/vpre/OSharp.Hangfire.svg)](https://www.nuget.org/packages/OSharp.Hangfire/)|[![OSharp.Hangfire](https://img.shields.io/nuget/dt/OSharp.Hangfire.svg)](https://www.nuget.org/packages/OSharp.Hangfire/)||[OSharp.Identity](https://www.nuget.org/packages/OSharp.Identity/)|[![OSharp.Identity](https://img.shields.io/nuget/v/OSharp.Identity.svg)](https://www.nuget.org/packages/OSharp.Identity/)|
|[OSharp.Identity](https://www.nuget.org/packages/OSharp.Identity/)|[![OSharp.Identity](https://img.shields.io/nuget/v/OSharp.Identity.svg)](https://www.nuget.org/packages/OSharp.Identity/)|[![OSharp.Identity](https://img.shields.io/nuget/vpre/OSharp.Identity.svg)](https://www.nuget.org/packages/OSharp.Identity/)|[![OSharp.Identity](https://img.shields.io/nuget/dt/OSharp.Identity.svg)](https://www.nuget.org/packages/OSharp.Identity/)|
|[OSharp.Log4Net](https://www.nuget.org/packages/OSharp.Log4Net/)|[![OSharp.Log4Net](https://img.shields.io/nuget/v/OSharp.Log4Net.svg)](https://www.nuget.org/packages/OSharp.Log4Net/)|[![OSharp.Log4Net](https://img.shields.io/nuget/vpre/OSharp.Log4Net.svg)](https://www.nuget.org/packages/OSharp.Log4Net/)|[![OSharp.Log4Net](https://img.shields.io/nuget/dt/OSharp.Log4Net.svg)](https://www.nuget.org/packages/OSharp.Log4Net/)|
|[OSharp.MiniProfiler](https://www.nuget.org/packages/OSharp.MiniProfiler/)|[![OSharp.MiniProfiler](https://img.shields.io/nuget/v/OSharp.MiniProfiler.svg)](https://www.nuget.org/packages/OSharp.MiniProfiler/)|[![OSharp.MiniProfiler](https://img.shields.io/nuget/vpre/OSharp.MiniProfiler.svg)](https://www.nuget.org/packages/OSharp.MiniProfiler/)|[![OSharp.MiniProfiler](https://img.shields.io/nuget/dt/OSharp.MiniProfiler.svg)](https://www.nuget.org/packages/OSharp.MiniProfiler/)|
|[OSharp.Redis](https://www.nuget.org/packages/OSharp.Redis/)|[![OSharp.Redis](https://img.shields.io/nuget/v/OSharp.Redis.svg)](https://www.nuget.org/packages/OSharp.Redis/)|[![OSharp.Redis](https://img.shields.io/nuget/vpre/OSharp.Redis.svg)](https://www.nuget.org/packages/OSharp.Redis/)|[![OSharp.Redis](https://img.shields.io/nuget/dt/OSharp.Redis.svg)](https://www.nuget.org/packages/OSharp.Redis/)|
|[OSharp.Exceptionless](https://www.nuget.org/packages/OSharp.Exceptionless/)|[![OSharp.Exceptionless](https://img.shields.io/nuget/v/OSharp.Exceptionless.svg)](https://www.nuget.org/packages/OSharp.Exceptionless/)|[![OSharp.Exceptionless](https://img.shields.io/nuget/vpre/OSharp.Exceptionless.svg)](https://www.nuget.org/packages/OSharp.Exceptionless/)|[![OSharp.Exceptionless](https://img.shields.io/nuget/dt/OSharp.Exceptionless.svg)](https://www.nuget.org/packages/OSharp.Exceptionless/)|
|[OSharp.Swagger](https://www.nuget.org/packages/OSharp.Swagger/)|[![OSharp.Swagger](https://img.shields.io/nuget/v/OSharp.Swagger.svg)](https://www.nuget.org/packages/OSharp.Swagger/)|[![OSharp.Swagger](https://img.shields.io/nuget/vpre/OSharp.Swagger.svg)](https://www.nuget.org/packages/OSharp.Swagger/)|[![OSharp.Swagger](https://img.shields.io/nuget/dt/OSharp.Swagger.svg)](https://www.nuget.org/packages/OSharp.Swagger/)|
|[OSharp.Wpf](https://www.nuget.org/packages/OSharp.Wpf/)|[![OSharp.Wpf](https://img.shields.io/nuget/v/OSharp.Wpf.svg)](https://www.nuget.org/packages/OSharp.Wpf/)|[![OSharp.Wpf](https://img.shields.io/nuget/vpre/OSharp.Wpf.svg)](https://www.nuget.org/packages/OSharp.Wpf/)|[![OSharp.Wpf](https://img.shields.io/nuget/dt/OSharp.Wpf.svg)](https://www.nuget.org/packages/OSharp.Wpf/)|
|[OSharp.Hosting.Core](https://www.nuget.org/packages/OSharp.Hosting.Core/)|[![OSharp.Hosting.Core](https://img.shields.io/nuget/v/OSharp.Hosting.Core.svg)](https://www.nuget.org/packages/OSharp.Hosting.Core/)|[![OSharp.Hosting.Core](https://img.shields.io/nuget/vpre/OSharp.Hosting.Core.svg)](https://www.nuget.org/packages/OSharp.Hosting.Core/)|[![OSharp.Hosting.Core](https://img.shields.io/nuget/dt/OSharp.Hosting.Core.svg)](https://www.nuget.org/packages/OSharp.Hosting.Core/)|
|[OSharp.Hosting.EntityConfiguration](https://www.nuget.org/packages/OSharp.Hosting.EntityConfiguration/)|[![OSharp.Hosting.EntityConfiguration](https://img.shields.io/nuget/v/OSharp.Hosting.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharp.Hosting.EntityConfiguration/)|[![OSharp.Hosting.EntityConfiguration](https://img.shields.io/nuget/vpre/OSharp.Hosting.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharp.Hosting.EntityConfiguration/)|[![OSharp.Hosting.EntityConfiguration](https://img.shields.io/nuget/dt/OSharp.Hosting.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharp.Hosting.EntityConfiguration/)|
|[OSharp.Hosting.Apis](https://www.nuget.org/packages/OSharp.Hosting.Apis/)|[![OSharp.Hosting.Apis](https://img.shields.io/nuget/v/OSharp.Hosting.Apis.svg)](https://www.nuget.org/packages/OSharp.Hosting.Apis/)|[![OSharp.Hosting.Apis](https://img.shields.io/nuget/vpre/OSharp.Hosting.Apis.svg)](https://www.nuget.org/packages/OSharp.Hosting.Apis/)|[![OSharp.Hosting.Apis](https://img.shields.io/nuget/dt/OSharp.Hosting.Apis.svg)](https://www.nuget.org/packages/OSharp.Hosting.Apis/)|
|[OSharpNS](https://www.nuget.org/packages/OSharpNS/)|[![OSharpNS](https://img.shields.io/nuget/v/OSharpNS.svg)](https://www.nuget.org/packages/OSharpNS/)|[![OSharpNS](https://img.shields.io/nuget/vpre/OSharpNS.svg)](https://www.nuget.org/packages/OSharpNS/)|[![OSharpNS](https://img.shields.io/nuget/dt/OSharpNS.svg)](https://www.nuget.org/packages/OSharpNS/)|
|[OSharp.Template.WebApi](https://www.nuget.org/packages/OSharp.Template.WebApi/)|[![OSharp.Template.WebApi](https://img.shields.io/nuget/v/OSharp.Template.WebApi.svg)](https://www.nuget.org/packages/OSharp.Template.WebApi/)|[![OSharp.Template.WebApi](https://img.shields.io/nuget/dt/OSharp.Template.WebApi.svg)](https://www.nuget.org/packages/OSharp.Template.WebApi/)|                                                          |

## <span id="02">OSharp 特性</span>

### 1. 模块化的组件设计

框架设计了一个模块（[Pack](http://docs.osharp.org/api/OSharp.Core.Packs.html)）的系统，所有实现了模块基类（[OsharpPack](http://docs.osharp.org/api/OSharp.Core.Packs.OsharpPack.html)）的类都视为一个独立的模块，一个模块可以独立添加服务（AddServices），并可在初始化时应用服务（UsePack）进行模块初始化。

### 2. 自动化的依赖注入机制

框架定义了`ISingletonDependency`，`IScopeDependency`，`ITransientDependency`三个空接口对应 DependencyInjection 中的三种服务生命周期，系统初始化时，通过反射检索程序集的方式，检索出所有服务类型(ServiceType)与服务实现(ImplementationType)及生命周期类型(ServiceLifetime)的相关数据，对依赖注入的 ServiceCollection 进行全自动初始化。

### 3. UnitOfWork-Repository 模式，EFCore 上下文动态构建

-   数据模块使用了`UnitOfWork-Repository`的模式来设计，设计了一个泛型的实体仓储接口`IRepository<TEntity,TKey>`，避免每个实体都需实现一个仓储的繁琐操作。设计了`IUnitOfWork`接口来管理事务，通过 UnitOfWork 模式管理 DbContext 的创建，使同上下文类型同数据库连接字符串的上下文使用**相同 DbConnection**对象来创建，达到多上下文的事务同步能力。

-   基于 MVC 的`ActionFilter`的`UnitOfWorkAttribute` AOP 事务自动提交，业务中不再需要关心事务的生命周期。
-   系统初始化时，通过反射检索程序集的方式，检索出各个实体与上下文的映射关系，向上下文中动态添加实体类来构建上下文类型，以达到上下文类型与业务实体解耦的目的。通过统一基类`EntityTypeConfigurationBase<TEntity, TKey>`的 FluentAPI 实体映射，自由配置每个实体与数据库映射的每一个细节。

### 4. 基于 AspNetCore 的 Identity 的身份认证设计系统

-   使用 AspNetCore 原生的用户身份认证框架，身份认证相关操作统一使用`UserManager<TUser>`，`RoleManager<TRole>`两个入口，保持了原生 Identity 的体系强大性与功能完整性。

-   重新设计了用户存储`UserStore`和角色存储`RoleStore`，使用框架内设计的`IRepository<TEntity,TKey>`数据仓储接口来实现对数据的仓储操作，使 Identity**身份认证系统与框架完美结合**，避免了使用官方的`Microsoft.AspNetCore.Identity.EntityFrameworkCore`造成多个上下文或者被强制使用 Identity 上下文作为系统数据上下文来实现业务造成的尴尬。

### 5. 设计了一个强大的功能权限与数据权限的授权体系

-   从底层开始，自动收集了系统的所有业务点（[IFunction](http://docs.osharp.org/api/OSharp.Core.Functions.IFunction.html)）和数据实体（[IEntityInfo](http://docs.osharp.org/api/OSharp.Core.EntityInfos.IEntityInfo.html)），用于对系统的功能权限、数据权限、数据缓存、操作审计 等实用功能提供数据支持。

-   功能点`Function`与 MVC 的`Area/Controller/Action`一一对应，是功能权限的最小验证单位，基于功能点，可以配置：
    -   功能访问类型（匿名访问、登录访问、限定角色访问）
    -   功能的数据缓存时间及缓存过期方式（绝对过期、相对过期）
    -   是否开启操作审计（XXX 人员 XXX 时间做了 XXX 操作）
    -   是否开启数据审计（操作引起的数据变化详情（新增、更新、删除））
-   数据实体`EntityInfo`与数据库中的各个数据实体一一对应，基于数据实体，可以配置：
    -   是否开启数据审计，与`Function`上的同配置级别不同，如果指定实体未开放审计，则不审计当前实体。
    -   **[部分实现]** 数据权限，基于`角色 - 实体`的数据权限设计，通过配置实现 XXX 角色是否有权访问 XXX 实体数据（的 XX 属性）
-   设计了一个树形结构的业务模块体系（[Module](http://docs.osharp.org/api/OSharp.Security.ModuleBase-1.html)），对应着后端向前端开放的操作点（菜单/按钮），一个模块可由一个或多个功能点构成，模块是对外开放的特殊功能点，是进行**角色/用户功能授权**的单位。把一个模块授权给角色，角色即拥有了一个或多个功能点的操作权限。
-   #### 功能权限授权流程
    -   **[自动]** 创建 MVC 的各个`Area/Controller/Action`的功能点`Function`信息，存储到数据库
    -   **[自动]** 创建树形模块`Module`信息，并创建模块与功能点（一个或多个）的分配关系，存储到数据库
    -   将模块`Module`分配给角色`Role`
    -   将角色`Role`分配给用户`User`
    -   可将模块`Module`分配给用户`User`，解决特权问题
    -   这样用户即可根据拥有的角色，自动拥有模块对应着的所有功能点的功能权限
-   #### 功能权限验证流程

    -   系统初始化时，根据每个角色`Role`分配到的模块`Module`，自动初始化每个 `角色 Role - Function[] `的权限对应关系并缓存
    -   游客进入系统时，自动请求所有可匿名访问`FunctionAccessType.Anonymouse`的模块信息并缓存到浏览器，浏览器根据这个缓存的模块集合，对前端页面的各个操作点（菜单/按钮）进行是否隐藏/禁用的状态控制
    -   注册用户登录系统时，自动请求所有可执行（包括匿名的`FunctionAccessType.Anonymouse`、登录的`FunctionAccessType.Logined`、指定角色的`FunctionAccessType.RoleLimit`）的模块信息并缓存到浏览器，浏览器根据这个缓存的模块集合，对前端页面的各个操作点（菜单/按钮）进行是否隐藏/禁用的状态控制
    -   用户`User`执行一个功能点`Function`时，验证流程如下：
        -   功能点不存在时，返回 404
        -   功能点被锁定时，返回 423
        -   功能点可访问性为匿名`FunctionAccessType.Anonymouse`验证通过
        -   功能点可访问性为需要登录`FunctionAccessType.Logined`时，用户未登录，返回 401，已登录则验证通过
        -   功能点可访问性为需要登录`FunctionAccessType.RoleLimit`时，流程如下：
            -   用户未登录，返回 401
            -   逐个验证用户拥有的角色`Role`，根据角色从缓存中取出`Role-Function[]`缓存项，`Function[]`包含要验证的功能点时，验证通过
            -   由分配给用户的模块`Module`对应的功能点，获取到`User-Function[]`（并缓存），`Function[]`包含要验证的功能点时，验证通过
            -   验证未通过，返回 403

-   #### [部分实现] 数据权限授权流程

    -   基于 角色`Role`-实体`EntityInfo` 的一一对应关系，配置指定角色对指定数据实体的数据查询筛选规则，并持久化到数据库中
    -   数据查询筛选规则组成为 条件组`FilterGroup`和条件`FilterRule`，一个条件组 [FilterGroup](http://docs.osharp.org/api/OSharp.Filter.FilterGroup.html) 包含 一个或多个条件 [FilterRule](http://docs.osharp.org/api/OSharp.Filter.FilterRule.html) 和 一个或多个 条件组`FilterGroup`，这样就实现了条件组和条件的无限嵌套，能满足绝大多数数据筛选规则的组装需要，如下图：

    ![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0009.png)

-   #### [部分实现] 数据权限验证流程
    -   系统初始化时，将所有`角色-实体`的数据筛选规则缓存到内存中
    -   进行数据查询的时候，根据当前用户的所有`角色 Role`和要查询的`实体 EntityInfo`，查找出所有配置的数据筛选规则`FilterGroup`，转换为数据查询表达式`Expression<Func<TEntity,bool>>`，各个角色的表达式之间使用`Or`逻辑进行组合
    -   将以上生成的`数据权限`数据查询表达式，使用`And`逻辑组合到用户的提交的查询条件生成的表达式中，得到最终的数据查询表达式，提交到数据库中进行数据查询，从而获得数据权限限制下的合法数据

## 6. 集成 Swagger 后端 API 文档系统

OSharp 快速启动模板的开发模式，集成了`Swagger` API 文档生成组件，更方便了前后端分离的开发模式中前后端开发人员的数据接口对接工作。基于`Swagger`的工作原理，API 的输入输出都需使用`强类型`的数据类型，`Swagger`才能发挥更好的作用，而 OSharp 框架通过`AutoMapper`的`ProjectTo`对业务实体到输出 DTO`IOutputDto`提供了自动映射功能，能有效减轻后端开发中数据对象属性映射的工作量。

## <span id="03">快速启动</span>

OSharp 框架制作了一个基于`dotnet cli`命令行工具的快速启动模板，下面演示如何来使用这个模板快速创建一个基于 OSharp 框架的初始化项目。

### 1. 安装最新版本 `dotnetcore sdk`

OSharp 当前版本（6.0.0）使用了 `.net` 当前最新版本 `6.0.0`，所以对应的 `net sdk` 需要安装到对应版本 [>=v6.0.0](https://www.microsoft.com/net/download/windows)。

### 2. 安装 OSharp 的`dotnet new`项目模板

在任意空白目录，打开 `cmd`或`powershell` 命令行窗口，执行命令

> dotnet new -i OSharp.Template.WebApi

执行后，将能看到`osharp_xxx`系列的命令已安装到列表中

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0001.png)

### 3. 执行`osharp`命令，获取项目一键项目安装脚本

> dotnet new osharp

执行后，将得到一个名为`osharp.bat`的批处理脚本文件

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0002.png)

### 4. 运行脚本文件，生成项目初始化代码

直接执行`osharp.bat`脚本代码，将会提示 `请输入项目名称，推荐形如 “公司.项目”的模式：`，此名称将用作解决方案名称、工程名称起始部分、代码中的`namespace`起始部分。例如输入`Liuliu.Demo`，将生成如下代码结构：

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0003.png)

### 5. 用 VS 打开解决方案

打开解决方案后，各个工程之间的引用关系已配置好，osharp 框架的类库已引用 nuget.org 上的相应版本，并将自动还原好。项目结构如图所示：

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0004.png)

#### 项目代码结构说明：

-   Liuliu.Demo.Core： 业务核心工程，顶层文件夹以业务模块内聚，每个文件夹按职责划分文件夹，通常可包含传输对象`Dtos`、实体类型`Entities`、事件处理`Events`等，业务接口 IXXXContract 与业务实现 IXXXService 放在外边，如果文件数量多的话也可以建文件夹存放。
-   Liuliu.Demo.EntityConfiguration： EFCore 实体映射工程，用于配置各个业务实体映射到数据库的映射细节。文件夹也推荐按模块内聚。
-   Liuliu.Demo.Web： 网站的 Hosting 项目，按常规方式使用即可

#### 项目启动配置

-   按实际环境修改配置文件`appsetting.Development.json`中的`OSharp:DbContexts:[SqlServer|MySql]`中的配置信息，`ConnectionString`为数据库连接串，`AutoMigrationEnabled`为是否开启自动迁移
-   如未开启`AutoMigrationEnabled`的自动迁移功能，还需要在`nuget 控制台`手动执行迁移操作

> Update-Database

-   配置好后，即可正常启动端口号为`7001`的项目，启动后开发模式将进入`Swagger`的后端 Api 接口的文档页。

### 6. Angular7 的前端项目启动

前端项目使用了`ng-alain`和`kendoui`作为 UI 进行开发的，需要熟悉`nodejs`,`angular7`等技术。

#### 安装`NodeJS`，搭建前端技术环境

-   安装最新版本 NodeJS：angular7 需要最新版本(node 10.x 和 npm 6.x 以上的版本)的 NodeJS，请到 [NodeJS 官方网站](https://nodejs.org/en/) 下载最新版本的 NodeJS 进行安装。
-   设置 npm 的淘宝镜像仓库：由于 npm 的国外仓储会很慢，所以最好把 npm 仓库地址指定国内镜像，推荐淘宝镜像：

    > `npm config set registry https://registry.npm.taobao.org`

-   安装全局 Angular/Cli：如果 Angular/Cli 没有安装，执行如下命令全局安装 Angular
    Angular 的快速启动，请参考[Angular 官方文档](https://angular.cn/guide/quickstart)

    > npm install -g @angular/cli

-   下载安装 Visual Studio Code：前端最好用的 IDE，[官方下载](https://code.visualstudio.com/)

#### 使用 VS Code 打开 Angular 前端项目

-   定位到项目的目录`src/ui/ng-alain`，在空白处点右键，使用 VS Code 打开项目，可看到如下结构：

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0005.png)

-   按`Ctrl+Tab`快捷键，调出 VS Code 的命令行控制台，输入 NodeJS 包安装命令：

    > npm install

-   包安装完成后，输入项目启动命令：

    > npm start

此命令将会执行如下命令：`ng serve --port 4201 --proxy-config proxy.config.json --open`，其中`--proxy-config proxy.config.json`对前端项目发起的 API 请求进行了代理，所有以 `/api/`开头的请求，都会转发到服务端项目中进行处理，代理的实际配置如下：

```
{
    "/api": {
        "target": "http://localhost:7001",
        "secure": false
    }
}
```

至此，项目启动完成，最终效果如下图所示：

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0006.png)

## <span id="04">项目开发进度</span>

截止到目前，OSharp 框架的完成程度已经很高了，计划中的功能点，均已得到较高水准的实现，具体功能点完成进度如下所示：

-   [ ] **OSharp Framework**
    -   [x] OSharp
        -   [x] 添加常用 Utility 辅助工具类
        -   [x] 添加框架配置 Options 定义
        -   [x] 定义 Entity 数据访问相关接口
        -   [x] 定义依赖注入模块相关接口
        -   [x] 定义并实现 EventBus 事件总线的设计
        -   [x] 定义 Mapper 对象映射模块相关接口
        -   [x] 定义实体信息 EntityInfo 及初始化，用于给各个实体进行数据日志审计配置及数据权限设计
        -   [x] 定义功能点信息 Function 及初始化，用于收集各个业务功能点（如 MVC 的 Action），用于对功能进行缓存配置、操作日志审计、功能权限设计
        -   [x] 定义 Permissions 权限模块的相关接口
        -   [x] 实现框架依赖注入服务启动入口，调用各个功能模块（Pack）添加各模块的服务映射
        -   [x] 实现 ServiceLocator 服务定位模式的依赖注入对象的解析
    -   [x] OSharp.EntityFrameworkCore
        -   [x] 实现运行时上下文类型初始化及自动加载相关实体类型的功能
        -   [x] 实现 Repository 仓储的数据存储功能
        -   [x] 实现 UnitOfWork 的多上下文管理及同 DbConnection 的上下文事务同步
        -   [x] 实现主从结构的数据读写分离
    -   [x] OSharp.AutoMapper
        -   [x] 不同的映射类型，通过实现`Profile`来实现映射注册
        -   [x] 实现通过遍历程序集，查找实现了`IMapTuple`接口的`Profile`来自动注册映射策略
        -   [x] 定义`MapToAttribute`，`MapFromAttribute`类型，用以标注 Mapping 的 Source 与 Target 类型，使用时在要映射的类型上标注如`[MapTo(typeof(TTarget))]`或`[MapFrom(typeof(TSource))]`特性，框架初始化时自动查找相应的类型进行 CreateMap 映射注册
    -   [x] OSharp.AspNetCore
        -   [x] AspNet
            -   [x] 实现框架启动入口`app.UseOSharp()`，调用 Pack 模块管理器`OSharpPackManager`启动各个功能模块（OSharpPack）
            -   [x] 实现基于当前请求的 ServiceLocator 的 Scoped 对象的解析
            -   [x] 实现 JSON 请求的 404 处理中间件
            -   [x] 实现 JSON 请求的异常信息到 JSON 操作结果与异常日志记录中间件
        -   [x] MVC - [x] 添加 Api 专用控制器基类`ApiController`,`AreaApiController` - [x] 实现 MVC 功能点处理器 - [x] 实现 MVC 业务模块处理器 - [x] 实现基于 MVC 的功能权限 AOP 拦截验证 - [x] 实现基于 MVC 的事务提交 AOP 拦截提交
        -   [x] SignalR
    -   [x] OSharp.Identity
        -   [x] 身份认证 Authentication
            -   [x] 实现用户 Claims 提供器`IUserClaimsProvider`
            -   [x] Cookie
                -   [x] 实现 Cookie 登录，并刷新在线用户信息
            -   [x] JwtBearer
                -   [x] 实现 Jwt Token 的构建功能
                -   [x] 实现 Jwt Token 的刷新机制
            -   [x] OAuth2
                -   [x] 支持 QQ、Github、MicroSoft、Google 等第三方登录，创建本地用户并关联
        -   [x] 身份标识 Identity
            -   [x] 用户添加昵称`NickName`属性，并添加默认验证器
            -   [x] 重写 UserStore，RoleStore，使用现有 IRepository 进行数据存储
            -   [x] 实现第三方 OAuth2 认证系统的整合
            -   [x] 在线用户信息缓存系统，实现用户信息刷新
    -   [x] OSharp.Authorization.Functions
        -   [x] 实现功能权限各个业务实体的数据存储
        -   [x] 实现在系统初始化时，遍历反射程序集，自动初始化功能点、数据实体、业务模块等信息并持久化到数据库
        -   [x] 实现系统初始化时，将功能点，数据实体，角色功能权限等信息缓存到内存中
        -   [x] 实现`角色-功能点`，`用户-功能点`的功能权限验证
    -   [x] OSharp.Authorization.Datas
        -   [x] 实现`角色-实体`，`用户-实体`的数据权限配置
        -   [x] 实现`角色-实体`，`用户-实体`的数据权限过滤

## 感谢
[![](https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1Mu3b?ver=5c31)](https://dotnet.microsoft.com/zh-cn/)
[![JetBrains Resharper](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)](https://www.jetbrains.com/resharper/)
