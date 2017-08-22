// -----------------------------------------------------------------------
//  <copyright file="DictionaryExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-19 13:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace OSharp.Extensions
{
    /// <summary>
    /// 字典辅助扩展方法
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 从字典中获取值，不存在则返回字典<typeparamref name="TValue"/>类型的默认值
        /// </summary>
        /// <typeparam name="TKey">字典键类型</typeparam>
        /// <typeparam name="TValue">字典值类型</typeparam>
        /// <param name="dictionary">要操作的字典</param>
        /// <param name="key">指定键名</param>
        /// <returns>获取到的值</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue>dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : default(TValue);
        }

        /// <summary>
        /// 获取指定键的值，不存在则按指定委托添加值
        /// </summary>
        /// <typeparam name="TKey">字典键类型</typeparam>
        /// <typeparam name="TValue">字典值类型</typeparam>
        /// <param name="dictionary">要操作的字典</param>
        /// <param name="key">指定键名</param>
        /// <param name="addFunc">添加值的委托</param>
        /// <returns>获取到的值</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue>dictionary, TKey key, Func<TValue> addFunc)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }
            return dictionary[key] = addFunc();
        }
    }
}