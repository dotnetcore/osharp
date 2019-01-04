// -----------------------------------------------------------------------
//  <copyright file="AsyncHelper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-02 14:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;

using JetBrains.Annotations;


namespace OSharp.Threading
{
    /// <summary>
    /// 异步辅助操作类
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// 执行Task并处理Finally
        /// </summary>
        public static async Task AwaitTaskWithFinally(Task returnValueTask, Action<Exception> finalAction)
        {
            Exception exception = null;
            try
            {
                await returnValueTask;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        public static async Task AwaitTaskWithPostActionAndFinally(Task returnValueTask, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await returnValueTask;
                await postAction();
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        public static async Task AwaitTaskWithPreActionAndPostActionAndFinally(Func<Task> actualReturnValue,
            Func<Task> preAction = null,
            Func<Task> postAction = null,
            Action<Exception> finalAction = null)
        {
            Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        public static async Task<T> AwaitTaskWithFinallyAndGetResult<T>(Task<T> actualReturnValue, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                return await actualReturnValue;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        public static object CallAwaitTaskWithFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Action<Exception> finalAction)
        {
            return typeof(AsyncHelper)
                .GetTypeInfo()
                .GetMethod("AwaitTaskWithFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                ?.MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, finalAction });
        }

        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue,
            Func<Task> postAction,
            Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                var result = await actualReturnValue;
                await postAction();
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType,
            object actualReturnValue,
            Func<Task> action,
            Action<Exception> finalAction)
        {
            return typeof(AsyncHelper)
                .GetTypeInfo()
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                ?.MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, action, finalAction });
        }

        public static async Task<T> AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult<T>(Func<Task<T>> actualReturnValue,
            Func<Task> preAction = null,
            Func<Task> postAction = null,
            Action<Exception> finalAction = null)
        {
            Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                var result = await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();
                }

                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        public static object CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(Type taskReturnType,
            Func<object> actualReturnValue,
            Func<Task> preAction = null,
            Func<Task> postAction = null,
            Action<Exception> finalAction = null)
        {
            var returnFunc = typeof(AsyncHelper)
                .GetTypeInfo()
                .GetMethod("ConvertFuncOfObjectToFuncOfTask", BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue });

            return typeof(AsyncHelper)
                .GetTypeInfo()
                .GetMethod("AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                ?.MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { returnFunc, preAction, postAction, finalAction });
        }

        [UsedImplicitly]
        private static Func<Task<T>> ConvertFuncOfObjectToFuncOfTask<T>(Func<object> actualReturnValue)
        {
            return () => (Task<T>)actualReturnValue();
        }
    }
}