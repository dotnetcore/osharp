// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:04 17:42</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Data;


namespace OSharp.Linq
{
    /// <summary>
    /// Expression表达式扩展操作类
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 以特定的条件运行组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="first">第一个Expression表达式</param>
        /// <param name="second">要组合的Expression表达式</param>
        /// <param name="merge">组合条件运算方式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            Check.NotNull(first, nameof(first));
            Check.NotNull(second, nameof(second));
            Check.NotNull(merge, nameof(merge));
            
            Dictionary<ParameterExpression, ParameterExpression> map =
                first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// 以 Expression.AndAlso 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="first">第一个Expression表达式</param>
        /// <param name="second">要组合的Expression表达式</param>
        /// <param name="ifExp">判断条件表达式，当此条件为true时，才执行组合</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second, bool ifExp = true)
        {
            Check.NotNull(first, nameof(first));
            Check.NotNull(second, nameof(second));
            return ifExp ? first.Compose(second, Expression.AndAlso) : first;
        }

        /// <summary>
        /// 以 Expression.OrElse 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="first">第一个Expression表达式</param>
        /// <param name="second">要组合的Expression表达式</param>
        /// <param name="ifExp">判断条件表达式，当此条件为true时，才执行组合</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second, bool ifExp = true)
        {
            Check.NotNull(first, nameof(first));
            Check.NotNull(second, nameof(second));
            return ifExp ? first.Compose(second, Expression.OrElse) : first;
        }
        

        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (_map.TryGetValue(node, out replacement))
                {
                    node = replacement;
                }
                return base.VisitParameter(node);
            }
        }
    }
}