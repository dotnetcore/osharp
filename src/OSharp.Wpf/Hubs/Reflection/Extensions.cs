// -----------------------------------------------------------------------
//  <copyright file="Extensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 19:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace OSharp.Wpf.Hubs.Reflection
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 从表达式中获取方法名称及其参数值
        /// <para>这用于进行动态调用或将.net调用转换为wire-api调用。</para>
        /// </summary>
        public static Invocation GetInvocation<T>(this Expression<Action<T>> action)
        {
            if (!(action.Body is MethodCallExpression))
            {
                throw new ArgumentException(@"Action must be a method call", nameof(action));
            }

            MethodCallExpression callExpression = (MethodCallExpression)action.Body;
            Invocation invocation = new Invocation()
            {
                Parameters = callExpression.Arguments.Select(ReduceToConstant).ToArray(),
                MethodName = callExpression.Method.Name
            };
            return invocation;
        }

        /// <summary>
        /// 从表达式中获取方法名称及其参数值
        /// <para>这用于进行动态调用或将.net调用转换为wire-api调用。</para>
        /// </summary>
        public static Invocation GetInvocation<TInput, TResult>(this Expression<Func<TInput, TResult>> action)
        {
            if (!(action.Body is MethodCallExpression))
            {
                throw new ArgumentException(@"Action must be a method call", nameof(action));
            }
            MethodCallExpression callExpression = (MethodCallExpression)action.Body;
            Invocation invocation = new Invocation()
            {
                Parameters = callExpression.Arguments.Select(ReduceToConstant).ToArray(),
                MethodName = callExpression.Method.Name,
                ReturnType = typeof(TResult)
            };
            return invocation;
        }

        /// <summary>
        /// 从lambda表达式中获取方法的名称和该方法的参数类型
        /// <para>这用于事件绑定。</para>
        /// </summary>
        public static MethodCallInfo GetBinding(this LambdaExpression exp)
        {
            var unaryExpression = (UnaryExpression)exp.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

            var methodInfo = MethodInfo(methodCallExpression);

            return new MethodCallInfo
            {
                MethodName = methodInfo.Name,
                ParameterTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray()
            };
        }

        private static MethodInfo MethodInfo(MethodCallExpression methodCallExpression)
        {
            // This works on .Net 4.0 and 4.5
            var methodInfo = (methodCallExpression.Object == null)
                ? ((MethodInfo)((ConstantExpression)methodCallExpression.Arguments.Last()).Value)
                : ((MethodInfo)((ConstantExpression)methodCallExpression.Object).Value);
            return methodInfo;
        }

        private static object ReduceToConstant(Expression expression)
        {
            var objectMember = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }
    }
}