# OSharp Framework with .NETStandard2.0

[![osharp@66soft](https://img.shields.io/badge/66soft-OsharpCore-red.svg)](http://core.osharp.org)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/OSharpNS.svg)](https://www.nuget.org/packages/OSharpNS/)
[![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/i66soft/osharp-ns20/master/LICENSE)

---

 - [OSharpNS简介](#01)
 - [OSharpNS特性](#02)
 - [快速开始](#03)
 - [项目进度](#04)

## <a id="01"/>OSharpNS简介
OSharp Framework with .NetStandard2.0（OSharpNS）是[OSharp](https://github.com/i66soft/osharp)的以`.NetStandard2.0`为目标框架，在`AspNetCore`的现有组件 `Microsoft.Extensions.DependencyInjection`，`Microsoft.Extensions.Configuration`，
`Microsoft.Extensions.Logging`，`Microsoft.Extensions.Caching`等 上进行构建的快速开发框架。

### 框架组件组织
* 框架核心组件[OSharp.dll]：框架的核心组件，包含一系列快速开发中经常用到的Utility辅助工具功能，框架各个组件的核心接口定义，部分核心功能的实现。
* 数据组件[OSharp.EntityFrameworkCore.dll]：框架的数据存储功能的EntityFrameworkCore封装实现
* 数据组件-SqlServer[OSharp.EntityFrameworkCore.SqlServer.dll]：SqlServer数据库的使用封装实现
* 对象映射组件[OSharp.AutoMapper.dll]：InputDto，OutputDto对象与实体映射的AutoMapper封装实现
* 权限组件[OSharp.Permissions.dll]：使用AspNetCore的Identity为基础实现身份认证的封装，以Security为基础实现以角色-功能、用户-功能的功能权限实现，以角色-数据，用户-数据的数据权限的封装
* AspNetCore组件[OSharp.AspNetCore.dll]

## <a id="02"/>OSharpNS特性
### 1. 依赖注入功能的全自动初始化
框架定义了`ISingletonDependency`，`IScopeDependency`，`ITransientDependency`三个空接口对应DependencyInjection中的三种服务生命周期，系统初始化时，通过反射检索程序集的方式，检索出所有服务类型(ServiceType)与服务实现(ImplementationType)及生命周期类型(ServiceLifetime)的相关数据，对依赖注入的ServiceCollection进行全自动初始化。
### 2. EntityFrameworkCore数据上下文动态构建
通过对各个实体添加`IEntityTypeConfiguration<TEntity>`接口的实现类型（T4模板自动生成），系统初始化时，通过反射检索程序集的方式，检索出各个实体与上下文的映射关系，向上下文中动态添加实体类来构建上下文类型，以达到上下文类型与业务实体解耦的目的。
### 3. EntityFrameworkCore同`DbConnection`下的事务同步
通过UnitOfWork模式管理DbContext的创建，使同上下文类型同数据库连接字符串的上下文使用相同DbConnection对象来创建，达到多上下文的事务同步功能。（**注：由于EF Core2.0尚不支持`TransactionScope`的限制，不同库的多上下文事务同步尚未实现**）

## <a id="03"/>快速启动

### 1.引用nuget package

> Install-Package OSharpNS -Pre

### 2.向Startup.cs文件中添加如下代码
> services.AddOSharp();

添加位置如下：

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddOSharp();
    services.AddMvc();
}
```

### 3.数据库创建与迁移
数据库迁移主要通过`Nuget控制台`的Migration功能来完成，由于数据迁移功能需要一个完整的`DbContext`数据上下文类型，而OSharp的数据上下文(默认`OSharp.Entity.DefaultDbContext`)是运行时动态构建的，并不包含实体信息，不能直接用于数据迁移工作。因此需要添加一个“设计时`DesignTime`”的数据上下文来进行数据迁移。

#### 3.1在启动项目中（网站的Host）合适的位置添加如下两个类型

继承`DefaultDbContext`的设计时数据上下文类型：
```csharp
[IgnoreDependency]
public class SqlServerDesignTimeDefaultDbContext : DefaultDbContext
{
    public SqlServerDesignTimeDefaultDbContext(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
        : base(options, typeFinder)
    {
    }
}
```
继承`IDesignTimeDbContextFactory<TEntity>`的设计时数据上下文对象创建工厂
```csharp
public class SqlServerDesignTimeDefaultDbContextFactory : IDesignTimeDbContextFactory<DesignTimeDefaultDbContext>
{
    public SqlServerDesignTimeDefaultDbContext CreateDbContext(string[] args)
    {
        string connString = "数据库连接串";
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder<DefaultDbContext>();
        builder.UseSqlServer(connString);//数据库类型
        IEntityConfigurationTypeFinder typeFinder = new EntityConfigurationTypeFinder(new EntityConfigurationAssemblyFinder(new AppAllAssemblyFinder()));
        return new DesignTimeDefaultDbContext(builder.Options, typeFinder);
    }
}
```

#### 3.2 数据迁移命令，Add-Migration 命令时，使用`-context`参数指定上下文类型

> Add-Migration MigrationName -context "OSharp.Demo.Web.SqlServerDesignTimeDefaultDbContext"

> Update-Database -context OSharp.Demo.Web.SqlServerDesignTimeDefaultDbContext

### 4.示例项目：OSharp.Demo.Web

示例项目是使用`asp.net core mvc` + `vue` + `typescript`来构建的，`vue`和`typescript`使用`webpack`来进行编译打包，所以，需要安装`NodeJS`才能把项目运行起来

#### 4.1 安装NodeJS

打开 NodeJS网站下载安装即可，网址：[https://nodejs.org/en/](https://nodejs.org/en/)

#### 4.2 安装依赖JS包

所有第三方JS包都通过 NodeJS 的`npm`来进行安装，首先要先还原 npm 包，在`OSharp.Demo.Web`根目录下执行如下命令，所有JS包都会被下载到`node_modules`目录中

> npm install

#### 4.3 编译打包不大需要更新的第三方JS包

框架对那些平时不太需要更新的第三方JS包，比如 `jquery`, `bootstrap`, `vue` 等，进行了单独打包，避免每次前端编译都对这些包进行打包压缩，提高前端编译速度。对第三方引用包的打包配置文件为：`webpack.config.vendor.js`，当第一次运行，或者第三方包有更新时，可执行如下命令进行手动打包。在项目`OSharp.Demo.Web`目录下执行如下命令，输出目录为`wwwroot\dist`

> webpack --config webpack.config.vendor.js

#### 4.4 手动前端编译

框架启动时会进行前端编译，当然，也可以手动执行`webpack`进行编译，在`OSharp.Demo.Web`根目录执行如下命令，默认会使用`webpack.config.js`配置文件进行编译打包，输出文件为`wwwroot\dist\build.js`

> webpack

## <a id="04"/>项目开发进度

- [ ] **OSharpNS Framework**
    - [ ] OSharp
        - [x] 添加常用Utility辅助工具类
        - [x] 添加框架配置Options定义
        - [x] 定义Entity数据访问相关接口
        - [x] 定义依赖注入模块相关接口
        - [x] 实现依赖注入功能的ServiceCollection自动初始化
        - [x] 实现EventBus事件总线的设计
        - [x] 定义Mapper对象映射模块相关接口
        - [x] 定义实体信息EntityInfo及初始化，用于给各个实体进行数据日志审计配置及数据权限设计
        - [x] 定义功能点信息Function及初始化，用于收集各个业务功能点（如MVC的Action），用于对功能进行缓存配置、操作日志审计、功能权限设计
        - [ ] 定义Permissions权限模块的相关接口
    - [x] OSharp.EntityFrameworkCore
        - [x] 实现运行时上下文类型初始化及自动加载相关实体类型的功能
        - [x] 实现Repository仓储的数据存储功能        - 
        - [x] 实现UnitOfWork的多上下文管理及同DbConnection的上下文事务同步
    - [x] OSharp.AutoMapper
        - [x] 不同的映射类型，通过实现`Profile`来实现映射注册
        - [x] 实现通过遍历程序集，查找实现了`IMapTuple`接口的`Profile`来自动注册映射策略
        - [x] 定义`MapToAttribute`，`MapFromAttribute`类型，用以标注Mapping的Source与Target类型，使用时在要映射的类型上标注如`[MapTo(typeof(TTarget))]`或`[MapFrom(typeof(TSource))]`特性，框架初始化时自动查找相应的类型进行CreateMap映射注册
    - [ ] OSharp.AspNetCore
    - [ ] OSharp.Permissions
