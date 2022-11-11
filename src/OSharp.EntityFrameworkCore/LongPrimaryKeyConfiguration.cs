// -----------------------------------------------------------------------
//  <copyright file="LongPrimaryKeyConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-11 23:36</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity;

/// <summary>
/// Long类型的主键，使用雪花算法，关闭数据库自增
/// </summary>
public class LongPrimaryKeyRemoveAutoIncreaseConfiguration : IEntityBatchConfiguration
{
    /// <summary>
    /// 配置指定的<see cref="IMutableEntityType"/>
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    /// <param name="mutableEntityType">实体的<see cref="IMutableEntityType"/>类型</param>
    public void Configure(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    {
        var key = mutableEntityType.FindPrimaryKey();
        if (key == null || !key.IsPrimaryKey())
        {
            return;
        }

        var property = key.Properties.FirstOrDefault();
        if (property == null || property.ClrType != typeof(long))
        {
            return;
        }

        property.ValueGenerated = ValueGenerated.Never;
    }
}
