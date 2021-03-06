# OSharp Framework

[![Member project of .NET Core Community](https://img.shields.io/badge/member%20project%20of-NCC-9e20c9.svg)](https://github.com/dotnetcore)
[![depoly action](https://github.com/dotnetcore/OSharp/workflows/CI/badge.svg)](https://github.com/dotnetcore/OSharp/actions?query=workflow%3A%22CI%22)
[![NuGet Badge](https://buildstats.info/nuget/osharpns.core)](https://www.nuget.org/packages/osharpns/)
[![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/i66soft/osharp-ns20/master/LICENSE)

---

 - [OSharpNS简介](#01)
 - [OSharpNS特性](#02)
 - [快速开始](#03)
 - [项目进度](#04)
 - [更新记录](https://github.com/i66soft/osharp/releases)
 - [代码生成器VSIX插件](https://marketplace.visualstudio.com/items?itemName=LiuliuSoft.osharp)
 - [OSharp文档中心](https://docs.osharp.org)

---

## <a id="01"/>OSharp简介

OSharpNS 全称 OSharp Framework with .NetStandard2.x，是一个基于.NetStandard2.x开发的一个.NetCore快速开发框架。这个框架使用最新稳定版的.NetCore SDK（当前是.NET Core 3.1），对 AspNetCore 的配置、依赖注入、日志、缓存、实体框架、Mvc(WebApi)、身份认证、权限授权等模块进行更高一级的自动化封装，并规范了一套业务实现的代码结构与操作流程，使 .Net Core 框架更易于应用到实际项目开发中。

相关示例项目：
- Vue版本(vben): [https://github.com/i66soft/osharp-vben](https://github.com/i66soft/osharp-vben)
- Mvc版本(layui): [https://github.com/i66soft/osharp-layui](https://github.com/i66soft/osharp-layui)
- Blazor版本(BootstrapBlazor): [https://github.com/i66soft/osharp-blazor](https://github.com/i66soft/osharp-blazor)

### 框架组件组织

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0007.png)

* OSharp【框架核心组件】：框架的核心组件，包含一系列快速开发中经常用到的Utility辅助工具功能，框架各个组件的核心接口定义，部分核心功能的实现
* OSharp.AspNetCore【AspNetCore组件】：AspNetCore组件，提供AspNetCore的服务端功能的封装
* OSharp.Authorization.Datas 【OSharp 数据权限组件】：OSharp 数据权限组件，对应用中数据权限进行授权的设计实现
* OSharp.Authorization.Functions【OSharp 功能权限组件】：OSharp 功能权限组件，API功能权限授权的设计实现
* OSharp.AutoMapper【对象映射组件】：AutoMapper 对象映射组件，封装基于AutoMapper的对象映射实现
* OSharp.EntityFrameworkCore【EFCore 数据组件】：EFCore数据访问组件，封装EntityFrameworkCore数据访问功能的实现
* OSharp.EntityFrameworkCore.MySql【EFCore MySql 数据组件】：EFCore MySql数据访问组件，封装MySql的EntityFrameworkCore数据访问功能的实现
* OSharp.EntityFrameworkCore.SqlServer【EFCore SqlServer 数据组件】：EFCore SqlServer数据访问组件，封装SqlServer的EntityFrameworkCore数据访问功能的实现
* OSharp.EntityFrameworkCore.Sqlite【EFCore Sqlite 数据组件】：EFCore Sqlite数据访问组件，封装Sqlite的EntityFrameworkCore数据访问功能的实现
* OSharp.EntityFrameworkCore.PostgreSql【EFCore PostgreSql 数据组件】：EFCore PostgreSql数据访问组件，封装PostgreSql的EntityFrameworkCore数据访问功能的实现
* OSharp.EntityFrameworkCore.Oracle【EFCore PostgreSql 数据组件】：EFCore Oracle数据访问组件，封装Oracle的EntityFrameworkCore数据访问功能的实现
* OSharp.Hangfire【后台任务组件】：封装基于Hangfire后台任务的服务端实现
* OSharp.Identity【身份认证组件】：使用AspNetCore的Identity为基础实现身份认证的封装
* OSharp.IdentityServer【OSharp IdentityServer组件】:OSharp IdentityServer组件，基于IdentityServer4提供身份认证与客户端授权的实现
* OSharp.IdentityServer.EntityConfiguration【OSharp IdentityServer EFCore存储配置组件】:OSharp IdentityServer EFCore存储配置组件，对IdentityServer4的存储进行EFCore映射配置
* OSharp.Log4Net【日志组件】：基于Log4Net的日志记录组件
* OSharp.MiniProfiler【MiniProfiler组件】：基于MiniProfiler实现的性能监测组件
* OSharpNS.NLog【OSharp NLog组件】:OSharp NLog组件，封装使用nlog组件来实现框架的日志输出功能
* OSharp.Redis【缓存组件】：基于Redis的分布式缓存客户端组件
* OSharp.Swagger【SwaggerAPI组件】：基于Swagger生成MVC的Action的API测试接口信息
* OSharp.Wpf【OSharp Wpf 客户端组件】：OSharp Wpf 客户端组件，封装Wpf客户端的辅助操作
* OSharp.Hosting.Core【OSharp框架非业务核心】：OSharp框架业务核心，封装框架非业务如认证，权限，系统，消息等模块的接口与业务实现
* OSharp.Hosting.EntityConfiguration【OSharp框架非业务实体映射】：OSharp框架非业务实体映射，封装框架非业务如认证，权限，系统，消息等模块的EFCore实体映射
* OSharp.Hosting.Apis【OSharp框架非业务WebAPI实现】：OSharp框架非业务WebAPI实现，封装框架非业务如认证，权限，系统，消息等模块的WebApi实现

### Nuget Packages
| 包名称                                                         | Nuget稳定版本                                                                                                       | Nuget预览版本                                                                                                          | 下载数                                                                                                               |
|----------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------|
| [OSharpNS.Core](https://www.nuget.org/packages/OSharpNS.Core/) | [![OSharpNS.Core](https://img.shields.io/nuget/v/OSharpNS.Core.svg)](https://www.nuget.org/packages/OSharpNS.Core/) | [![OSharpNS.Core](https://img.shields.io/nuget/vpre/OSharpNS.Core.svg)](https://www.nuget.org/packages/OSharpNS.Core/) | [![OSharpNS.Core](https://img.shields.io/nuget/dt/OSharpNS.Core.svg)](https://www.nuget.org/packages/OSharpNS.Core/) |
| [OSharpNS.AspNetCore](https://www.nuget.org/packages/OSharpNS.AspNetCore/)                                         | [![OSharpNS.AspNetCore](https://img.shields.io/nuget/v/OSharpNS.AspNetCore.svg)](https://www.nuget.org/packages/OSharpNS.AspNetCore/)     | [![OSharpNS.AspNetCore](https://img.shields.io/nuget/vpre/OSharpNS.AspNetCore.svg)](https://www.nuget.org/packages/OSharpNS.AspNetCore/)                                                             | [![OSharpNS.AspNetCore](https://img.shields.io/nuget/dt/OSharpNS.AspNetCore.svg)](https://www.nuget.org/packages/OSharpNS.AspNetCore/)                                                             |  |
| [OSharpNS.Authorization.Datas](https://www.nuget.org/packages/OSharpNS.Authorization.Datas/)                                       | [![OSharpNS.Authorization.Datas](https://img.shields.io/nuget/v/OSharpNS.Authorization.Datas.svg)](https://www.nuget.org/packages/OSharpNS.Authorization.Datas/)     | [![OSharpNS.Authorization.Datas](https://img.shields.io/nuget/vpre/OSharpNS.Authorization.Datas.svg)](https://www.nuget.org/packages/OSharpNS.Authorization.Datas/)                                                        | [![OSharpNS.Authorization.Datas](https://img.shields.io/nuget/dt/OSharpNS.Authorization.Datas.svg)](https://www.nuget.org/packages/OSharpNS.Authorization.Datas/)                                                          |
| [OSharpNS.Authorization.Functions](https://www.nuget.org/packages/OSharpNS.Authorization.Functions/)                                       | [![OSharpNS.Authorization.Functions](https://img.shields.io/nuget/v/OSharpNS.Authorization.Functions.svg)](https://www.nuget.org/packages/OSharpNS.Authorization.Functions/)     | [![OSharpNS.Authorization.Functions](https://img.shields.io/nuget/vpre/OSharpNS.Authorization.Functions.svg)](https://www.nuget.org/packages/OSharpNS.Authorization.Functions/)                                                        | [![OSharpNS.Authorization.Functions](https://img.shields.io/nuget/dt/OSharpNS.Authorization.Functions.svg)](https://www.nuget.org/packages/OSharpNS.Authorization.Functions/)                                                          |
| [OSharpNS.AutoMapper](https://www.nuget.org/packages/OSharpNS.AutoMapper/)                                         | [![OSharpNS.AutoMapper](https://img.shields.io/nuget/v/OSharpNS.AutoMapper.svg)](https://www.nuget.org/packages/OSharpNS.AutoMapper/)      | [![OSharpNS.AutoMapper](https://img.shields.io/nuget/vpre/OSharpNS.AutoMapper.svg)](https://www.nuget.org/packages/OSharpNS.AutoMapper/)                                                            | [![OSharpNS.AutoMapper](https://img.shields.io/nuget/dt/OSharpNS.AutoMapper.svg)](https://www.nuget.org/packages/OSharpNS.AutoMapper/)                                                             |  |
| [OSharpNS.EntityFrameworkCore](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore/)                       | [![OSharpNS.EntityFrameworkCore](https://img.shields.io/nuget/v/OSharpNS.EntityFrameworkCore.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore/)  | [![OSharpNS.EntityFrameworkCore](https://img.shields.io/nuget/vpre/OSharpNS.EntityFrameworkCore.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore/)                                 | [![OSharpNS.EntityFrameworkCore](https://img.shields.io/nuget/dt/OSharpNS.EntityFrameworkCore.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore/)                                  |
| [OSharpNS.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.SqlServer/)   | [![OSharpNS.EntityFrameworkCore.SqlServer](https://img.shields.io/nuget/v/OSharpNS.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.SqlServer/) | [![OSharpNS.EntityFrameworkCore.SqlServer](https://img.shields.io/nuget/vpre/OSharpNS.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.SqlServer/)    | [![OSharpNS.EntityFrameworkCore.SqlServer](https://img.shields.io/nuget/dt/OSharpNS.EntityFrameworkCore.SqlServer.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.SqlServer/)    |
| [OSharpNS.EntityFrameworkCore.MySql](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.MySql/)           | [![OSharpNS.EntityFrameworkCore.MySql](https://img.shields.io/nuget/v/OSharpNS.EntityFrameworkCore.MySql.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.MySql/)  | [![OSharpNS.EntityFrameworkCore.MySql](https://img.shields.io/nuget/vpre/OSharpNS.EntityFrameworkCore.MySql.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.MySql/)                | [![OSharpNS.EntityFrameworkCore.MySql](https://img.shields.io/nuget/dt/OSharpNS.EntityFrameworkCore.MySql.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.MySql/)                |
| [OSharpNS.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Sqlite/)         | [![OSharpNS.EntityFrameworkCore.Sqlite](https://img.shields.io/nuget/v/OSharpNS.EntityFrameworkCore.Sqlite.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Sqlite/)   | [![OSharpNS.EntityFrameworkCore.Sqlite](https://img.shields.io/nuget/vpre/OSharpNS.EntityFrameworkCore.Sqlite.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Sqlite/)            | [![OSharpNS.EntityFrameworkCore.Sqlite](https://img.shields.io/nuget/dt/OSharpNS.EntityFrameworkCore.Sqlite.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Sqlite/)             |
| [OSharpNS.EntityFrameworkCore.PostgreSql](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.PostgreSql/) | [![OSharpNS.EntityFrameworkCore.PostgreSql](https://img.shields.io/nuget/v/OSharpNS.EntityFrameworkCore.PostgreSql.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.PostgreSql/) | [![OSharpNS.EntityFrameworkCore.PostgreSql](https://img.shields.io/nuget/vpre/OSharpNS.EntityFrameworkCore.PostgreSql.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.PostgreSql/) | [![OSharpNS.EntityFrameworkCore.PostgreSql](https://img.shields.io/nuget/dt/OSharpNS.EntityFrameworkCore.PostgreSql.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.PostgreSql/) |
| [OSharpNS.EntityFrameworkCore.Oracle](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Oracle/)         | [![OSharpNS.EntityFrameworkCore.Oracle](https://img.shields.io/nuget/v/OSharpNS.EntityFrameworkCore.Oracle.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Oracle/) | [![OSharpNS.EntityFrameworkCore.Oracle](https://img.shields.io/nuget/vpre/OSharpNS.EntityFrameworkCore.Oracle.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Oracle/)            | [![OSharpNS.EntityFrameworkCore.Oracle](https://img.shields.io/nuget/dt/OSharpNS.EntityFrameworkCore.Oracle.svg)](https://www.nuget.org/packages/OSharpNS.EntityFrameworkCore.Oracle/)             |
| [OSharpNS.Hangfire](https://www.nuget.org/packages/OSharpNS.Hangfire/)                                             | [![OSharpNS.Hangfire](https://img.shields.io/nuget/v/OSharpNS.Hangfire.svg)](https://www.nuget.org/packages/OSharpNS.Hangfire/)                                              | [![OSharpNS.Hangfire](https://img.shields.io/nuget/vpre/OSharpNS.Hangfire.svg)](https://www.nuget.org/packages/OSharpNS.Hangfire/)                                                                  | [![OSharpNS.Hangfire](https://img.shields.io/nuget/dt/OSharpNS.Hangfire.svg)](https://www.nuget.org/packages/OSharpNS.Hangfire/)                                                                   |
| [OSharpNS.Identity](https://www.nuget.org/packages/OSharpNS.Identity/)                                       | [![OSharpNS.Identity](https://img.shields.io/nuget/v/OSharpNS.Identity.svg)](https://www.nuget.org/packages/OSharpNS.Identity/)     | [![OSharpNS.Identity](https://img.shields.io/nuget/vpre/OSharpNS.Identity.svg)](https://www.nuget.org/packages/OSharpNS.Identity/)                                                        | [![OSharpNS.Identity](https://img.shields.io/nuget/dt/OSharpNS.Identity.svg)](https://www.nuget.org/packages/OSharpNS.Identity/)                                                          |
| [OSharpNS.IdentityServer](https://www.nuget.org/packages/OSharpNS.IdentityServer/) | [![OSharpNS.IdentityServer](https://img.shields.io/nuget/v/OSharpNS.IdentityServer.svg)](https://www.nuget.org/packages/OSharpNS.IdentityServer/) | [![OSharpNS.IdentityServer](https://img.shields.io/nuget/vpre/OSharpNS.IdentityServer.svg)](https://www.nuget.org/packages/OSharpNS.IdentityServer/) | [![OSharpNS.IdentityServer](https://img.shields.io/nuget/dt/OSharpNS.IdentityServer.svg)](https://www.nuget.org/packages/OSharpNS.IdentityServer/) |
| [OSharpNS.IdentityServer.EntityConfiguration](https://www.nuget.org/packages/OSharpNS.IdentityServer.EntityConfiguration/) | [![OSharpNS.IdentityServer.EntityConfiguration](https://img.shields.io/nuget/v/OSharpNS.IdentityServer.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharpNS.IdentityServer.EntityConfiguration/) | [![OSharpNS.IdentityServer.EntityConfiguration](https://img.shields.io/nuget/vpre/OSharpNS.IdentityServer.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharpNS.IdentityServer.EntityConfiguration/) | [![OSharpNS.IdentityServer.EntityConfiguration](https://img.shields.io/nuget/dt/OSharpNS.IdentityServer.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharpNS.IdentityServer.EntityConfiguration/) |
| [OSharpNS.Log4Net](https://www.nuget.org/packages/OSharpNS.Log4Net/)                                               | [![OSharpNS.Log4Net](https://img.shields.io/nuget/v/OSharpNS.Log4Net.svg)](https://www.nuget.org/packages/OSharpNS.Log4Net/)                               | [![OSharpNS.Log4Net](https://img.shields.io/nuget/vpre/OSharpNS.Log4Net.svg)](https://www.nuget.org/packages/OSharpNS.Log4Net/)                                                                    | [![OSharpNS.Log4Net](https://img.shields.io/nuget/dt/OSharpNS.Log4Net.svg)](https://www.nuget.org/packages/OSharpNS.Log4Net/)                                                                      |
| [OSharpNS.MiniProfiler](https://www.nuget.org/packages/OSharpNS.MiniProfiler/)                                     | [![OSharpNS.MiniProfiler](https://img.shields.io/nuget/v/OSharpNS.MiniProfiler.svg)](https://www.nuget.org/packages/OSharpNS.MiniProfiler/)                                    | [![OSharpNS.MiniProfiler](https://img.shields.io/nuget/vpre/OSharpNS.MiniProfiler.svg)](https://www.nuget.org/packages/OSharpNS.MiniProfiler/)                                                      | [![OSharpNS.MiniProfiler](https://img.shields.io/nuget/dt/OSharpNS.MiniProfiler.svg)](https://www.nuget.org/packages/OSharpNS.MiniProfiler/)                                                       |
| [OSharpNS.Redis](https://www.nuget.org/packages/OSharpNS.Redis/)                                                   | [![OSharpNS.Redis](https://img.shields.io/nuget/v/OSharpNS.Redis.svg)](https://www.nuget.org/packages/OSharpNS.Redis/)                                                              | [![OSharpNS.Redis](https://img.shields.io/nuget/vpre/OSharpNS.Redis.svg)](https://www.nuget.org/packages/OSharpNS.Redis/)                                                                           | [![OSharpNS.Redis](https://img.shields.io/nuget/dt/OSharpNS.Redis.svg)](https://www.nuget.org/packages/OSharpNS.Redis/)                                                                            |
| [OSharpNS.Exceptionless](https://www.nuget.org/packages/OSharpNS.Exceptionless/)                                   | [![OSharpNS.Exceptionless](https://img.shields.io/nuget/v/OSharpNS.Exceptionless.svg)](https://www.nuget.org/packages/OSharpNS.Exceptionless/)                             | [![OSharpNS.Exceptionless](https://img.shields.io/nuget/vpre/OSharpNS.Exceptionless.svg)](https://www.nuget.org/packages/OSharpNS.Exceptionless/)                                                   | [![OSharpNS.Exceptionless](https://img.shields.io/nuget/dt/OSharpNS.Exceptionless.svg)](https://www.nuget.org/packages/OSharpNS.Exceptionless/)                                                    |
| [OSharpNS.Swagger](https://www.nuget.org/packages/OSharpNS.Swagger/)                                               | [![OSharpNS.Swagger](https://img.shields.io/nuget/v/OSharpNS.Swagger.svg)](https://www.nuget.org/packages/OSharpNS.Swagger/)                                       | [![OSharpNS.Swagger](https://img.shields.io/nuget/vpre/OSharpNS.Swagger.svg)](https://www.nuget.org/packages/OSharpNS.Swagger/)                                                                      | [![OSharpNS.Swagger](https://img.shields.io/nuget/dt/OSharpNS.Swagger.svg)](https://www.nuget.org/packages/OSharpNS.Swagger/)                                                                      |
| [OSharpNS.Wpf](https://www.nuget.org/packages/OSharpNS.Wpf/) | [![OSharpNS.Wpf](https://img.shields.io/nuget/v/OSharpNS.Wpf.svg)](https://www.nuget.org/packages/OSharpNS.Wpf/) | [![OSharpNS.Wpf](https://img.shields.io/nuget/vpre/OSharpNS.Wpf.svg)](https://www.nuget.org/packages/OSharpNS.Wpf/) | [![OSharpNS.Wpf](https://img.shields.io/nuget/dt/OSharpNS.Wpf.svg)](https://www.nuget.org/packages/OSharpNS.Wpf/) |
| [OSharpNS.Hosting.Core](https://www.nuget.org/packages/OSharpNS.Hosting.Core/) | [![OSharpNS.Hosting.Core](https://img.shields.io/nuget/v/OSharpNS.Hosting.Core.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.Core/) | [![OSharpNS.Hosting.Core](https://img.shields.io/nuget/vpre/OSharpNS.Hosting.Core.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.Core/) | [![OSharpNS.Hosting.Core](https://img.shields.io/nuget/dt/OSharpNS.Hosting.Core.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.Core/) |
| [OSharpNS.Hosting.EntityConfiguration](https://www.nuget.org/packages/OSharpNS.Hosting.EntityConfiguration/) | [![OSharpNS.Hosting.EntityConfiguration](https://img.shields.io/nuget/v/OSharpNS.Hosting.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.EntityConfiguration/) | [![OSharpNS.Hosting.EntityConfiguration](https://img.shields.io/nuget/vpre/OSharpNS.Hosting.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.EntityConfiguration/) | [![OSharpNS.Hosting.EntityConfiguration](https://img.shields.io/nuget/dt/OSharpNS.Hosting.EntityConfiguration.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.EntityConfiguration/) |
| [OSharpNS.Hosting.Apis](https://www.nuget.org/packages/OSharpNS.Hosting.Apis/) | [![OSharpNS.Hosting.Apis](https://img.shields.io/nuget/v/OSharpNS.Hosting.Apis.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.Apis/) | [![OSharpNS.Hosting.Apis](https://img.shields.io/nuget/vpre/OSharpNS.Hosting.Apis.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.Apis/) | [![OSharpNS.Hosting.Apis](https://img.shields.io/nuget/dt/OSharpNS.Hosting.Apis.svg)](https://www.nuget.org/packages/OSharpNS.Hosting.Apis/) |
| [OSharpNS](https://www.nuget.org/packages/OSharpNS/)                                                               | [![OSharpNS](https://img.shields.io/nuget/v/OSharpNS.svg)](https://www.nuget.org/packages/OSharpNS/)                                                                                   | [![OSharpNS](https://img.shields.io/nuget/vpre/OSharpNS.svg)](https://www.nuget.org/packages/OSharpNS/)                                                                                              | [![OSharpNS](https://img.shields.io/nuget/dt/OSharpNS.svg)](https://www.nuget.org/packages/OSharpNS/)                                                                                              |
| [OSharpNS.Template.Mvc_Angular](https://www.nuget.org/packages/OSharpNS.Template.Mvc_Angular/)                     | [![OSharpNS.Template.Mvc_Angular](https://img.shields.io/nuget/v/OSharpNS.Template.Mvc_Angular.svg)](https://www.nuget.org/packages/OSharpNS.Template.Mvc_Angular/)      |                            | [![OSharpNS.Template.Mvc_Angular](https://img.shields.io/nuget/dt/OSharpNS.Template.Mvc_Angular.svg)](https://www.nuget.org/packages/OSharpNS.Template.Mvc_Angular/)  

## <a id="02"/>OSharpNS特性

### 1. 模块化的组件设计

框架设计了一个模块（[Pack](http://docs.osharp.org/api/OSharp.Core.Packs.html)）的系统，所有实现了模块基类（[OsharpPack](http://docs.osharp.org/api/OSharp.Core.Packs.OsharpPack.html)）的类都视为一个独立的模块，一个模块可以独立添加服务（AddServices），并可在初始化时应用服务（UsePack）进行模块初始化。

### 2. 自动化的依赖注入机制

框架定义了`ISingletonDependency`，`IScopeDependency`，`ITransientDependency`三个空接口对应DependencyInjection中的三种服务生命周期，系统初始化时，通过反射检索程序集的方式，检索出所有服务类型(ServiceType)与服务实现(ImplementationType)及生命周期类型(ServiceLifetime)的相关数据，对依赖注入的ServiceCollection进行全自动初始化。

### 3. UnitOfWork-Repository模式，EFCore上下文动态构建

* 数据模块使用了`UnitOfWork-Repository`的模式来设计，设计了一个泛型的实体仓储接口`IRepository<TEntity,TKey>`，避免每个实体都需实现一个仓储的繁琐操作。设计了`IUnitOfWork`接口来管理事务，通过UnitOfWork模式管理DbContext的创建，使同上下文类型同数据库连接字符串的上下文使用**相同DbConnection**对象来创建，达到多上下文的事务同步能力。

* 基于MVC的`ActionFilter`的`UnitOfWorkAttribute` AOP 事务自动提交，业务中不再需要关心事务的生命周期。
* 系统初始化时，通过反射检索程序集的方式，检索出各个实体与上下文的映射关系，向上下文中动态添加实体类来构建上下文类型，以达到上下文类型与业务实体解耦的目的。通过统一基类`EntityTypeConfigurationBase<TEntity, TKey>`的FluentAPI实体映射，自由配置每个实体与数据库映射的每一个细节。

### 4. 基于AspNetCore的Identity的身份认证设计系统

* 使用AspNetCore原生的用户身份认证框架，身份认证相关操作统一使用`UserManager<TUser>`，`RoleManager<TRole>`两个入口，保持了原生Identity的体系强大性与功能完整性。

* 重新设计了用户存储`UserStore`和角色存储`RoleStore`，使用框架内设计的`IRepository<TEntity,TKey>`数据仓储接口来实现对数据的仓储操作，使Identity**身份认证系统与框架完美结合**，避免了使用官方的`Microsoft.AspNetCore.Identity.EntityFrameworkCore`造成多个上下文或者被强制使用Identity上下文作为系统数据上下文来实现业务造成的尴尬。

### 5. 设计了一个强大的功能权限与数据权限的授权体系

* 从底层开始，自动收集了系统的所有业务点（[IFunction](http://docs.osharp.org/api/OSharp.Core.Functions.IFunction.html)）和数据实体（[IEntityInfo](http://docs.osharp.org/api/OSharp.Core.EntityInfos.IEntityInfo.html)），用于对系统的功能权限、数据权限、数据缓存、操作审计 等实用功能提供数据支持。

* 功能点`Function`与MVC的`Area/Controller/Action`一一对应，是功能权限的最小验证单位，基于功能点，可以配置：
    * 功能访问类型（匿名访问、登录访问、限定角色访问）
    * 功能的数据缓存时间及缓存过期方式（绝对过期、相对过期）
    * 是否开启操作审计（XXX人员XXX时间做了XXX操作）
    * 是否开启数据审计（操作引起的数据变化详情（新增、更新、删除））
* 数据实体`EntityInfo`与数据库中的各个数据实体一一对应，基于数据实体，可以配置：
    * 是否开启数据审计，与`Function`上的同配置级别不同，如果指定实体未开放审计，则不审计当前实体。
    * **[部分实现]** 数据权限，基于`角色 - 实体`的数据权限设计，通过配置实现 XXX角色是否有权访问XXX实体数据（的XX属性）
* 设计了一个树形结构的业务模块体系（[Module](http://docs.osharp.org/api/OSharp.Security.ModuleBase-1.html)），对应着后端向前端开放的操作点（菜单/按钮），一个模块可由一个或多个功能点构成，模块是对外开放的特殊功能点，是进行**角色/用户功能授权**的单位。把一个模块授权给角色，角色即拥有了一个或多个功能点的操作权限。
* #### 功能权限授权流程
    * **[自动]** 创建MVC的各个`Area/Controller/Action`的功能点`Function`信息，存储到数据库
    * **[自动]** 创建树形模块`Module`信息，并创建模块与功能点（一个或多个）的分配关系，存储到数据库
    * 将模块`Module`分配给角色`Role`
    * 将角色`Role`分配给用户`User`
    * 可将模块`Module`分配给用户`User`，解决特权问题
    * 这样用户即可根据拥有的角色，自动拥有模块对应着的所有功能点的功能权限 
* #### 功能权限验证流程
    * 系统初始化时，根据每个角色`Role`分配到的模块`Module`，自动初始化每个 `角色 Role - Function[] `的权限对应关系并缓存
    * 游客进入系统时，自动请求所有可匿名访问`FunctionAccessType.Anonymouse`的模块信息并缓存到浏览器，浏览器根据这个缓存的模块集合，对前端页面的各个操作点（菜单/按钮）进行是否隐藏/禁用的状态控制
    * 注册用户登录系统时，自动请求所有可执行（包括匿名的`FunctionAccessType.Anonymouse`、登录的`FunctionAccessType.Logined`、指定角色的`FunctionAccessType.RoleLimit`）的模块信息并缓存到浏览器，浏览器根据这个缓存的模块集合，对前端页面的各个操作点（菜单/按钮）进行是否隐藏/禁用的状态控制
    * 用户`User`执行一个功能点`Function`时，验证流程如下：
        * 功能点不存在时，返回404
        * 功能点被锁定时，返回423
        * 功能点可访问性为匿名`FunctionAccessType.Anonymouse`验证通过
        * 功能点可访问性为需要登录`FunctionAccessType.Logined`时，用户未登录，返回401，已登录则验证通过
        * 功能点可访问性为需要登录`FunctionAccessType.RoleLimit`时，流程如下：
            * 用户未登录，返回401
            * 逐个验证用户拥有的角色`Role`，根据角色从缓存中取出`Role-Function[]`缓存项，`Function[]`包含要验证的功能点时，验证通过
            * 由分配给用户的模块`Module`对应的功能点，获取到`User-Function[]`（并缓存），`Function[]`包含要验证的功能点时，验证通过
            * 验证未通过，返回403

* #### [部分实现] 数据权限授权流程
    * 基于 角色`Role`-实体`EntityInfo` 的一一对应关系，配置指定角色对指定数据实体的数据查询筛选规则，并持久化到数据库中
    * 数据查询筛选规则组成为 条件组`FilterGroup`和条件`FilterRule`，一个条件组 [FilterGroup](http://docs.osharp.org/api/OSharp.Filter.FilterGroup.html) 包含 一个或多个条件 [FilterRule](http://docs.osharp.org/api/OSharp.Filter.FilterRule.html) 和 一个或多个 条件组`FilterGroup`，这样就实现了条件组和条件的无限嵌套，能满足绝大多数数据筛选规则的组装需要，如下图：
    
    ![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0009.png)

* #### [部分实现] 数据权限验证流程
    * 系统初始化时，将所有`角色-实体`的数据筛选规则缓存到内存中
    * 进行数据查询的时候，根据当前用户的所有`角色 Role`和要查询的`实体 EntityInfo`，查找出所有配置的数据筛选规则`FilterGroup`，转换为数据查询表达式`Expression<Func<TEntity,bool>>`，各个角色的表达式之间使用`Or`逻辑进行组合
    * 将以上生成的`数据权限`数据查询表达式，使用`And`逻辑组合到用户的提交的查询条件生成的表达式中，得到最终的数据查询表达式，提交到数据库中进行数据查询，从而获得数据权限限制下的合法数据

## 6. 集成 Swagger 后端API文档系统

OSharpNS 快速启动模板的开发模式，集成了`Swagger` API 文档生成组件，更方便了前后端分离的开发模式中前后端开发人员的数据接口对接工作。基于`Swagger`的工作原理，API的输入输出都需使用`强类型`的数据类型，`Swagger`才能发挥更好的作用，而OSharpNS框架通过`AutoMapper`的`ProjectTo`对业务实体到输出DTO`IOutputDto`提供了自动映射功能，能有效减轻后端开发中数据对象属性映射的工作量。

## <a id="03"/> 快速启动

OSharpNS框架制作了一个基于`dotnet cli`命令行工具的快速启动模板，下面演示如何来使用这个模板快速创建一个基于OSharpNS框架的初始化项目。
### 1. 安装最新版本 `dotnetcore sdk`
OSharpNS当前版本（0.5.0-beta04）使用了 `dotnetcore` 当前最新版本 `2.2.4`，所以对应的 `dotnetcore sdk` 需要安装到对应版本 [>=v2.2.203](https://www.microsoft.com/net/download/windows)。
### 2. 安装OSharpNS的`dotnet new`项目模板
在任意空白目录，打开 `cmd`或`powershell` 命令行窗口，执行命令

> dotnet new -i OSharpNS.Template.Mvc_Angular

执行后，将能看到`osharp_xxx`系列的命令已安装到列表中

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0001.png)

### 3. 执行`osharp`命令，获取项目一键项目安装脚本

>dotnet new osharp

执行后，将得到一个名为`osharp.bat`的批处理脚本文件

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0002.png)

### 4. 运行脚本文件，生成项目初始化代码
直接执行`osharp.bat`脚本代码，将会提示 `请输入项目名称，推荐形如 “公司.项目”的模式：`，此名称将用作解决方案名称、工程名称起始部分、代码中的`namespace`起始部分。例如输入`Liuliu.Demo`，将生成如下代码结构：

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0003.png)

### 5. 用VS打开解决方案
打开解决方案后，各个工程之间的引用关系已配置好，osharp框架的类库已引用 nuget.org 上的相应版本，并将自动还原好。项目结构如图所示：

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0004.png)

#### 项目代码结构说明：
* Liuliu.Demo.Core： 业务核心工程，顶层文件夹以业务模块内聚，每个文件夹按职责划分文件夹，通常可包含传输对象`Dtos`、实体类型`Entities`、事件处理`Events`等，业务接口IXXXContract与业务实现IXXXService放在外边，如果文件数量多的话也可以建文件夹存放。
* Liuliu.Demo.EntityConfiguration： EFCore实体映射工程，用于配置各个业务实体映射到数据库的映射细节。文件夹也推荐按模块内聚。
* Liuliu.Demo.Web： 网站的Hosting项目，按常规方式使用即可

#### 项目启动配置
* 按实际环境修改配置文件`appsetting.Development.json`中的`OSharp:DbContexts:[SqlServer|MySql]`中的配置信息，`ConnectionString`为数据库连接串，`AutoMigrationEnabled`为是否开启自动迁移
* 如未开启`AutoMigrationEnabled`的自动迁移功能，还需要在`nuget 控制台`手动执行迁移操作

> Update-Database

* 配置好后，即可正常启动端口号为`7001`的项目，启动后开发模式将进入`Swagger`的后端Api接口的文档页。

### 6. Angular7的前端项目启动
前端项目使用了`ng-alain`和`kendoui`作为UI进行开发的，需要熟悉`nodejs`,`angular7`等技术。

#### 安装`NodeJS`，搭建前端技术环境
* 安装最新版本 NodeJS：angular7需要最新版本(node 10.x 和 npm 6.x 以上的版本)的 NodeJS，请到 [NodeJS官方网站](https://nodejs.org/en/) 下载最新版本的NodeJS进行安装。
* 设置npm的淘宝镜像仓库：由于npm的国外仓储会很慢，所以最好把npm仓库地址指定国内镜像，推荐淘宝镜像：

  > `npm config set registry https://registry.npm.taobao.org`

* 安装全局Angular/Cli：如果Angular/Cli没有安装，执行如下命令全局安装Angular
    Angular的快速启动，请参考[Angular官方文档](https://angular.cn/guide/quickstart)

  > npm install -g @angular/cli

* 下载安装 Visual Studio Code：前端最好用的IDE，[官方下载](https://code.visualstudio.com/)

#### 使用 VS Code 打开 Angular 前端项目

* 定位到项目的目录`src/ui/ng-alain`，在空白处点右键，使用 VS Code 打开项目，可看到如下结构：

![image](https://raw.githubusercontent.com/i66soft/docs_images/master/osharpns/Readme/0005.png)

* 按`Ctrl+Tab`快捷键，调出VS Code的命令行控制台，输入NodeJS包安装命令：
  > npm install

* 包安装完成后，输入项目启动命令：
  > npm start

此命令将会执行如下命令：`ng serve --port 4201 --proxy-config proxy.config.json --open`，其中`--proxy-config proxy.config.json`对前端项目发起的API请求进行了代理，所有以 `/api/`开头的请求，都会转发到服务端项目中进行处理，代理的实际配置如下：
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

## <a id="04"/>项目开发进度

截止到目前，OSharpNS 框架的完成程度已经很高了，计划中的功能点，均已得到较高水准的实现，具体功能点完成进度如下所示：

- [ ] **OSharpNS Framework**
    - [ ] OSharp
        - [x] 添加常用Utility辅助工具类
        - [x] 添加框架配置Options定义
        - [x] 定义Entity数据访问相关接口
        - [x] 定义依赖注入模块相关接口
        - [x] 定义并实现EventBus事件总线的设计
        - [x] 定义Mapper对象映射模块相关接口
        - [x] 定义实体信息EntityInfo及初始化，用于给各个实体进行数据日志审计配置及数据权限设计
        - [x] 定义功能点信息Function及初始化，用于收集各个业务功能点（如MVC的Action），用于对功能进行缓存配置、操作日志审计、功能权限设计
        - [x] 定义Permissions权限模块的相关接口
        - [x] 实现框架依赖注入服务启动入口，调用各个功能模块（Pack）添加各模块的服务映射
        - [x] 实现ServiceLocator服务定位模式的依赖注入对象的解析
    - [x] OSharp.EntityFrameworkCore
        - [x] 实现运行时上下文类型初始化及自动加载相关实体类型的功能
        - [x] 实现Repository仓储的数据存储功能
        - [x] 实现UnitOfWork的多上下文管理及同DbConnection的上下文事务同步
    - [x] OSharp.AutoMapper
        - [x] 不同的映射类型，通过实现`Profile`来实现映射注册
        - [x] 实现通过遍历程序集，查找实现了`IMapTuple`接口的`Profile`来自动注册映射策略
        - [x] 定义`MapToAttribute`，`MapFromAttribute`类型，用以标注Mapping的Source与Target类型，使用时在要映射的类型上标注如`[MapTo(typeof(TTarget))]`或`[MapFrom(typeof(TSource))]`特性，框架初始化时自动查找相应的类型进行CreateMap映射注册
    - [x] OSharp.AspNetCore
        - [x] AspNet
            - [x] 实现框架启动入口`app.UseOSharp()`，调用Pack模块管理器`OSharpPackManager`启动各个功能模块（OSharpPack）
            - [x] 实现基于当前请求的ServiceLocator的Scoped对象的解析
            - [x] 实现JSON请求的404处理中间件
            - [x] 实现JSON请求的异常信息到JSON操作结果与异常日志记录中间件
        - [x] MVC
              - [x] 添加Api专用控制器基类`ApiController`,`AreaApiController`
              - [x] 实现MVC功能点处理器
              - [x] 实现MVC业务模块处理器
              - [x] 实现基于MVC的功能权限AOP拦截验证
              - [x] 实现基于MVC的事务提交AOP拦截提交
        - [x] SignalR
    - [ ] OSharp.Permissions
        - [ ] 身份认证Identity
            - [x] 用户添加昵称`NickName`属性，并添加默认验证器
            - [x] 重写UserStore，RoleStore，使用现有IRepository进行数据存储
            - [x] 实现第三方OAuth2认证系统的整合
        - [x] 权限授权Security
            - [x] 功能权限
                - [x] 实现功能权限各个业务实体的数据存储
                - [x] 实现在系统初始化时，遍历反射程序集，自动初始化功能点、数据实体、业务模块等信息并持久化到数据库
                - [x] 实现系统初始化时，将功能点，数据实体，角色功能权限等信息缓存到内存中
                - [x] 实现`角色-功能点`，`用户-功能点`的功能权限验证
            - [x] 数据权限
                - [x] 实现`角色-实体`，`用户-实体`的数据权限配置
                - [x] 实现`角色-实体`，`用户-实体`的数据权限过滤
        - [x] 系统System
            - [x] 实现键值对数据字典功能
