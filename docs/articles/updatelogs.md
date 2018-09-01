# OSharp .NetStandard 更新记录

### 0.3.0-beta11
1. 后台管理的系统模块增加OsharpPack模块列表
2. 添加MySql的迁移类MySqlDefaultDbContextMigrationPack和MySqlDesignTimeDefaultDbContextFactory
3. 将MySql迁移模块基类由MySqlMigrationModuleBase重命名为MySqlMigrationPackBase
4. 添加在线用户信息提供者注入IOnlineUserProvider，同时OnlineUser添加一个字典属性ExtendData作为在线用户扩展数据存储，有需要可注入替换IOnlineUserProvider服务来实现在线用户的自定义扩展数据
5. 修复T4不能生成的问题。实体元数据的属性数据去掉virtual的属性
6. 添加字符串的单复数转换的字符串扩展方法
7. 给Pack模块添加中文名称描述
8. 添加设置信息ISetting 对键值数据的获取与存储
9. 添加设置模块的底层模型支持
10. 优化KeyValue的类命名获取方式
11. 添加 IdentityErrorDescriberZhHans 替换原有的IdentityErrorDescriber为Identity提供中文错误描述
12. 修复数据权限验证不走缓存的问题，修复读取数据时启用了缓存仍然有读取数据库数据库的问题
13. Repository添加GetFirst方法，简化查找符合条件的第一个数据
14. 角色设置模块时，只设置有引用了角色限制的功能的模块
15. 修复异步开启事务时报事务连接与上下文连接不匹配的问题
16. 重构数据组件工作单元UnitOfWork，详见 [#35](https://github.com/i66soft/osharp-ns20/issues/35)

### 0.3.0-beta10
1. 新增AspOsharpPack基类，用于基于AspNetCore环境的Pack模块，将AspNetCore与非AspNetCore环境的OsharpPack分开，添加IOsharpPackManager 接口用于管理模块管理器
2. 修复DistributedCacheExtensions缓存扩展方法中当功能缓存秒数设置为0时无法创建缓存的问题 更新T4模板
3. 修复ng项目前端页面的众多小问题
4. 判断数据权限时只判断有功能权限的角色数据权限。
5. 修复更新数据权限时导致数据权限缓存项操作类型使用为Read的问题
6. 修复数据权限信息更新时缓存不更新或不移除的问题。将System命名空间更名为Systems，以避免与.net的System命名空间冲突
7. 优化系统启动时间：对系统初始化时从程序集中提取的Module，Function，EntityInfo 进行签名对比，与上次相同则不进行数据库数据同步
8. EF Core 组件添加数据模型缓存，解决数据模型重复创建的问题
9. 将系统的键值字典KeyValueCouple更名为KeyValue
10. 给用户设置角色时，或者锁定用户角色分配时，移除相应的在线用户信息缓存，使用户角色即时生效

### 0.3.0-beta09
1. 修复 UserStore 中的 FindByLoginAsync当userId为null时会引发异常的问题
2. 支持Lazy<>的注入。将Repository的Query拆分为多个方法，解决查询不支持可选参数的问题。DbContextBase添加延迟加载代理LazyLoadingProxiesEnabled的开关
3. OSharp.dll增加中增加Entity，InputDto，OutputDto 类型元数据生成功能，可用于自动生成代码。增加T4生成TS列表组件的代码生成功能
4. 添加实体生成InputDto、OutputDto、EntityConfiguration类型的T4脚本
5. 更新OutputDto生成TS组件代码的流程，验证信息由相应的InputDto提供
6. 代码生成类型元数据支持可空属性支持，修复 T4 模板可空类型不使用可空属性类型的问题
7. IQueryable相关扩展方法去除TEntity必须是实体类的限制，现可用于任意IQueryable<T>数据源

### 0.3.0-beta08
1. 修复ModuleInfo提取时排序码不起作用的问题
2. 修复登录时用户信息重复Track的问题
3. 优化Repository的Update相关实现，使之按需更新
5. 修复EntityInfoHandler查找实体信息不正确的问题4. 
5. 增加一个基于Scoped生命周期的数据字典`ScopedDictionary`，用于在程序执行路径上下文传递Scoped的数据，如当前执行功能，当前用户，操作审计，数据审计等
6. 完善操作审计`AuditOperationEntry`与数据审计`AuditEntityEntry`的基础实现
    1. 添加操作审计AOP拦截器`AuditOperationAttribute`，用于获取正在执行的功能新建操作审计`AuditOperationEntry`信息
    2. 操作审计将存入`ScopedDictionary`字典中向下传递
    3. 数据层执行`context.SaveChanges`操作的时候，视`上下文选项、功能是否允许数据审计、实体是否允许数据审计`等条件决定是否允许数据审计，如都允许，获取操作中的数据变更`新增数据的所有属性及新值、更新数据的变化属性及变更值，删除数据的所有属性及原值`创建数据变更信息。
    4. 数据变更信息由`事件总线`的`AuditEntityEventHandler`处理，附加到`ScopedDictionary`中的操作审计中
    5. 操作审计拦截器`AuditOperationAttribute`在请求结束时执行审计信息的持久化。
    6. 在Demo中实现了基于数据库的审计信息存储`AuditDatabaseStore`
    7. 给操作审计添加操作系统，浏览器信息
    8. 完善审计功能，收集操作结果，耗时，返回消息等数据
7. UnitOfWork添加Rollback可用于两个事务间清理上一个事务的变更数据跟踪 

### 0.3.0-beta07
1. 优化模块数据初始化流程，自动生成分类性质的模块信息，分类性质的模块信息是指在Area模块与Controller模块之间的模块信息，如下的`Identity`模块就是个分类模块信息：
    ```
    namespace Liuliu.Demo.Web.Areas.Admin.Controllers
    {
        [ModuleInfo(Order = 1, Position = "Identity", PositionName = "身份认证模块")]
        [Description("管理-用户信息")]
        public class UserController : AdminApiController
        { }
    }
    ```
    将生成如下模块结构：
    > 根节点 - Root
    > > 管理 - Admin
    > > > **身份认证模块 - Identity**
    > > > > 用户信息 - User

### 0.3.0-beta05-06
1. 客户端代码移除部分多余布局文件
2. 整合模块，简化`Startup`的代码。调整Pack的`UsePack`参数类型，由原来的`IServiceProvider`变更为`IApplicationBuilder`。将Startup中的初始化代码规划到各个Pack中，简化Startup中的代码
3. 优化前台header样式，固定header不随滚动条滚动
4. 修复`UnitOfWork`查找上下文的时候不返回关联上下文存在时的问题
5. 将事务开启时机由原先的创建上下文对象即开启更改为提交保存前开启，减小事务范围，同时避免只读业务也开启事务
6. 更新事件总线初始化流程，添加事件处理器查找器`EventHandlerTypeFinder`进行事件总线初始化
7. 将依赖注入功能提取为`DependencyPack`模块
8. 提取MVC模块基类 MvcPackBase，AspNetCoreMvcPack还是需要在Hosting项目中定义，以方便调整启动顺序（MVC模块需要在Identity模块之后启动，否则Identity无法生效）
9. 将事务开启更改为由DbConnection对象来开启，DbContext只是使用此事务，不负责开启事务
10. 简化数据存储模块OSharp.EntityFrameworkCore的上下文获取与管理，优化数据存储模块的初始化机制
11. 修复未开启事务时提交事务出错的问题
12. 修复部分模块状态启动不正确的问题
13. 修复因IEntityConfigurationTypeFinder未初始化导致迁移时获取不到实体类的问题

### 0.3.0-beta04
1. `OSharp.Permissions`增加QQ登录的支持
2. 将后台列表页数据读取改为强类型`PageRequest`
3. 解决JWToken信息不同步的问题，简化JWToken只包含`UserId，UserName`的基本信息，用户其他信息使用`Identity/Profile`从在线用户缓存中进行获取
4. 更新ng-alain, @delon/xxx, ng-zorro-antd组件到当前最新版本
5. 修复 `DesignTimeDbContextFactoryBase` 泛型使用不正确的问题

### 0.3.0-beta02-03
1. 添加在线用户信息缓存功能，使用户权限变更即时生效
2. 引入 material 前端UI，作为前台UI库，完成header布局
3. 将GetExpression替换成GetDataFilterExpression，以启用数据权限过滤
4. 添加更新、删除操作的数据权限验证
5. 移除`IRepository`的`Entities`,`TrackEntities`属性，重新启用`Query`数据查询方法，并添加数据权限过滤开关
6. `Repository`的数据查询，更新，删除，自动整合数据权限配置的数据筛选过滤条件，顶层使用的数据，默认已经是数据权限过滤之后的数据源。
7. 添加`IDataAuthEnabled`接口，用于控制查询数据的`更新、删除`数据权限，以方便控制数据显示的操作按钮状态，当OutputDto实现此接口时，数据查询的时候，自动验证数据权限并设置数据的`更新、删除`两个数据权限状态。
8. 实现用户个人数据的数据权限控制，通过在实体中的用户编号属性上标注[UserFlag]特性，配置数据权限时设置@CurrentUserId的特定值，在翻译成查询表达式时，把@CurrentUserId值替换成当前用户编号，生成只属于当前用户的数据查询表达式，来判断数据权限
9. 更新 .net core 到 2.1.2，SDK需要更新到`2.1.302`版本方可运行

### 0.3.0-beta01
1. 完善框架初始化过程的日志输出
2. 优化appsettings.json的日志配置
3. 添加 `app_offline.htm` 文件，用于 WebDeploy 部属时解锁网站并接手请求
4. 重构优化各个模块的类型查找器的代码
5. 依赖注入添加 `MultipleDependencyAttribute` 特性支持一个接口多个实现类的映射
6. 添加`IAutoMapperConfiguration`接口用于`AutoMapper`的各个模块DTO的特殊对象映射配置
7. 添加数据权限的`EntityRole`存储实现
8. 完善 swagger api 返回类型
9. 添加数据权限的规则实体设计，存储，缓存，数据查询时自动附加规则实现数据权限控制
10. 模块处理器添加删除数据库中多余模块的功能。
11. 添加数据权限规则的缓存同步事件
12. 完成角色数据权限的数据筛选规则条件组嵌套界面的布局
13. 完成数据权限规则设计器的实体属性关联，属性值控件未实现
14. 完善数据权限更新功能
15. 将用户权限设置拆分成角色设置和模块设置
16. 优化管理列表的命令列显示控制
17. 优化管理页面 新增 和 更新 的编辑状态检查机制，修复无更新权限无法新增的问题，优化管理列表的操作列显示

### 0.2.1-beta06
1. 示例Hosting项目改名为 `Liuliu.Demo.Web`
2. 调整AspNetCore类库的部分类位置
3. 添加docs全自动生成工具
4. 前端向后端请求数据的URL由/api/开头改为api/
5. 添加图片验证码生成类与验证类，注册、重置密码、激活邮箱等功能启用验证码
6. `UnitOfWorkAttribute`添加提交标记防止重复提交
7. 添加`remoteInverse`指令，用于远程验证结果相反转的场景
8. 使用缓存代替Session存储验证码
9. 添加新工程`OSharp.Log4Net`，实现日志的`log4net`按日志级别输出不同文件

### 0.2.1-beta05
1. 开发模式中引入`Swagger`组件，用于生成WebApi的接口文档
2. 添加api文档生成命令行工具
3. 补充缺失的注释信息，添加docs Api文档生成工具，用于生成类似MSDN的API文档页面

### 0.2.1-beta04
1. 增加字典实体（`KeyValueCouple`），用于存储系统配置等键值对类型的数据
2. 取消用户表的种子数据，第一个注册的用户即为超级管理员用户
3. 修复登录按钮不可用的问题

### 0.2.1-beta02-03
1. 修正一些使用OSharp作为命名空间的类型，使之符合模板要求
2. 发布dotnetcli的模板nuget包：`OSharpNS.Template.Mvc_Angular`

### 0.2.0-beta01
1. 将IRepository中的`Query`，`TrackQuery`方法标记为过时，使用属性`Entities`，`TrackEntities`代替
2. 将代码中`Query()`的引用更新为`Entities`，`TrackQuery()`的引用更新为`TrackEntities`
3. ng-alain: 修复管理页面window按键事件触发多个标签页的事件的问题
4. 优化用户认证前端操作的操作结果页面，完成Identity的功能，优化Identity各大Component与Service的代码组织，Service返回AdResult
5. 完成后端对URL的功能权限检查，完善权限系统
6. ng-alain: 更新 ng-alain 组件到最新版本
7. 添加功能信息及功能权限缓存的刷新事件，完成前端菜单/按钮权限的功能权限设计
8. 优化在单例类型中 `IServiceProvider.CreateScope` 执行Scoped业务的代码，统一在`ServiceLocator`中执行
9. 完成`Root.Site.Identity`路径的前端按钮权限控制
10. Admin页面添加权限控制组件基类，完成后台 管理页面的权限控制
11. 修复后台添加了 `[ApiController]` 特性之后 kendo grid 引发 400 的问题
12. 添加 `[ModuleInfoAttribute]` 特性，用于标记组合到Module树中的Function。添加 [DependOnFunctionAttribute]  特性，用于设置Module的额外功能依赖。完善各个控制器中两个特性的设置
13. 将代码模块命名由`Module`改为`Pack`，以解决和业务模块Module实体命名混淆的问题
14. 添加Module数据的初始化功能
15. 完成初始化种子数据、自动生成模块树、并自动给模块分配功能依赖，现在可以顺利初始化系统并能流畅使用了

### 0.1.0-alpha10
1. `User`实体增加`IsLocked`属性
2. 完善角色、用户权限设置功能
3. 将`UnitOfAttribute`由全局Filter更改为Action的Filter。
4. `IDbContext`和`IRepository`执行Sql的扩展方法
5. 事件总线：区分同步事件和异步事件，同步事件同步执行，异步事件启动后就不管。添加用户登录业务实现
6. 添加用户注册与登录的业务实现。添加登录，注册的表单验证。完成注册后邮箱激活功能。完成身份认证的发送激活邮件，通过邮件重置密码功能。
7. 权限安全模块 添加功能权限验证功能实现
8. 用户模块映射添加Disabled字段，用于设置用户对模块的True白名单与False黑名单功能
9. 完成后端权限过滤
10. 添加数据权限相关实体基类
11. 更新 .net core 框架类库到 2.0 版本的最新版
12. 添加Node前端技术的NoFound和Exception处理中间件
13. 优化HTTP异常处理逻辑
14. 完成`JwtBearer`的`Token`生成和验证
15. kendo的请求的grid，treeview请求，添加jwt头信息
16. 添加ng-alain模板的ng示例项目，完成后面管理列表数据显示
17. 将admin下的模块添加admin-，以解决路由冲突的问题
18. 移除admin子模块名中的 admin-前缀，路由中使用 ../admin来区别前台路由
19. 升级 .net core 版本：2.0 -> 2.1

### 0.1.0-alpha09
1. 添加 `DistributedCache` 缓存操作扩展方法
2. 添加日志模块的文本文件日志输出实现
3. 更新`Function`的Name的取值方式
4. 实现OSharp-ns20对mysql的支持，添加MySql上下文处理层，修改test/web的codefirst数据库链接配置
5. `ServiceLocator`支持`Scoped`类型的对象解析
6. 添加Audit审计的EF实体数据及属性值变更值的审计信息提取，并发布相应的事件，外部可订阅获得数据审计信息
7. 解决EventBus遗留的同步反射执行EventHandler的性能问题，使用快速反射执行器(`FastInvokeHandler`)来执行EventHander，将执行由同步改为Task.Run异步执行，防止Handler中的执行阻塞正常业务的执行
8. 更新第三方库，引入`NSubstitute`作为单元测试mock框架
9. 优化EventBus代码组织，移除`IEventBus`接口，添加`IEventStore`存储接口，并实现`InMemoryEventStore`，用于存储`EventType`与`EventHandler`的关系
10. 前后端分离，后端使用webapi作为数据服务，前端使用ng-cli进行开发，使用ng server进行实时编译
11. 添加`Permission`组件的`Security`模块的基础实现
12. 添加权限安全模块的Module相关的Store
13. `OSharp.Reflection`命名空间 添加 Com组件 免注册调用
14. 添加同步锁辅助类 `SyncLocker`
15. 整理加密解密工具类，添加AES，移除DES，RSA跨平台兼容
16. 添加 通信传输加密类 `TransmissionCryptor` 的测试用例
17. 更新AspNetCore到 2.0.2
18. 将OSharp项目中命名空间`OSharp.Infrastructure`更改为`OSharp.Core`
19. 给框架添加模块功能，以`OSharpModule`为基类，完成每个模块的初始化工作
20. 添加Identity模块功能，替换原来的`AddOSharpIdentity`扩展方法来初始化Identity模块
21. 添加`OSharpBuilder`，用于手动控制Module是否加载，不使用则加载所有检索到的Module
22. 更新各实体基类，给属性添加`DisplayName`特性，用于实体信息模块收集各个属性的中文名称
23. 添加权限安全的模块引导。修复依赖注入中Singleton与Scoped生命周期，同一实现类有多个接口注册时，不同接口获取的实例不一致的问题
24. 模块添加模块级别`ModuleLevel`，用于控制启动顺序。
25. 添加appsettings.json配置文件读取类。
26. 添加自动迁移功能，程序启动时自动获取未提交的迁移记录提交迁移
27. 模块排序使用`级别Level+层次Order`的方式，先按级别排序，级别内部再按层次排序。
28. 将AspNetCore更新到2.0.6版本。修复`OSharpOptions`初始化不能正确读取OSharp节点的问题
29. `SqlServerEntityFrameworkCoreModule`重命名为`SqlServerDefaultDbContextMigrationModule`
30. 添加种子数据初始化模块 `SeedDataInitModule`，用于初始化系统运行的一些必需数据。对系统数据的更新与删除进行限制。修复一些CreateScope之后对scope.ServiceProvider使用不正确的问题。

### 0.1.0-alpha08
1. 将测试项目更改为 netcoreapp2.0 类型
2. 添加OSharpOptions选项类型，用于配置OSharp框架的选项
3. 添加用户身份认证 `AspNetCore Identity` 的Store实现
4. 完成`UserStore`和`RoleStore`的基类
5. 添加Identity实体的实体配置类
6. Identity实体添加必要唯一索引
7. `OSharp.EntityFrameworkCore`组件引入`EntityFramework.Plus.EFCore`实现批量删除，批量更新操作
8. 使用`[MapTo(typeof(TTarget))]`，`[MapFrom(typeof(TSource))]` 两个Attribute来标注Mapping的Source与Target的关联，移除原来的命名规则约定
9. 添加框架基础实体：实体信息类 的定义和初始化功能
10. 添加框架基础实体：功能点信息类 的定义与MVC功能点信息的初始化实现
11. 修复功能信息初始化错误的问题

### 0.1.0-alpha05-07
1. `OSharp.AspNetCore` 添加一些MVC周边辅助功能
2. 查找所有程序集功能，由遍历文件夹的方式改为遍历所有引用程序集的依赖关系来查找

### 0.1.0-alpha03-04
1. 将原OSharp.Utility工具组件合并到OSharp项目中，并添加单元测试
2. 添加查找器，类型查找器，程序集查找器的接口定义
3. 添加 `AsyncLock` 异步锁功能
4. 添加数据组件核心接口，添加EntityFrameworkCore的数据访问实现（未完成）
5. 完成默认上下文的设计，初始化操作，添加EF数据组件的测试项目
6. 数据组件 `OSharp.EntityFrameworkCore` 基本设计成功，动态上下文构建成功。
7. 完成 DependencyInjection 模块的服务类型自动注册功能，要依赖注入的服务，只要继承相应生命周期的空接口，框架初始化时，就能自动读取程序集获取服务类型及对应的接口类型，进行自动匹配注册
8. 添加EntityFrameworkCore组件的多上下文支持，以及同DbConnection连接对象的多上下文事务同步，不同连接对象的多上下文受EF Core本身限制，暂时无法事务同步。
9. 添加 `IgnoreDependencyAttribute` 特性，用于禁用指定接口或类型不参与依赖注入的`ServiceCollection`自动初始化
10. 完成 对象映射功能Mapper的设计及 `OSharp.AutoMapper` 初始化封装
