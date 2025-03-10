// -----------------------------------------------------------------------
//  <copyright file="AuditOperationConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:16</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Systems.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;


namespace Liuliu.Demo.EntityConfiguration.Systems
{
    public partial class AuditOperationConfiguration : EntityTypeConfigurationBase<AuditOperation, long>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<AuditOperation> builder)
        {
            builder.Property(m => m.Id).ValueGeneratedNever();
            EntityConfigurationAppend(builder);
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void EntityConfigurationAppend(EntityTypeBuilder<AuditOperation> builder);
    }
}