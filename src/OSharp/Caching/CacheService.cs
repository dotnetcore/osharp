// -----------------------------------------------------------------------
//  <copyright file="CacheService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-19 19:10</last-date>
// -----------------------------------------------------------------------

using System.Text.Json;

namespace OSharp.Caching;

/// <summary>
/// 缓存服务实现
/// </summary>
public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ICacheKeyGenerator _keyGenerator;
    private readonly IGlobalCacheKeyGenerator _globalKeyGenerator;
    private readonly ILogger<CacheService> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 初始化一个<see cref="CacheService"/>类型的新实例
    /// </summary>
    public CacheService(
        IDistributedCache cache,
        ICacheKeyGenerator keyGenerator,
        ILogger<CacheService> logger,
        IServiceProvider serviceProvider)
    {
        _cache = cache;
        _keyGenerator = keyGenerator;
        _globalKeyGenerator = keyGenerator as IGlobalCacheKeyGenerator;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    #region Implementation of ICacheService

    /// <summary>
    /// 查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据筛选表达式</param>
    /// <param name="pageCondition">分页条件</param>
    /// <param name="selector">数据投影表达式</param>
    /// <param name="cacheSeconds">缓存时间</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual PageResult<TResult> ToPageCache<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        PageCondition pageCondition,
        Expression<Func<TSource, TResult>> selector,
        int cacheSeconds = 60,
        params object[] keyParams)
    {
        string key = GetKey(source, predicate, pageCondition, selector, keyParams);
        return _cache.Get(key, () => source.ToPage(predicate, pageCondition, selector), cacheSeconds);
    }

    /// <summary>
    /// 查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据筛选表达式</param>
    /// <param name="pageCondition">分页条件</param>
    /// <param name="selector">数据投影表达式</param>
    /// <param name="function">当前功能信息</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual PageResult<TResult> ToPageCache<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        PageCondition pageCondition,
        Expression<Func<TSource, TResult>> selector,
        IFunction function,
        params object[] keyParams)
    {
        string key = GetKey(source, predicate, pageCondition, selector, keyParams);
        return _cache.Get(key, () => source.ToPage(predicate, pageCondition, selector), function);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="cacheSeconds">缓存时间：秒</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        Expression<Func<TSource, TResult>> selector,
        int cacheSeconds = 60,
        params object[] keyParams)
    {
        return ToCacheList(source.Where(predicate), selector, cacheSeconds, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="cacheSeconds">缓存时间：秒</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        Expression<Func<TSource, TResult>> selector,
        int cacheSeconds = 60,
        params object[] keyParams)
    {
        return ToCacheArray(source.Where(predicate), selector, cacheSeconds, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        Expression<Func<TSource, TResult>> selector,
        IFunction function,
        params object[] keyParams)
    {
        return ToCacheList(source.Where(predicate), selector, function, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        Expression<Func<TSource, TResult>> selector,
        IFunction function,
        params object[] keyParams)
    {
        return ToCacheArray(source.Where(predicate), selector, function, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="cacheSeconds">缓存的秒数</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> selector,
        int cacheSeconds = 60,
        params object[] keyParams)
    {
        string key = GetKey(source, selector, keyParams);
        return _cache.Get(key, () => source.Select(selector).ToList(), cacheSeconds);
    }

    /// <summary>
    /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="cacheSeconds">缓存的秒数</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> selector,
        int cacheSeconds = 60,
        params object[] keyParams)
    {
        string key = GetKey(source, selector, keyParams);
        return _cache.Get(key, () => source.Select(selector).ToArray(), cacheSeconds);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> selector,
        IFunction function,
        params object[] keyParams)
    {
        string key = GetKey(source, selector, keyParams);
        return _cache.Get(key, () => source.Select(selector).ToList(), function);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TResult">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="selector">数据筛选表达式</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> selector,
        IFunction function,
        params object[] keyParams)
    {
        string key = GetKey(source, selector, keyParams);
        return _cache.Get(key, () => source.Select(selector).ToArray(), function);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="cacheSeconds">缓存的秒数</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual List<TSource> ToCacheList<TSource>(IQueryable<TSource> source, int cacheSeconds = 60, params object[] keyParams)
    {
        string key = GetKey(source.Expression, keyParams);
        return _cache.Get(key, source.ToList, cacheSeconds);
    }

    /// <summary>
    /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="cacheSeconds">缓存的秒数</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual TSource[] ToCacheArray<TSource>(IQueryable<TSource> source, int cacheSeconds = 60, params object[] keyParams)
    {
        string key = GetKey(source.Expression, keyParams);
        return _cache.Get(key, source.ToArray, cacheSeconds);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual List<TSource> ToCacheList<TSource>(IQueryable<TSource> source, IFunction function, params object[] keyParams)
    {
        if (function == null || function.CacheExpirationSeconds <= 0)
        {
            return source.ToList();
        }

        string key = GetKey(source.Expression, keyParams);
        return _cache.Get(key, source.ToList, function);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual TSource[] ToCacheArray<TSource>(IQueryable<TSource> source, IFunction function, params object[] keyParams)
    {
        if (function == null || function.CacheExpirationSeconds <= 0)
        {
            return source.ToArray();
        }

        string key = GetKey(source.Expression, keyParams);
        return _cache.Get(key, source.ToArray, function);
    }

    /// <summary>
    /// 查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TOutputDto">分页数据类型</typeparam>
    /// <param name="source">要查询的数据集</param>
    /// <param name="predicate">查询条件谓语表达式</param>
    /// <param name="pageCondition">分页查询条件</param>
    /// <param name="cacheSeconds">缓存的秒数</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询的分页结果</returns>
    public virtual PageResult<TOutputDto> ToPageCache<TEntity, TOutputDto>(IQueryable<TEntity> source,
        Expression<Func<TEntity, bool>> predicate,
        PageCondition pageCondition,
        int cacheSeconds = 60,
        params object[] keyParams) where TOutputDto : IOutputDto
    {
        string key = GetKey<TEntity, TOutputDto>(source, predicate, pageCondition, keyParams);
        return _cache.Get(key, () => source.ToPage<TEntity, TOutputDto>(predicate, pageCondition), cacheSeconds);
    }

    /// <summary>
    /// 查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TOutputDto">分页数据类型</typeparam>
    /// <param name="source">要查询的数据集</param>
    /// <param name="predicate">查询条件谓语表达式</param>
    /// <param name="pageCondition">分页查询条件</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询的分页结果</returns>
    public virtual PageResult<TOutputDto> ToPageCache<TEntity, TOutputDto>(IQueryable<TEntity> source,
        Expression<Func<TEntity, bool>> predicate,
        PageCondition pageCondition,
        IFunction function,
        params object[] keyParams) where TOutputDto : IOutputDto
    {
        string key = GetKey<TEntity, TOutputDto>(source, predicate, pageCondition, keyParams);
        return _cache.Get(key, () => source.ToPage<TEntity, TOutputDto>(predicate, pageCondition), function);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="cacheSeconds">缓存时间：秒</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        int cacheSeconds = 60,
        params object[] keyParams)
    {
        return ToCacheList<TSource, TOutputDto>(source.Where(predicate), cacheSeconds, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="cacheSeconds">缓存时间：秒</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        int cacheSeconds = 60,
        params object[] keyParams)
    {
        return ToCacheArray<TSource, TOutputDto>(source.Where(predicate), cacheSeconds, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        IFunction function,
        params object[] keyParams)
    {
        return ToCacheList<TSource, TOutputDto>(source.Where(predicate), function, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">数据源的项数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">数据源</param>
    /// <param name="predicate">数据查询表达式</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns></returns>
    public virtual TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        IFunction function,
        params object[] keyParams)
    {
        return ToCacheArray<TSource, TOutputDto>(source.Where(predicate), function, keyParams);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="cacheSeconds">缓存的秒数</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source, int cacheSeconds = 60, params object[] keyParams)
    {
        string key = GetKey<TSource, TOutputDto>(source, keyParams);
        return _cache.Get(key, () => source.ToOutput<TSource, TOutputDto>().ToList(), cacheSeconds);
    }

    /// <summary>
    /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="cacheSeconds">缓存的秒数</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source, int cacheSeconds = 60, params object[] keyParams)
    {
        string key = GetKey<TSource, TOutputDto>(source, keyParams);
        return _cache.Get(key, () => source.ToOutput<TSource, TOutputDto>().ToArray(), cacheSeconds);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source, IFunction function, params object[] keyParams)
    {
        string key = GetKey<TSource, TOutputDto>(source, keyParams);
        return _cache.Get(key, () => source.ToOutput<TSource, TOutputDto>().ToList(), function);
    }

    /// <summary>
    /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
    /// </summary>
    /// <typeparam name="TSource">源数据类型</typeparam>
    /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
    /// <param name="source">查询数据源</param>
    /// <param name="function">缓存策略相关功能</param>
    /// <param name="keyParams">缓存键参数</param>
    /// <returns>查询结果</returns>
    public virtual TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source, IFunction function, params object[] keyParams)
    {
        string key = GetKey<TSource, TOutputDto>(source, keyParams);
        return _cache.Get(key, () => source.ToOutput<TSource, TOutputDto>().ToArray(), function);
    }

    #endregion

    #region 私有方法

    private string GetKey<TEntity, TResult>(IQueryable<TEntity> source,
        Expression<Func<TEntity, bool>> predicate,
        PageCondition pageCondition,
        Expression<Func<TEntity, TResult>> selector,
        params object[] keyParams)
    {
        source = source.Where(predicate);
        SortCondition[] sortConditions = pageCondition.SortConditions;
        if (sortConditions == null || sortConditions.Length == 0)
        {
            if (typeof(TEntity).IsEntityType())
            {
                source = source.OrderBy("Id");
            }
            else if (typeof(TEntity).IsBaseOn<ICreatedTime>())
            {
                source = source.OrderBy("CreatedTime");
            }
            else
            {
                throw new OsharpException($"类型“{typeof(TEntity)}”未添加默认排序方式");
            }
        }
        else
        {
            int count = 0;
            IOrderedQueryable<TEntity> orderSource = null;
            foreach (SortCondition sortCondition in sortConditions)
            {
                orderSource = count == 0
                    ? CollectionPropertySorter<TEntity>.OrderBy(source, sortCondition.SortField, sortCondition.ListSortDirection)
                    : CollectionPropertySorter<TEntity>.ThenBy(orderSource, sortCondition.SortField, sortCondition.ListSortDirection);
                count++;
            }

            source = orderSource;
        }

        int pageIndex = pageCondition.PageIndex, pageSize = pageCondition.PageSize;
        source = source != null
            ? source.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            : Enumerable.Empty<TEntity>().AsQueryable();
        IQueryable<TResult> query = source.Select(selector);
        return GetKey(query.Expression, keyParams);
    }

    private string GetKey<TEntity, TOutputDto>(IQueryable<TEntity> source,
        Expression<Func<TEntity, bool>> predicate,
        PageCondition pageCondition,
        params object[] keyParams)
        where TOutputDto : IOutputDto
    {
        source = source.Where(predicate);
        SortCondition[] sortConditions = pageCondition.SortConditions;
        if (sortConditions == null || sortConditions.Length == 0)
        {
            if (typeof(TEntity).IsEntityType())
            {
                source = source.OrderBy("Id");
            }
            else if (typeof(TEntity).IsBaseOn<ICreatedTime>())
            {
                source = source.OrderBy("CreatedTime");
            }
            else
            {
                throw new OsharpException($"类型“{typeof(TEntity)}”未添加默认排序方式");
            }
        }
        else
        {
            int count = 0;
            IOrderedQueryable<TEntity> orderSource = null;
            foreach (SortCondition sortCondition in sortConditions)
            {
                orderSource = count == 0
                    ? CollectionPropertySorter<TEntity>.OrderBy(source, sortCondition.SortField, sortCondition.ListSortDirection)
                    : CollectionPropertySorter<TEntity>.ThenBy(orderSource, sortCondition.SortField, sortCondition.ListSortDirection);
                count++;
            }

            source = orderSource;
        }

        int pageIndex = pageCondition.PageIndex, pageSize = pageCondition.PageSize;
        source = source != null
            ? source.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            : Enumerable.Empty<TEntity>().AsQueryable();
        IQueryable<TOutputDto> query = source.ToOutput<TEntity, TOutputDto>(true);
        return GetKey(query.Expression, keyParams);
    }

    private string GetKey<TSource, TOutputDto>(IQueryable<TSource> source,
        params object[] keyParams)
    {
        IQueryable<TOutputDto> query = source.ToOutput<TSource, TOutputDto>(true);
        return GetKey(query.Expression, keyParams);
    }

    private string GetKey<TSource, TResult>(IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> selector,
        params object[] keyParams)
    {
        IQueryable<TResult> query = source.Select(selector);
        return GetKey(query.Expression, keyParams);
    }

    private string GetKey(Expression expression, params object[] keyParams)
    {
        string key;
        try
        {
            key = new ExpressionCacheKeyGenerator(expression).GetKey(keyParams);
        }
        catch (TargetInvocationException)
        {
            key = new StringCacheKeyGenerator().GetKey(keyParams);
        }

        key = $"Query:{key.ToMd5Hash()}";
        _logger.LogDebug($"get cache key: {key}");
        return key;
    }

    #endregion

    /// <summary>
    /// 获取或添加全局缓存，不考虑租户
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="factory">缓存数据获取工厂</param>
    /// <param name="expiration">过期时间</param>
    /// <returns>缓存数据</returns>
    public T GetOrAddGlobal<T>(string key, Func<T> factory, TimeSpan? expiration = null)
    {
        Check.NotNullOrEmpty(key, nameof(key));
        Check.NotNull(factory, nameof(factory));

        if (_globalKeyGenerator == null)
        {
            throw new OsharpException("当前缓存键生成器不支持全局缓存键生成");
        }

        string cacheKey = _globalKeyGenerator.GetGlobalKey(key);
        T result = Get<T>(cacheKey);
        if (!Equals(result, default(T)))
        {
            return result;
        }

        result = factory();
        if (Equals(result, default(T)))
        {
            return default;
        }

        Set(cacheKey, result, expiration);
        return result;
    }

    /// <summary>
    /// 异步获取或添加全局缓存，不考虑租户
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="factory">缓存数据获取工厂</param>
    /// <param name="expiration">过期时间</param>
    /// <returns>缓存数据</returns>
    public async Task<T> GetOrAddGlobalAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        Check.NotNullOrEmpty(key, nameof(key));
        Check.NotNull(factory, nameof(factory));

        if (_globalKeyGenerator == null)
        {
            throw new OsharpException("当前缓存键生成器不支持全局缓存键生成");
        }

        string cacheKey = await _globalKeyGenerator.GetGlobalKeyAsync(key);
        T result = await GetAsync<T>(cacheKey);
        if (!Equals(result, default(T)))
        {
            return result;
        }

        result = await factory();
        if (Equals(result, default(T)))
        {
            return default;
        }

        await SetAsync(cacheKey, result, expiration);
        return result;
    }

    /// <summary>
    /// 移除全局缓存，不考虑租户
    /// </summary>
    /// <param name="key">缓存键</param>
    public void RemoveGlobal(string key)
    {
        Check.NotNullOrEmpty(key, nameof(key));

        if (_globalKeyGenerator == null)
        {
            throw new OsharpException("当前缓存键生成器不支持全局缓存键生成");
        }

        string cacheKey = _globalKeyGenerator.GetGlobalKey(key);
        _cache.Remove(cacheKey);
    }

    /// <summary>
    /// 异步移除全局缓存，不考虑租户
    /// </summary>
    /// <param name="key">缓存键</param>
    public async Task RemoveGlobalAsync(string key)
    {
        Check.NotNullOrEmpty(key, nameof(key));

        if (_globalKeyGenerator == null)
        {
            throw new OsharpException("当前缓存键生成器不支持全局缓存键生成");
        }

        string cacheKey = await _globalKeyGenerator.GetGlobalKeyAsync(key);
        await _cache.RemoveAsync(cacheKey);
    }

    // 在 CacheService 类中添加以下方法

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="value">缓存值</param>
    /// <param name="expiration">过期时间</param>
    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        Check.NotNullOrEmpty(key, nameof(key));

        if (value == null)
        {
            return;
        }

        var options = new DistributedCacheEntryOptions();
        if (expiration.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiration;
        }

        string json = JsonSerializer.Serialize(value);
        _cache.SetString(key, json, options);
    }

    /// <summary>
    /// 异步设置缓存
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="value">缓存值</param>
    /// <param name="expiration">过期时间</param>
    /// <returns>异步任务</returns>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        Check.NotNullOrEmpty(key, nameof(key));

        if (value == null)
        {
            return;
        }

        var options = new DistributedCacheEntryOptions();
        if (expiration.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiration;
        }

        string json = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, json, options);
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <returns>缓存数据</returns>
    public T Get<T>(string key)
    {
        Check.NotNullOrEmpty(key, nameof(key));

        string json = _cache.GetString(key);
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// 异步获取缓存
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <returns>缓存数据</returns>
    public async Task<T> GetAsync<T>(string key)
    {
        Check.NotNullOrEmpty(key, nameof(key));

        string json = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }
}
