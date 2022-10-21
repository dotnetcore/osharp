// -----------------------------------------------------------------------
//  <copyright file="SingletonDictionary.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-18 17:56</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.Data
{
    /// <summary>
    /// 创建一个单例字典，该实例的生命周期将跟随整个应用程序
    /// </summary>
    /// <typeparam name="TKey">字典键类型</typeparam>
    /// <typeparam name="TValue">字典值类型</typeparam>
    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        static SingletonDictionary()
        {
            Singleton<IDictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 获取指定类型的字典的单例实例
        /// </summary>
        public new static IDictionary<TKey, TValue> Instance { get { return Singleton<IDictionary<TKey, TValue>>.Instance; } }
    }
}