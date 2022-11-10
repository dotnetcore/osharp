// -----------------------------------------------------------------------
//  <copyright file="FilterService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-20 0:15</last-date>
// -----------------------------------------------------------------------


namespace OSharp.Filter;

/// <summary>
/// 查询表达式服务
/// </summary>
public class FilterService : IFilterService
{
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
                        ?? throw new InvalidOperationException($"名称为“StartsWith”的方法不存在"),
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
                        ?? throw new InvalidOperationException($"名称为“EndsWith”的方法不存在"),
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
                        ?? throw new InvalidOperationException($"名称为“Contains”的方法不存在"),
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
                        ?? throw new InvalidOperationException($"名称为“Contains”的方法不存在"),
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

    private readonly IServiceProvider _provider;

    /// <summary>
    /// 初始化一个<see cref="FilterService"/>类型的新实例
    /// </summary>
    public FilterService(IServiceProvider provider)
    {
        _provider = provider;
    }
        
    protected ClaimsPrincipal CurrentUser => _provider.GetCurrentUser();

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
        Check.NotNull(rule, nameof(rule));
            
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
    [Obsolete("使用IDataAuthService.GetDataFilter代替")]
    public virtual Expression<Func<T, bool>> GetDataFilterExpression<T>(FilterGroup group = null,
        DataAuthOperation operation = DataAuthOperation.Read)
    {
        IDataAuthService dataAuthService = _provider.GetRequiredService<IDataAuthService>();
        Expression<Func<T, bool>> exp = dataAuthService.GetDataFilter<T>(operation, group);
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
        param.CheckNotNull("param");

        //如果无条件或条件为空，直接返回 true表达式
        if (group == null || (group.Rules.Count == 0 && group.Groups.Count == 0))
        {
            return Expression.Constant(true);
        }
        List<Expression> bodies = new List<Expression>();
        bodies.AddRange(group.Rules.Select(rule => GetExpressionBody(param, rule)));
        bodies.AddRange(group.Groups.Select(subGroup => GetExpressionBody(param, subGroup)));

        if (group.Operate == FilterOperate.And)
        {
            return bodies.Aggregate(Expression.AndAlso);
        }
        if (group.Operate == FilterOperate.Or)
        {
            return bodies.Aggregate(Expression.OrElse);
        }
        throw new OsharpException(Resources.Filter_GroupOperateError);
    }

    private Expression GetExpressionBody(ParameterExpression param, FilterRule rule)
    {
        // if (rule == null || rule.Value == null || string.IsNullOrEmpty(rule.Value.ToString()))
        if (rule == null)
        {
            return Expression.Constant(true);
        }
        LambdaExpression expression = GetPropertyLambdaExpression(param, rule);
        if (expression == null)
        {
            return Expression.Constant(true);
        }
        Expression constant = ChangeTypeToExpression(rule, expression.Body.Type);
        return ExpressionDict[rule.Operate](expression.Body, constant);
    }

    private static LambdaExpression GetPropertyLambdaExpression(ParameterExpression param, FilterRule rule)
    {
        string[] propertyNames = rule.Field.Split('.');
        if (rule.IsLowerCaseToUpperCase)
        {
            propertyNames = propertyNames.Select(m => m.ToUpperCase()).ToArray();
        }
        Expression propertyAccess = param;
        Type type = param.Type;
        for (var index = 0; index < propertyNames.Length; index++)
        {
            string propertyName = propertyNames[index];
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format(Resources.Filter_RuleFieldInTypeNotFound, rule.Field, type.FullName));
            }

            type = property.PropertyType;
            //验证最后一个属性与属性值是否匹配
            if (index == propertyNames.Length - 1)
            {
                bool flag = CheckFilterRule(type, rule);
                if (!flag)
                {
                    return null;
                }
            }

            propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
        }
        return Expression.Lambda(propertyAccess, param);
    }

    /// <summary>
    /// 验证最后一个属性与属性值是否匹配 
    /// </summary>
    /// <param name="type">最后一个属性</param>
    /// <param name="rule">条件信息</param>
    /// <returns></returns>
    private static bool CheckFilterRule(Type type, FilterRule rule)
    {
        if (rule.Value == null || rule.Value.ToString() == string.Empty)
        {
            rule.Value = null;
        }

        if (rule.Value == null && (type == typeof(string) || type.IsNullableType()))
        {
            return rule.Operate == FilterOperate.Equal || rule.Operate == FilterOperate.NotEqual;
        }

        if (rule.Value == null)
        {
            return !type.IsValueType;
        }
        return true;
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
        if (rule.Value?.ToString() == UserFlagAttribute.Token)
        {
            // todo: 将UserFlag之类的功能提升为接口进行服务注册，好方便实现自定义XXXFlag
            if (rule.Operate != FilterOperate.Equal)
            {
                throw new OsharpException($"当前用户“{rule.Value}”只能用在“{FilterOperate.Equal.ToDescription()}”操作中");
            }
            ClaimsPrincipal user = CurrentUser;
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