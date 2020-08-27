// -----------------------------------------------------------------------
//  <copyright file="AuditEntity.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:17</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Hosting.Systems.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;



namespace OSharp.Hosting.EntityConfiguration.Systems
{
    public partial class AuditEntityConfiguration : EntityTypeConfigurationBase<AuditEntity, Guid>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<AuditEntity> builder)
        {
            builder.HasIndex(m => m.OperationId);
            builder.HasOne(m => m.Operation).WithMany(n => n.AuditEntities).HasForeignKey(m => m.OperationId);

            EntityConfigurationAppend(builder);
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void EntityConfigurationAppend(EntityTypeBuilder<AuditEntity> builder);
    }
}