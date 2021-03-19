// -----------------------------------------------------------------------
//  <copyright file="TableNamePrefixConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-17 23:41</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Extensions;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 表名前缀配置
    /// </summary>
    public class TableNamePrefixConfiguration : IEntityBatchConfiguration
    {
        /// <summary>
        /// 配置指定的<see cref="IMutableEntityType"/>
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="mutableEntityType">实体的<see cref="IMutableEntityType"/>类型</param>
        public void Configure(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        {
            string prefix = GetTableNamePrefix(mutableEntityType.ClrType);
            if (prefix.IsNullOrEmpty())
            {
                return;
            }
            
            string tableName = $"{prefix}_{mutableEntityType.GetTableName()}";
            modelBuilder.Entity(mutableEntityType.ClrType).ToTable(tableName);
        }

        /// <summary>
        /// 从实体类型获取表名前缀
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        protected virtual string GetTableNamePrefix(Type entityType)
        {
            TableNamePrefixAttribute attribute = entityType.GetAttribute<TableNamePrefixAttribute>();
            return attribute?.Prefix;
        }
    }
}