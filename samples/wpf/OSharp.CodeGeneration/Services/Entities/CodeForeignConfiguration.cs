// -----------------------------------------------------------------------
//  <copyright file="CodeForeignConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 23:13</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Entities
{
    public class CodeForeignConfiguration : EntityTypeConfigurationBase<CodeForeign, Guid>
    {
        /// <summary>重写以实现实体类型各个属性的数据库配置</summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<CodeForeign> builder)
        {
            builder.HasOne(m => m.Entity).WithMany(n => n.Foreigns).HasForeignKey(m => m.EntityId).IsRequired();
        }
    }
}
