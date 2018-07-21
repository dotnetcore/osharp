// -----------------------------------------------------------------------
//  <copyright file="TryCatchExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>最后修改人</last-editor>
//  <last-date>2014-07-29 2:57</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Extensions
{
    /// <summary>
    /// Try-Catch扩展操作
    /// </summary>
    public static class TryCatchExtensions
    {
        /// <summary>
        /// 对某对象执行指定功能与后续功能，并处理异常情况
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">值</param>
        /// <param name="action">要对值执行的主功能代码</param>
        /// <param name="failureAction">catch中的功能代码</param>
        /// <param name="successAction">主功能代码成功后执行的功能代码</param>
        /// <returns>主功能代码是否顺利执行</returns>
        public static bool TryCatch<T>(this T source, Action<T> action, Action<Exception> failureAction, Action<T> successAction) where T : class
        {
            bool result;
            try
            {
                action(source);
                successAction(source);
                result = true;
            }
            catch (Exception obj)
            {
                failureAction(obj);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 对某对象执行指定功能，并处理异常情况
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">值</param>
        /// <param name="action">要对值执行的主功能代码</param>
        /// <param name="failureAction">catch中的功能代码</param>
        /// <returns>主功能代码是否顺利执行</returns>
        public static bool TryCatch<T>(this T source, Action<T> action, Action<Exception> failureAction) where T : class
        {
            return source.TryCatch(action,
                failureAction,
                obj =>
                { });
        }

        /// <summary>
        /// 对某对象执行指定功能，并处理异常情况与返回值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="source">值</param>
        /// <param name="func">要对值执行的主功能代码</param>
        /// <param name="failureAction">catch中的功能代码</param>
        /// <param name="successAction">主功能代码成功后执行的功能代码</param>
        /// <returns>功能代码的返回值，如果出现异常，则返回对象类型的默认值</returns>
        public static TResult TryCatch<T, TResult>(this T source, Func<T, TResult> func, Action<Exception> failureAction, Action<T> successAction)
            where T : class
        {
            TResult result;
            try
            {
                TResult u = func(source);
                successAction(source);
                result = u;
            }
            catch (Exception obj)
            {
                failureAction(obj);
                result = default(TResult);
            }
            return result;
        }

        /// <summary>
        /// 对某对象执行指定功能，并处理异常情况与返回值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="source">值</param>
        /// <param name="func">要对值执行的主功能代码</param>
        /// <param name="failureAction">catch中的功能代码</param>
        /// <returns>功能代码的返回值，如果出现异常，则返回对象类型的默认值</returns>
        public static TResult TryCatch<T, TResult>(this T source, Func<T, TResult> func, Action<Exception> failureAction) where T : class
        {
            return source.TryCatch(func,
                failureAction,
                obj =>
                { });
        }

        /// <summary>
        /// 对某对象执行指定功能，并处理finally
        /// </summary>
        public static void TryFinally<T>(this T source, Action<T> action, Action<T> finallyAction) where T : class
        {
            try
            {
                action(source);
            }
            finally
            {
                finallyAction(source);
            }
        }
    }
}