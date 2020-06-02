// -----------------------------------------------------------------------
//  <copyright file="CollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-08-04 0:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Authorization;
using OSharp.Collections;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Mapping;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 集合扩展辅助操作类
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询指定数据筛选的分页信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TResult">分页数据类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageCondition">分页查询条件</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <returns>分页结果信息</returns>
        public static PageResult<TResult> ToPage<TEntity, TResult>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            PageCondition pageCondition,
            Expression<Func<TEntity, TResult>> selector)
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");
            pageCondition.CheckNotNull("pageCondition");
            selector.CheckNotNull("selector");

            return source.ToPage(predicate, pageCondition.PageIndex, pageCondition.PageSize, pageCondition.SortConditions, selector);
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询指定数据筛选的分页信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TResult">分页数据类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sortConditions">排序条件集合</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <returns>分页结果信息</returns>
        public static PageResult<TResult> ToPage<TEntity, TResult>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            int pageIndex,
            int pageSize,
            SortCondition[] sortConditions,
            Expression<Func<TEntity, TResult>> selector)
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");
            pageIndex.CheckGreaterThan("pageIndex", 0);
            pageSize.CheckGreaterThan("pageSize", 0);
            selector.CheckNotNull("selector");

            TResult[] data = source.Where(predicate, pageIndex, pageSize, out int total, sortConditions).Select(selector).ToArray();
            return new PageResult<TResult>() { Total = total, Data = data };
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询指定输出DTO的分页信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TOutputDto">输出DTO数据类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageCondition">分页查询条件</param>
        /// <returns>分页结果信息</returns>
        public static PageResult<TOutputDto> ToPage<TEntity, TOutputDto>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            PageCondition pageCondition)
            where TOutputDto : IOutputDto
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");
            pageCondition.CheckNotNull("pageCondition");
            return source.ToPage<TEntity, TOutputDto>(predicate,
                pageCondition.PageIndex,
                pageCondition.PageSize,
                pageCondition.SortConditions);
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询出指定输出DTO的分页信息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TOutputDto">输出DTO数据类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sortConditions">排序条件集合</param>
        /// <returns>分页结果信息</returns>
        public static PageResult<TOutputDto> ToPage<TEntity, TOutputDto>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            int pageIndex,
            int pageSize,
            SortCondition[] sortConditions)
            where TOutputDto : IOutputDto
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");
            pageIndex.CheckGreaterThan("pageIndex", 0);
            pageSize.CheckGreaterThan("pageSize", 0);

            TOutputDto[] data = source.Where(predicate, pageIndex, pageSize, out int total, sortConditions).ToOutput<TEntity, TOutputDto>().ToArray();
            return new PageResult<TOutputDto>() { Total = total, Data = data };
        }

        /// <summary>
        /// 将数据源映射为指定<typeparamref name="TOutputDto"/>的集合，
        /// 并验证数据的<see cref="DataAuthOperation.Update"/>,<see cref="DataAuthOperation.Delete"/>数据权限状态
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="getKey">是否应用于获取缓存键时</param>
        public static IQueryable<TOutputDto> ToOutput<TEntity, TOutputDto>(this IQueryable<TEntity> source, bool getKey = false)
        {
            if (!typeof(TOutputDto).IsBaseOn<IDataAuthEnabled>() || getKey)
            {
                return MapperExtensions.ToOutput<TEntity, TOutputDto>(source);
            }

            List<TEntity> entities = source.ToList();
            List<TOutputDto> dtos = new List<TOutputDto>();
            Func<TEntity, bool> updateFunc = FilterHelper.GetDataFilterExpression<TEntity>(null, DataAuthOperation.Update).Compile();
            Func<TEntity, bool> deleteFunc = FilterHelper.GetDataFilterExpression<TEntity>(null, DataAuthOperation.Delete).Compile();
            foreach (TEntity entity in entities)
            {
                TOutputDto dto = entity.MapTo<TOutputDto>();
                IDataAuthEnabled dto2 = (IDataAuthEnabled)dto;
                dto2.Updatable = updateFunc(entity);
                dto2.Deletable = deleteFunc(entity);
                dtos.Add(dto);
            }
            return dtos.AsQueryable();
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询指定分页条件的子数据集
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageCondition">分页查询条件</param>
        /// <param name="total">输出符合条件的总记录数</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            PageCondition pageCondition,
            out int total)
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");
            pageCondition.CheckNotNull("pageCondition");

            return source.Where(predicate, pageCondition.PageIndex, pageCondition.PageSize, out total, pageCondition.SortConditions);
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询指定分页条件的子数据集
        /// </summary>
        /// <typeparam name="TEntity">动态实体类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="total">输出符合条件的总记录数</param>
        /// <param name="sortConditions">排序条件集合</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate,
            int pageIndex,
            int pageSize,
            out int total,
            SortCondition[] sortConditions = null)
        {
            source.CheckNotNull("source");
            predicate.CheckNotNull("predicate");
            pageIndex.CheckGreaterThan("pageIndex", 0);
            pageSize.CheckGreaterThan("pageSize", 0);

            total = source.Count(predicate);
            source = source.Where(predicate);
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
            return source != null
                ? source.Skip((pageIndex - 1) * pageSize).Take(pageSize)
                : Enumerable.Empty<TEntity>().AsQueryable();
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询未过期的子数据集，用于筛选实现了<see cref="IExpirable"/>接口的数据集
        /// </summary>
        public static IQueryable<TEntity> Unexpired<TEntity>(this IQueryable<TEntity> source)
            where TEntity : class, IExpirable
        {
            DateTime now = DateTime.Now;
            Expression<Func<TEntity, bool>> predicate =
                m => (m.BeginTime == null || m.BeginTime.Value <= now) && (m.EndTime == null || m.EndTime.Value >= now);
            return source.Where(predicate);
        }

        /// <summary>
        /// 从指定<see cref="IEnumerable{T}"/>集合中查询未过期的子数据集，用于筛选实现了<see cref="IExpirable"/>接口的数据集
        /// </summary>
        public static IEnumerable<TEntity> Unexpired<TEntity>(this IEnumerable<TEntity> source)
            where TEntity : class, IExpirable
        {
            DateTime now = DateTime.Now;
            bool Func(TEntity m) => (m.BeginTime == null || m.BeginTime.Value <= now) && (m.EndTime == null || m.EndTime.Value >= now);
            return source.Where(Func);
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>集合中查询已过期的子数据集，用于筛选实现了<see cref="IExpirable"/>接口的数据集
        /// </summary>
        public static IQueryable<TEntity> Expired<TEntity>(this IQueryable<TEntity> source)
            where TEntity : class, IExpirable
        {
            DateTime now = DateTime.Now;
            Expression<Func<TEntity, bool>> predicate = m => m.EndTime != null && m.EndTime.Value < now;
            return source.Where(predicate);
        }

        /// <summary>
        /// 从指定<see cref="IEnumerable{T}"/>集合中查询已过期的子数据集，用于筛选实现了<see cref="IExpirable"/>接口的数据集
        /// </summary>
        public static IEnumerable<TEntity> Expired<TEntity>(this IEnumerable<TEntity> source)
            where TEntity : class, IExpirable
        {
            DateTime now = DateTime.Now;
            bool Func(TEntity m) => m.EndTime != null && m.EndTime.Value < now;
            return source.Where(Func);
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>数据集中查询未锁定的子数据集，用于筛选实现了<see cref="ILockable"/>接口的数据集
        /// </summary>
        public static IQueryable<TEntity> Unlocked<TEntity>(this IQueryable<TEntity> source)
            where TEntity : class, ILockable
        {
            return source.Where(m => !m.IsLocked);
        }

        /// <summary>
        /// 从指定<see cref="IEnumerable{T}"/>数据集中查询未锁定的子数据集，用于筛选实现了<see cref="ILockable"/>接口的数据集
        /// </summary>
        public static IEnumerable<TEntity> Unlocked<TEntity>(this IEnumerable<TEntity> source)
            where TEntity : class, ILockable
        {
            return source.Where(m => !m.IsLocked);
        }

        /// <summary>
        /// 从指定<see cref="IQueryable{T}"/>数据集中查询已锁定的子数据集，用于筛选实现了<see cref="ILockable"/>接口的数据集
        /// </summary>
        public static IQueryable<TEntity> Locked<TEntity>(this IQueryable<TEntity> source)
            where TEntity : class, ILockable
        {
            return source.Where(m => m.IsLocked);
        }

        /// <summary>
        /// 从指定<see cref="IEnumerable{T}"/>数据集中查询已锁定的子数据集，用于筛选实现了<see cref="ILockable"/>接口的数据集
        /// </summary>
        public static IEnumerable<TEntity> Locked<TEntity>(this IEnumerable<TEntity> source)
            where TEntity : class, ILockable
        {
            return source.Where(m => m.IsLocked);
        }
    }
}
