// -----------------------------------------------------------------------
//  <copyright file="ClientConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 23:26</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;
using OSharp.IdentityServer4.Entities;


namespace OSharp.IdentityServer4.EntityFrameworkCore
{
    /// <summary>
    /// 客户端信息映射配置类
    /// </summary>
    public class ClientConfiguration : Id4EntityTypeConfigurationBase<Client, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasIndex(m => m.ClientId).IsUnique();

            builder.HasMany(m => m.AllowedGrantTypes).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.RedirectUris).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.PostLogoutRedirectUris).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.AllowedScopes).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.ClientSecrets).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.Claims).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.IdentityProviderRestrictions).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.AllowedCorsOrigins).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.Properties).WithOne(m => m.Client).HasForeignKey(m => m.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}