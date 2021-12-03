// -----------------------------------------------------------------------
//  <copyright file="MenuInfoConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-02-28 21:50</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.EntityConfiguration.Systems
{
    public partial class MenuConfiguration : EntityTypeConfigurationBase<Menu, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasIndex(m => m.ParentId);
            builder.HasMany(m => m.Children).WithOne(m => m.Parent).HasForeignKey(m => m.ParentId);

            EntityConfigurationAppend(builder);
        }

        partial void EntityConfigurationAppend(EntityTypeBuilder<Menu> builder);
    }
}