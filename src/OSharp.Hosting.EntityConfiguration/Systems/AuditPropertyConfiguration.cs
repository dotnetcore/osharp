// -----------------------------------------------------------------------
//  <copyright file="AuditPropertyConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-11 3:05</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.EntityConfiguration.Systems;

public partial class AuditPropertyConfiguration : EntityTypeConfigurationBase<AuditProperty, long>
{
    /// <summary>
    /// 重写以实现实体类型各个属性的数据库配置
    /// </summary>
    /// <param name="builder">实体类型创建器</param>
    public override void Configure(EntityTypeBuilder<AuditProperty> builder)
    {
        builder.Property(m => m.Id).ValueGeneratedNever();
        builder.HasIndex(m => m.AuditEntityId);
        builder.HasOne(m => m.AuditEntity).WithMany(n => n.Properties).HasForeignKey(m => m.AuditEntityId);

        EntityConfigurationAppend(builder);
    }

    /// <summary>
    /// 额外的数据映射
    /// </summary>
    partial void EntityConfigurationAppend(EntityTypeBuilder<AuditProperty> builder);
}
