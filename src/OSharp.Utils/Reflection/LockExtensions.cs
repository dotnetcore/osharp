// -----------------------------------------------------------------------
//  <copyright file="LockExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-19 2:39</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Reflection
{
    /// <summary>
    /// lock操作扩展方法
    /// </summary>
    public static class LockExtensions
    {
        /// <summary>
        /// 锁定给定object标识，执行委托
        /// </summary>
        public static void Locking(this object source, Action action)
        {
            lock (source)
            {
                action();
            }
        }

        /// <summary>
        /// 锁定给定源数据，执行委托
        /// </summary>
        public static void Locking<T>(this T source, Action<T>action)
        {
            lock (source)
            {
                action(source);
            }
        }

        /// <summary>
        /// 锁定给定object标识，执行委托
        /// </summary>
        public static TResult Locking<TResult>(this object source, Func<TResult>func)
        {
            lock (source)
            {
                return func();
            }
        }

        /// <summary>
        /// 锁定给定源数据，执行委托
        /// </summary>
        public static TResult Locking<TSource, TResult>(this TSource source, Func<TSource, TResult>func)
        {
            lock (source)
            {
                return func(source);
            }
        }
    }
}