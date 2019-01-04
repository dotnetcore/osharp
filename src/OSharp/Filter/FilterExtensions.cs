// -----------------------------------------------------------------------
//  <copyright file="FilterExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-10 14:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;


namespace OSharp.Filter
{
    /// <summary>
    /// 数据过滤扩展方法
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// 将条件组转换为查询表达式
        /// </summary>
        [Obsolete("使用 IFilterService 服务代替，此类将在1.0版本中移除")]
        public static Expression<Func<TEntity, bool>> ToExpression<TEntity>(this FilterGroup group)
        {
            return FilterHelper.GetExpression<TEntity>(group);
        }

        /// <summary>
        /// 将条件转换为查询表达式
        /// </summary>
        [Obsolete("使用 IFilterService 服务代替，此类将在1.0版本中移除")]
        public static Expression<Func<TEntity, bool>> ToExpression<TEntity>(this FilterRule rule)
        {
            return FilterHelper.GetExpression<TEntity>(rule);
        }
    }
}