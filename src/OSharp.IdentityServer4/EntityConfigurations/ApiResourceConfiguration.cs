// -----------------------------------------------------------------------
//  <copyright file="ApiResourceConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 22:49</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;
using OSharp.IdentityServer4.Entities;


namespace OSharp.IdentityServer4.EntityConfigurations
{
    /// <summary>
    /// API资源信息映射配置类
    /// </summary>
    public class ApiResourceConfiguration : EntityTypeConfigurationBase<ApiResource, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<ApiResource> builder)
        {
            builder.HasIndex(m => m.Name).IsUnique();

            builder.HasMany(m => m.Secrets).WithOne(m => m.ApiResource).HasForeignKey(m => m.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.Scopes).WithOne(m => m.ApiResource).HasForeignKey(m => m.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.UserClaims).WithOne(m => m.ApiResource).HasForeignKey(m => m.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.Properties).WithOne(m => m.ApiResource).HasForeignKey(m => m.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}