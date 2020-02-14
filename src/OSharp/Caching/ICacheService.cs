// -----------------------------------------------------------------------
//  <copyright file="ICacheService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-19 18:07</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Authorization.Functions;
using OSharp.Entity;
using OSharp.Filter;


namespace OSharp.Caching
{
    /// <summary>
    /// 定义缓存服务功能
    /// </summary>
    public interface ICacheService
    {
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
        PageResult<TResult> ToPageCache<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            PageCondition pageCondition,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60,
            params object[] keyParams);

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
        PageResult<TResult> ToPageCache<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            PageCondition pageCondition,
            Expression<Func<TSource, TResult>> selector,
            IFunction function,
            params object[] keyParams);

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
        List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60,
            params object[] keyParams);

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
        TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60,
            params object[] keyParams);

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
        List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            IFunction function,
            params object[] keyParams);

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
        TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            Expression<Func<TSource, TResult>> selector,
            IFunction function,
            params object[] keyParams);

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
        List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60,
            params object[] keyParams);

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
        TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector,
            int cacheSeconds = 60,
            params object[] keyParams);

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
        List<TResult> ToCacheList<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector,
            IFunction function,
            params object[] keyParams);

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
        TResult[] ToCacheArray<TSource, TResult>(IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector,
            IFunction function,
            params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        List<TSource> ToCacheList<TSource>(IQueryable<TSource> source, int cacheSeconds = 60, params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        TSource[] ToCacheArray<TSource>(IQueryable<TSource> source, int cacheSeconds = 60, params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="function">缓存策略相关功能</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        List<TSource> ToCacheList<TSource>(IQueryable<TSource> source, IFunction function, params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="function">缓存策略相关功能</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        TSource[] ToCacheArray<TSource>(IQueryable<TSource> source, IFunction function, params object[] keyParams);

        #region OutputDto

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
        PageResult<TOutputDto> ToPageCache<TEntity, TOutputDto>(IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            PageCondition pageCondition,
            int cacheSeconds = 60,
            params object[] keyParams)
            where TOutputDto : IOutputDto;

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
        PageResult<TOutputDto> ToPageCache<TEntity, TOutputDto>(IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            PageCondition pageCondition,
            IFunction function,
            params object[] keyParams)
            where TOutputDto : IOutputDto;

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
        List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            int cacheSeconds = 60,
            params object[] keyParams);

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
        TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            int cacheSeconds = 60,
            params object[] keyParams);

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
        List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            IFunction function,
            params object[] keyParams);

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
        TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            IFunction function,
            params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source,
            int cacheSeconds = 60,
            params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="cacheSeconds">缓存的秒数</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source,
            int cacheSeconds = 60,
            params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="function">缓存策略相关功能</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        List<TOutputDto> ToCacheList<TSource, TOutputDto>(IQueryable<TSource> source,
            IFunction function,
            params object[] keyParams);

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并按指定缓存策略存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TOutputDto">结果集的项数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="function">缓存策略相关功能</param>
        /// <param name="keyParams">缓存键参数</param>
        /// <returns>查询结果</returns>
        TOutputDto[] ToCacheArray<TSource, TOutputDto>(IQueryable<TSource> source,
            IFunction function,
            params object[] keyParams);

        #endregion
    }
}