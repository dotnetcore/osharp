// -----------------------------------------------------------------------
//  <copyright file="ModuleUserConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 15:32</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security.Entities;
using OSharp.Entity;


namespace OSharp.Demo.EntityConfiguration.Security
{
    /// <summary>
    /// 模块用户信息映射配置类
    /// </summary>
    public class ModuleUserConfiguration : EntityTypeConfigurationBase<ModuleUser, Guid>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<ModuleUser> builder)
        {
            builder.HasIndex(m => new { m.ModuleId, m.UserId }).HasName("ModuleUserIndex").IsUnique();

            builder.HasOne<Module>().WithMany().HasForeignKey(m => m.ModuleId);
            builder.HasOne<User>().WithMany().HasForeignKey(m => m.UserId);
        }
    }
}