// -----------------------------------------------------------------------
//  <copyright file="ISequentialGuidGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-14 13:48</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;


namespace OSharp.Entity.KeyGenerate
{
    /// <summary>
    /// 定义有顺序的Guid主键生成器
    /// </summary>
    public interface ISequentialGuidGenerator : IKeyGenerator<Guid>
    {
        /// <summary>
        /// 获取 顺序Guid数据库类型
        /// </summary>
        DatabaseType DatabaseType { get; }
    }
}