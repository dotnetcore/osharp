// -----------------------------------------------------------------------
//  <copyright file="IFilterService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-20 0:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

using OSharp.Authorization;
using OSharp.Data;


namespace OSharp.Filter
{
    /// <summary>
    /// 定义过滤表达式功能
    /// </summary>
    public interface IFilterService
    {
        /// <summary>
        /// 获取指定查询条件组的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="group">查询条件组，如果为null，则直接返回 true 表达式</param>
        /// <returns>查询表达式</returns>
        Expression<Func<T, bool>> GetExpression<T>(FilterGroup group);

        /// <summary>
        /// 获取指定查询条件的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="rule">查询条件，如果为null，则直接返回 true 表达式</param>
        /// <returns>查询表达式</returns>
        Expression<Func<T, bool>> GetExpression<T>(FilterRule rule);

        /// <summary>
        /// 获取指定查询条件组的查询表达式，并综合数据权限
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="group">传入的查询条件组，为空时则只返回数据权限过滤器</param>
        /// <param name="operation">数据权限操作</param>
        /// <returns>综合之后的表达式</returns>
        Expression<Func<T, bool>> GetDataFilterExpression<T>(FilterGroup group = null,
            DataAuthOperation operation = DataAuthOperation.Read);

        /// <summary>
        /// 验证指定查询条件组是否能正常转换为表达式
        /// </summary>
        /// <param name="group">查询条件组</param>
        /// <param name="type">实体类型</param>
        /// <returns>验证操作结果</returns>
        OperationResult CheckFilterGroup(FilterGroup group, Type type);
    }
}