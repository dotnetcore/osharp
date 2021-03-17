// -----------------------------------------------------------------------
//  <copyright file="IMutableEntityTypeConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-17 23:38</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using OSharp.Dependency;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义实体的批量配置功能
    /// </summary>
    [MultipleDependency]
    public interface IEntityBatchConfiguration
    {
        /// <summary>
        /// 配置指定的<see cref="IMutableEntityType"/>
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="mutableEntityType">实体的<see cref="IMutableEntityType"/>类型</param>
        void Configure(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType);
    }
}