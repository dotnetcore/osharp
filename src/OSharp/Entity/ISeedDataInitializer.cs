// -----------------------------------------------------------------------
//  <copyright file="ISeedData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-06 21:36</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Dependency;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义种子数据初始化
    /// </summary>
    [MultipleDependency]
    public interface ISeedDataInitializer
    {
        /// <summary>
        /// 获取 种子数据初始化的顺序
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 获取 所属实体类型
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// 初始化种子数据
        /// </summary>
        void Initialize();
    }
}