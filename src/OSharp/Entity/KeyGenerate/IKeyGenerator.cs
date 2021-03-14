// -----------------------------------------------------------------------
//  <copyright file="IIntKeyGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-14 13:35</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity.KeyGenerate
{
    /// <summary>
    /// 定义<see cref="TKey"/>类型主键生成器
    /// </summary>
    public interface IKeyGenerator<out TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取一个<see cref="TKey"/>类型的主键数据
        /// </summary>
        /// <returns></returns>
        TKey Create();
    }
}