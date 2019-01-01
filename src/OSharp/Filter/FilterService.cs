// -----------------------------------------------------------------------
//  <copyright file="FilterService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-20 0:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Data;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Linq;
using OSharp.Properties;
using OSharp.Reflection;
using OSharp.Secutiry;
using OSharp.Secutiry.Claims;


namespace OSharp.Filter
{
    /// <summary>
    /// 查询表达式服务
    /// </summary>
    public class FilterService : IFilterService
    {
        private readonly IServiceProvider _serviceProvider;

        #region 字段

        private static readonly Dictionary<FilterOperate, Func<Expression, Expression, Expression>> ExpressionDict =
            new Dictionary<FilterOperate, Func<Expression, Expression, Expression>>
            {
                {
                    FilterOperate.Equal, Expression.Equal
                },
                {
                    FilterOperate.NotEqual, Expression.NotEqual
                },
                {
                    FilterOperate.Less, Expression.LessThan
                },
                {
                    FilterOperate.Greater, Expression.GreaterThan
                },
                {
                    FilterOperate.LessOrEqual, Expression.LessThanOrEqual
                },
                {
                    FilterOperate.GreaterOrEqual, Expression.GreaterThanOrEqual
                },
                {
                    FilterOperate.StartsWith,
                    (left, right) =>
                    {
                        if (left.Type != typeof(string))
                        {
                            throw new NotSupportedException("“StartsWith”比较方式只支持字符串类型的数据");
                        }
                        return Expression.Call(left,
                            typeof(string).GetMethod("StartsWith", new[] { typeof(string) })
                            ?? throw new InvalidOperationException("名称为“StartsWith”的方法不存在"),
                            right);
                    }
                },
                {
                    FilterOperate.EndsWith,
                    (left, right) =>
                    {
                        if (left.Type != typeof(string))
                        {
                            throw new NotSupportedException("“EndsWith”比较方式只支持字符串类型的数据");
                        }
                        return Expression.Call(left,
                            typeof(string).GetMethod("EndsWith", new[] { typeof(string) })
                            ?? throw new InvalidOperationException("名称为“EndsWith”的方法不存在"),
                            right);
                    }
                },
                {
                    FilterOperate.Contains,
                    (left, right) =>
                    {
                        if (left.Type != typeof(string))
                        {
                            throw new NotSupportedException("“Contains”比较方式只支持字符串类型的数据");
                        }
                        return Expression.Call(left,
                            typeof(string).GetMethod("Contains", new[] { typeof(string) })
                            ?? throw new InvalidOperationException("名称为“Contains”的方法不存在"),
                            right);
                    }
                },
                {
                    FilterOperate.NotContains,
                    (left, right) =>
                    {
                        if (left.Type != typeof(string))
                        {
                            throw new NotSupportedException("“NotContains”比较方式只支持字符串类型的数据");
                        }
                        return Expression.Not(Expression.Call(left,
                            typeof(string).GetMethod("Contains", new[] { typeof(string) })
                            ?? throw new InvalidOperationException("名称为“Contains”的方法不存在"),
                            right));
                    }
                }
                //{
                //    FilterOperates.StdIn, (left, right) =>
                //    {
                //        if (!right.Type.IsArray)
                //        {
                //            return null;
                //        }
                //        return left.Type != typeof (string) ? null : Expression.Call(typeof (Enumerable), "Contains", new[] {left.Type}, right, left);
                //    }
                //},
                //{
                //    FilterOperates.DataTimeLessThanOrEqual, Expression.LessThanOrEqual
                //}
            };

        #endregion

        /// <summary>
        /// 初始化一个<see cref="FilterService"/>类型的新实例
        /// </summary>
        public FilterService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Implementation of IFilterService

        /// <summary>
        /// 获取指定查询条件组的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="group">查询条件组，如果为null，则直接返回 true 表达式</param>
        /// <returns>查询表达式</returns>
        public virtual Expression<Func<T, bool>> GetExpression<T>(FilterGroup group)
        {
            Check.NotNull(group, nameof(group));

            ParameterExpression param = Expression.Parameter(typeof(T), "m");
            Expression body = GetExpressionBody(param, group);
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(body, param);
            return expression;
        }

        /// <summary>
        /// 获取指定查询条件的查询表达式
        /// </summary>
        /// <typeparam name="T">表达式实体类型</typeparam>
        /// <param name="rule">查询条件，如果为null，则直接返回 true 表达式</param>
        /// <returns>查询表达式</returns>
        public virtual Expression<Func<T, bool>> GetExpression<T>(FilterRule rule)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "m");
            Expression body = GetExpressionBody(param, rule);
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(body, param);
            return expression;
        }

        /// <summary>
        /// 获取指定查询条件组的查询表达式，并综合数据权限
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="group">传入的查询条件组，为空时则只返回数据权限过滤器</param>
        /// <param name="operation">数据权限操作</param>
        /// <returns>综合之后的表达式</returns>
        public virtual Expression<Func<T, bool>> GetDataFilterExpression<T>(FilterGroup group = null,
            DataAuthOperation operation = DataAuthOperation.Read)
        {
            Expression<Func<T, bool>> exp = m => true;
            if (group != null)
            {
                exp = GetExpression<T>(group);
            }
            //从缓存中查找当前用户的角色与实体T的过滤条件
            ClaimsPrincipal user = _serviceProvider.GetCurrentUser();
            if (user == null)
            {
                return exp;
            }

            IDataAuthCache dataAuthCache = _serviceProvider.GetService<IDataAuthCache>();
            if (dataAuthCache == null)
            {
                return exp;
            }

            // 要判断数据权限功能,先要排除没有执行当前功能权限的角色,判断剩余角色的数据权限
            string[] roleNames = user.Identity.GetRoles();
            ScopedDictionary scopedDict = _serviceProvider.GetService<ScopedDictionary>();
            if (scopedDict?.Function != null)
            {
                roleNames = scopedDict.DataAuthValidRoleNames;
            }
            string typeName = typeof(T).GetFullNameWithModule();
            Expression<Func<T, bool>> subExp = null;
            foreach (string roleName in roleNames)
            {
                FilterGroup subGroup = dataAuthCache.GetFilterGroup(roleName, typeName, operation);
                if (subGroup == null)
                {
                    continue;
                }
                subExp = subExp == null ? GetExpression<T>(subGroup) : subExp.Or(GetExpression<T>(subGroup));
            }
            if (subExp != null)
            {
                if (group == null)
                {
                    return subExp;
                }
                exp = subExp.And(exp);
            }

            return exp;
        }

        /// <summary>
        /// 验证指定查询条件组是否能正常转换为表达式
        /// </summary>
        /// <param name="group">查询条件组</param>
        /// <param name="type">实体类型</param>
        /// <returns>验证操作结果</returns>
        public virtual OperationResult CheckFilterGroup(FilterGroup group, Type type)
        {
            try
            {
                ParameterExpression param = Expression.Parameter(type, "m");
                GetExpressionBody(param, group);
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, $"条件组验证失败：{ex.Message}");
            }
        }

        #endregion

        #region 私有方法

        private Expression GetExpressionBody(ParameterExpression param, FilterGroup group)
        {
            Check.NotNull(param, nameof(param));

            //如果无条件或条件为空，直接返回 true表达式
            if (group == null || (group.Rules.Count == 0 && group.Groups.Count == 0))
            {
                return Expression.Constant(true);
            }
            List<Expression> bodys = new List<Expression>();
            bodys.AddRange(group.Rules.Select(rule => GetExpressionBody(param, rule)));
            bodys.AddRange(group.Groups.Select(subGroup => GetExpressionBody(param, subGroup)));

            if (group.Operate == FilterOperate.And)
            {
                return bodys.Aggregate(Expression.AndAlso);
            }
            if (group.Operate == FilterOperate.Or)
            {
                return bodys.Aggregate(Expression.OrElse);
            }
            throw new OsharpException(Resources.Filter_GroupOperateError);
        }

        private Expression GetExpressionBody(ParameterExpression param, FilterRule rule)
        {
            if (rule == null || rule.Value == null || string.IsNullOrEmpty(rule.Value.ToString()))
            {
                return Expression.Constant(true);
            }
            LambdaExpression expression = GetPropertyLambdaExpression(param, rule);
            Expression constant = ChangeTypeToExpression(rule, expression.Body.Type);
            return ExpressionDict[rule.Operate](expression.Body, constant);
        }

        private static LambdaExpression GetPropertyLambdaExpression(ParameterExpression param, FilterRule rule)
        {
            string[] propertyNames = rule.Field.Split('.');
            Expression propertyAccess = param;
            Type type = param.Type;
            foreach (string propertyName in propertyNames)
            {
                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new InvalidOperationException(string.Format(Resources.Filter_RuleFieldInTypeNotFound, rule.Field, type.FullName));
                }
                type = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }
            return Expression.Lambda(propertyAccess, param);
        }

        private Expression ChangeTypeToExpression(FilterRule rule, Type conversionType)
        {
            //if (item.Method == QueryMethod.StdIn)
            //{
            //    Array array = (item.Value as Array);
            //    List<Expression> expressionList = new List<Expression>();
            //    if (array != null)
            //    {
            //        expressionList.AddRange(array.Cast<object>().Select((t, i) =>
            //            ChangeType(array.GetValue(i), conversionType)).Select(newValue => Expression.Constant(newValue, conversionType)));
            //    }
            //    return Expression.NewArrayInit(conversionType, expressionList);
            //}
            if (rule.Value?.ToString() == "@CurrentUserId")
            {
                if (rule.Operate != FilterOperate.Equal)
                {
                    throw new OsharpException($"当前用户“{rule.Value}”只能用在“{FilterOperate.Equal.ToDescription()}”操作中");
                }

                ClaimsPrincipal user = _serviceProvider.GetCurrentUser();
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    throw new OsharpException("需要获取当前用户编号，但当前用户为空，可能未登录或已过期");
                }
                object value = user.Identity.GetClaimValueFirstOrDefault(ClaimTypes.NameIdentifier);
                value = value.CastTo(conversionType);
                return Expression.Constant(value, conversionType);
            }
            else
            {
                object value = rule.Value.CastTo(conversionType);
                return Expression.Constant(value, conversionType);
            }
        }

        #endregion
    }
}