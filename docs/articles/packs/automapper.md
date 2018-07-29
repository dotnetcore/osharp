# 对象映射模块：AutoMapperPack

* 级别：PackLevel.Framework
* 启动顺序：0
* 位置：OSharp.AutoMapper.dll

> - [对象映射模块组成](#对象映射模块组成)
>     - [对象映射关系自动配置](#对象映射关系自动配置)
>     - [映射执行](#映射执行)
> - [对象映射模块初始化](#对象映射模块初始化)
> - [对象映射模块使用示例](#对象映射模块使用示例)
>     - [简单同属性映射](#简单同属性映射)
>     - [复杂类型的属性映射](#复杂类型的属性映射)
  
---

AutoMapper模块是AutoMapper功能的自动化封装，使程序初始化时自动对`对象映射`的源类与目标类建立映射关系。

## 对象映射模块组成

### 对象映射关系自动配置
对象映射的映射源类型与映射目标类型自动配对机制
* 框架中定义了两个特性`MapFromAttribute`,`MapToAttribute`，分别用于标注`当前类映射自其他类`和`当前类映射到其他类`两种方向。
    * `[MapFrom(typeof(TSource))]`表示将源类型TSource类映射到当前类
    * `[MapTo(typeof(TTarget))]`表示将当前类映射到目标类型TTarget
* 框架中还定义了一个定义执行配对的接口`IMapTuple`，用于创建源类型与目标类型的配对
    ```
    /// <summary>
    /// 定义对象映射源与目标配对
    /// </summary>
    public interface IMapTuple
    {
    /// <summary>
    /// 执行对象映射构造
    /// </summary>
    void CreateMap();
    }
    ```
    在实现类型配对的时候，就可以通过类型查找器`IMapFromAttributeTypeFinder`和`IMapToAttributeTypeFinder`查找出所有映射类型组合，进行类型映射配对的创建。
* `AutoMapper`组装配对，都是通过`MapperConfigurationExpression.AddProfile(Profile profile)`方法进行的，因此要引入`Profile`类型来真正执行`CreateMap`操作，通过基类`Profile`的方法`CreateMap(TSource, TTarget)`来接手接口`IMapTuple`的`CreateMap()`方法的配对工作，顺利将配对工作转移到`Profile`类中进行，配对实现如下：
    ```
    /// <summary>
    /// 创建源类型与目标类型的配对
    /// </summary>
    public class MapTupleProfile : Profile, IMapTuple
    {
        private readonly IMapFromAttributeTypeFinder _mapFromAttributeTypeFinder;
        private readonly IMapToAttributeTypeFinder _mapToAttributeTypeFinder;
        private readonly ILogger<MapTupleProfile> _logger;

        /// <summary>
        /// 初始化一个<see cref="MapTupleProfile"/>类型的新实例
        /// </summary>
        public MapTupleProfile(
            IMapFromAttributeTypeFinder mapFromAttributeTypeFinder,
            IMapToAttributeTypeFinder mapToAttributeTypeFinder,
            ILoggerFactory loggerFactory)
        {
            _mapFromAttributeTypeFinder = mapFromAttributeTypeFinder;
            _mapToAttributeTypeFinder = mapToAttributeTypeFinder;
            _logger = loggerFactory.CreateLogger<MapTupleProfile>();
        }

        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        public void CreateMap()
        {
            List<(Type Source, Type Target)> tuples = new List<(Type Source, Type Target)>();

            Type[] types = _mapFromAttributeTypeFinder.FindAll(true);
            foreach (Type targetType in types)
            {
                MapFromAttribute attribute = targetType.GetAttribute<MapFromAttribute>();
                foreach (Type sourceType in attribute.SourceTypes)
                {
                    var tuple = ValueTuple.Create(sourceType, targetType);
                    tuples.AddIfNotExist(tuple);
                }
            }

            types = _mapToAttributeTypeFinder.FindAll(true);
            foreach (Type sourceType in types)
            {
                MapToAttribute attribute = sourceType.GetAttribute<MapToAttribute>();
                foreach (Type targetType in attribute.TargetTypes)
                {
                    var tuple = ValueTuple.Create(sourceType, targetType);
                    tuples.AddIfNotExist(tuple);
                }
            }

            foreach ((Type Source, Type Target) tuple in tuples)
            {
                CreateMap(tuple.Source, tuple.Target);
                _logger.LogDebug($"创建“{tuple.Source}”到“{tuple.Target}”的对象映射关系");
            }
            _logger.LogInformation($"创建{tuples.Count}个对象映射关系");
        }
    }
    ```
* 以上是自动化的对象映射配对工作，还需支持**手动配置**的配对工作，手动配置通过`MapperConfigurationExpression`类型的`CreateMap<TSource, TTarget>()`来进行，定义如下接口
    ```
    /// <summary>
    /// 定义通过<see cref="MapperConfigurationExpression"/>配置对象映射的功能
    /// </summary>
    [MultipleDependency]
    public interface IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        void CreateMaps(MapperConfigurationExpression mapper);
    }
    ```
    举例实现如下，目标类型`RoleNode`的`RoleName`属性来自源类型`Role`的`Name`属性：
    ```
    /// <summary>
    /// DTO对象映射配置
    /// </summary>
    public class AutoMapperConfiguration : IAutoMapperConfiguration, ISingletonDependency
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Role, RoleNode>().ForMember(rn => rn.RoleId, opt => opt.MapFrom(r => r.Id))
                .ForMember(rn => rn.RoleName, opt => opt.MapFrom(r => r.Name));
        }
    }
    ```

### 映射执行

配置好映射关系之后，对象的映射执行，是通过如下扩展方法来解析的，
```
/// <summary>
/// 对象映射扩展操作
/// </summary>
public static class MapperExtensions
{
    private static IMapper _mapper;

    /// <summary>
    /// 设置对象映射执行者
    /// </summary>
    /// <param name="mapper">映射执行者</param>
    public static void SetMapper(IMapper mapper)
    {
        mapper.CheckNotNull("mapper");
        _mapper = mapper;
    }

    /// <summary>
    /// 将对象映射为指定类型
    /// </summary>
    /// <typeparam name="TTarget">要映射的目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <returns>目标类型的对象</returns>
    public static TTarget MapTo<TTarget>(this object source)
    {
        CheckMapper();
        return _mapper.MapTo<TTarget>(source);
    }

    /// <summary>
    /// 使用源类型的对象更新目标类型的对象
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <param name="target">待更新的目标对象</param>
    /// <returns>更新后的目标类型对象</returns>
    public static TTarget MapTo<TSource, TTarget>(this TSource source, TTarget target)
    {
        CheckMapper();
        return _mapper.MapTo(source, target);
    }

    /// <summary>
    /// 将数据源映射为指定<typeparamref name="TOutputDto"/>的集合
    /// </summary>
    public static IQueryable<TOutputDto> ToOutput<TEntity, TOutputDto>(this IQueryable<TEntity> source,
        params Expression<Func<TOutputDto, object>>[] membersToExpand)
    {
        CheckMapper();
        return _mapper.ToOutput<TOutputDto>(source, membersToExpand);
    }

    /// <summary>
    /// 验证映射执行者是否为空
    /// </summary>
    private static void CheckMapper()
    {
        if (_mapper == null)
        {
            throw new NullReferenceException(Resources.Map_MapperIsNull);
        }
    }
}
```
扩展方法中有个`private static IMapper _mapper;`字段是映射解析的执行者，其定义如下
```
/// <summary>
/// 定义对象映射功能
/// </summary>
public interface IMapper
{
    /// <summary>
    /// 将对象映射为指定类型
    /// </summary>
    /// <typeparam name="TTarget">要映射的目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <returns>目标类型的对象</returns>
    TTarget MapTo<TTarget>(object source);

    /// <summary>
    /// 使用源类型的对象更新目标类型的对象
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <param name="target">待更新的目标对象</param>
    /// <returns>更新后的目标类型对象</returns>
    TTarget MapTo<TSource, TTarget>(TSource source, TTarget target);

    /// <summary>
    /// 将数据源映射为指定输出DTO的集合
    /// </summary>
    /// <typeparam name="TOutputDto">输出DTO类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="membersToExpand">成员展开</param>
    /// <returns>输出DTO的结果集</returns>
    IQueryable<TOutputDto> ToOutput<TOutputDto>(IQueryable source, params Expression<Func<TOutputDto, object>>[] membersToExpand);
}
```
在`AutoMapper`这个映射组件中，映射主要是通过`Mapper.Map<TTarget>(source)`和`Mapper.Map<TSource, TTarget>(source, target)`两个静态方法来执行的，简单实现如下：
```
/// <summary>
/// AutoMapper映射执行类
/// </summary>
public class AutoMapperMapper : IMapper
{
    /// <summary>
    /// 将对象映射为指定类型
    /// </summary>
    /// <typeparam name="TTarget">要映射的目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <returns>目标类型的对象</returns>
    public TTarget MapTo<TTarget>(object source)
    {
        return Mapper.Map<TTarget>(source);
    }

    /// <summary>
    /// 使用源类型的对象更新目标类型的对象
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <param name="target">待更新的目标对象</param>
    /// <returns>更新后的目标类型对象</returns>
    public TTarget MapTo<TSource, TTarget>(TSource source, TTarget target)
    {
        return Mapper.Map<TSource, TTarget>(source, target);
    }

    /// <summary>
    /// 将数据源映射为指定输出DTO的集合
    /// </summary>
    /// <typeparam name="TOutputDto">输出DTO类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="membersToExpand">成员展开</param>
    /// <returns>输出DTO的结果集</returns>
    public IQueryable<TOutputDto> ToOutput<TOutputDto>(IQueryable source, params Expression<Func<TOutputDto, object>>[] membersToExpand)
    {
        return source.ProjectTo(membersToExpand);
    }
}
```
> 上面还有个没说到的方法`ToOutput`，调用的是`AutoMapper`的`ProjectTo`扩展方法，`ProjectTo`是专门为数据查询提供的功能。例如在`EntityFrameworkCore`中，只需传入一定的查询结果类型`TOutputDto`，AutoMapper即可根据`TOutputDto`类型为数据查询生成只附带了必需实体属性的查询表达式进行数据查询。

> 备注：通过`ProjectTo`执行的数据映射，不需要进行数据映射配置。

## 对象映射模块初始化

* AutoMapper的映射配置，是靠一个`MapperConfigurationExpression`对象来完成的，因此需要在依赖注入服务中注册一个`MapperConfigurationExpression`对象
    ```
    services.AddSingleton(new MapperConfigurationExpression());
    ```
* 注册其他服务类型
    ```
    services.AddSingleton<IMapFromAttributeTypeFinder, MapFromAttributeTypeFinder>();
    services.AddSingleton<IMapToAttributeTypeFinder, MapToAttributeTypeFinder>();
    services.AddSingleton<IMapTuple, MapTupleProfile>();
    services.AddSingleton<IMapper, AutoMapperMapper>();
    ```
* 执行初始化的时候，首先从依赖注入服务中解析出`MapperConfigurationExpression`对象
    ```
    MapperConfigurationExpression cfg = provider.GetService<MapperConfigurationExpression>() 
        ?? new MapperConfigurationExpression();
    ```
* 注册各个手动编写配置类进行映射的复杂映射
    ```
    IAutoMapperConfiguration[] configs = provider.GetServices<IAutoMapperConfiguration>().ToArray();
    foreach (IAutoMapperConfiguration config in configs)
    {
        config.CreateMaps(cfg);
    }
    ```
* 注册标注了`MapFromAttribute`,`MapToAttribute`特性的简单映射
    ```
    IMapTuple[] tuples = provider.GetServices<IMapTuple>().ToArray();
    foreach (IMapTuple mapTuple in tuples)
    {
        mapTuple.CreateMap();
        cfg.AddProfile(mapTuple as Profile);
    }
    ```
* 完成初始化，将映射执行者`IMapper`的对象设置到映射扩展方法`MapperExtensions`中
    ```
    Mapper.Initialize(cfg);
    IMapper mapper = provider.GetService<IMapper>();
    MapperExtensions.SetMapper(mapper);
    ```

## 对象映射模块使用示例

### 简单同属性映射

如果源类型与目标类型只是简单的同属性映射，只需在目标类型标注[MapFrom(typeof(TSource))]特性，或者在源类型标注[MapTo(typeof(TTarget))]特性即可。
```
public class SourceType
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class TargetType
{
    public int Id { get; set; }

    public string Name { get; set; }
}
```
只需标注相应特性，都可以建立两个类型的映射关系
```
[MapTo(typeof(Target))]
public class Source
{ } 

或者

[MapFrom(typeof(Source))]
public class Target
{ }
```
执行映射：
* 由源类型对象创建目标类型对象
    ```
    Source source = ...;
    Target target = source.Map<Target>();
    ```
* 由源类型对象更新目标类型对象
    ```
    Source source = ...;
    Target target = ...;
    target = source.Map<Source, Target>(source, target);
    ```

### 复杂类型的属性映射

对于比较复杂的类型映射，比如属性值不同，属性类型不同，或者需要一定的逻辑来转换，都需要写映射配置才能正常映射。例如下面两个类，目标类型的`TargetName`来自于源类型的`SourceName`
```
public class Source
{
    public int Id { get; set; }

    public string SourceName { get; set; }
}

public class Target
{
    public int Id { get; set; }

    public string TargetName { get; set; }
}
```

需要实现`IAutoMapperConfiguration`接口，通过`ForMember`来处理这个映射，同时配置类需要实现`ISingletonDependency`，以正确注册到依赖注入服务，对象映射服务初始化时将从依赖注入中解析配置类实例进行注册。
```
/// <summary>
/// DTO对象映射配置
/// </summary>
public class AutoMapperConfiguration : IAutoMapperConfiguration, ISingletonDependency
{
    /// <summary>
    /// 创建对象映射
    /// </summary>
    /// <param name="mapper">映射配置表达</param>
    public void CreateMaps(MapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Source, Target>().ForMember(t => t.TargetName, opt => opt.MapFrom(s => s.SourceName));
    }
}
```
执行映射：
* 由源类型对象创建目标类型对象
    ```
    Source source = ...;
    Target target = source.Map<Target>();
    ```
* 由源类型对象更新目标类型对象
    ```
    Source source = ...;
    Target target = ...;
    target = source.Map<Source, Target>(source, target);
    ```