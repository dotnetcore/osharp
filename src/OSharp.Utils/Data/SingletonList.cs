// -----------------------------------------------------------------------
//  <copyright file="SingletonList.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-18 17:56</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.Data
{
    /// <summary>
    /// 创建一个类型列表的单例，该实例的生命周期将跟随整个应用程序
    /// </summary>
    /// <typeparam name="T">要创建的列表元素的类型</typeparam>
    public class SingletonList<T> : Singleton<IList<T>>
    {
        static SingletonList()
        {
            Singleton<IList<T>>.Instance = new List<T>();
        }

        /// <summary>
        /// 获取指定类型的列表的单例实例
        /// </summary>
        public new static IList<T> Instance { get { return Singleton<IList<T>>.Instance; } }
    }
}