# OSharp Framework with .NETStandard2.0

标签（空格分隔）： osharp-ns

---

## OSharpNS简介
OSharp Framework with .NetStandard2.0（OSharpNS）是[OSharp](https://github.com/i66soft/osharp)的`.NetStandard2.0`版本，在`AspNetCore`的现有组件（`Microsoft.Extensions.Configuration`、`Microsoft.Extensions.Logging`、`Microsoft.Extensions.DependencyInjection`，`Microsoft.Extensions.Caching`等）上进行构建的快速开发框架。

### 框架组件组织

* 框架核心组件[OSharp.dll]：框架的核心组件，包含一系列快速开发中经常用到的Utility辅助工具功能，框架各个组件的核心接口定义，部分核心功能的实现。
* 数据组件[OSharp.EntityFrameworkCore.dll]：框架的数据存储功能的EntityFrameworkCore封装实现
* 数据组件-SqlServer[OSharp.EntityFrameworkCore.SqlServer.dll]：SqlServer数据库的使用封装实现
* 对象映射组件[OSharp.AutoMapper.dll]：InputDto，OutputDto对象与实体映射的AutoMapper封装实现
* 权限组件[OSharp.Permissions.dll]：使用AspNetCore的Identity为基础实现身份认证的封装，以Security为基础实现以角色-功能、用户-功能的功能权限实现，以角色-数据，用户-数据的数据权限的封装
* AspNetCore组件[OSharp.AspNetCore.dll]

## OSharpNS特性
计划中，OSharpNT将有如下特性：

#### 1. 依赖注入功能的全自动初始化
框架定义了`ISingletonDependency`，`IScopeDependency`，`ITransientDependency`三个空接口对应DependencyInjection中的三种服务生命周期，系统初始化时，通过反射检索程序集的方式，检索出所有服务类型(ServiceType)与服务实现(ImplementationType)及生命周期类型(ServiceLifetime)的相关数据，对依赖注入的ServiceCollection进行全自动初始化。
#### 2. EntityFrameworkCore数据上下文动态构建
通过对各个实体添加`IEntityTypeConfiguration<TEntity>`接口的实现类型（T4模板自动生成），系统初始化时，通过反射检索程序集的方式，检索出各个实体与上下文的映射关系，向上下文中动态添加实体类来构建上下文类型，以达到上下文类型与业务实体解耦的目的。
#### 3. EntityFrameworkCore同`DbConnection`下的事务同步
通过UnitOfWork模式管理DbContext的创建，使同上下文类型同数据库连接字符串的上下文使用相同DbConnection对象来创建，达到多上下文的事务同步功能。（**注：由于EF Core2.0尚不支持`TransactionScope`的限制，不同库的多上下文事务同步尚未实现**）

