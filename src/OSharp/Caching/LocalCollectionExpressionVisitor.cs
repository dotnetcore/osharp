// -----------------------------------------------------------------------
//  <copyright file="LocalCollectionExpressionVisitor.cs" company="柳柳软件">
//      Copyright (c) 2014-2016 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2016-11-16 23:34</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Reflection;


namespace OSharp.Caching
{
    internal class LocalCollectionExpressionVisitor : ExpressionVisitor
    {
        public static Expression Rewrite(Expression expression)
        {
            return new LocalCollectionExpressionVisitor().Visit(expression);
        }

        #region Overrides of ExpressionVisitor

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MethodCallExpression"/>.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Type instanceType = node.Object == null ? null : node.Object.Type;
            var map = new[] { new { Param = instanceType, Arg = node.Object } }.ToList();
            map.AddRange(node.Method.GetParameters().Zip(node.Arguments, (m, n) => new { Param = m.ParameterType, Arg = n }));

            var replacements = map.Where(m => m.Param != null && m.Param.IsGenericType)
                .Select(m => new { m, type = m.Param.GetGenericTypeDefinition() })
                .Where(o => typeof(IEnumerable<>).IsGenericAssignableFrom(o.type))
                .Where(o => o.m.Arg.NodeType == ExpressionType.Constant)
                .Select(o => new { o, type = o.m.Param.GetGenericArguments().Single() })
                .Select(p => new
                {
                    p.o.m.Arg,
                    Replacement = Expression.Constant("{" + string.Join("|", (IEnumerable)((ConstantExpression)p.o.m.Arg).Value) + "}")
                }).ToList();

            if (replacements.Any())
            {
                List<Expression> args =
                    map.Select(m => (from c in replacements where m.Arg == c.Arg select c.Replacement).SingleOrDefault() ?? m.Arg).ToList();
                try
                {
                    node = node.Update(args.First(), args.Skip(1));
                }
                catch (ArgumentException)
                {
                    return node;
                }
            }

            return base.VisitMethodCall(node);
        }

        #endregion
    }
}