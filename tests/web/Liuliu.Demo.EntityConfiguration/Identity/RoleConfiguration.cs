// -----------------------------------------------------------------------
//  <copyright file="RoleConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:48</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Identity.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;


namespace Liuliu.Demo.EntityConfiguration.Identity
{
    public class RoleConfiguration : EntityTypeConfigurationBase<Role, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasIndex(m => m.NormalizedName).HasName("RoleNameIndex").IsUnique();

            builder.Property(m => m.ConcurrencyStamp).IsConcurrencyToken();

            builder.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            builder.HasData(new Role() { Id = 1, Name = "系统管理员", NormalizedName = "系统管理员", Remark = "系统最高权限管理角色", IsAdmin = true, IsSystem = true });
        }
    }
}