// -----------------------------------------------------------------------
//  <copyright file="UserLoginConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:48</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Identity.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;


namespace Liuliu.Demo.EntityConfiguration.Identity
{
    public partial class UserLoginConfiguration : EntityTypeConfigurationBase<UserLogin, long>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.Property(m => m.Id).ValueGeneratedNever();
            builder.HasIndex(m => new { m.LoginProvider, m.ProviderKey }).HasDatabaseName("UserLoginIndex").IsUnique();
            builder.HasOne(ul => ul.User).WithMany(u => u.UserLogins).HasForeignKey(ul => ul.UserId).IsRequired();

            EntityConfigurationAppend(builder);
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void EntityConfigurationAppend(EntityTypeBuilder<UserLogin> builder);
    }
}