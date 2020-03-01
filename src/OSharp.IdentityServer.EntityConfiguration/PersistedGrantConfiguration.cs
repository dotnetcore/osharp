// -----------------------------------------------------------------------
//  <copyright file="PersistedGrantConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 23:59</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.IdentityServer.Storage.Entities;


namespace OSharp.IdentityServer.EntityConfiguration
{
    /// <summary>
    /// 持续授权信息映射配置类
    /// </summary>
    public class PersistedGrantConfiguration : Id4EntityTypeConfigurationBase<PersistedGrant, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<PersistedGrant> builder)
        {
            builder.HasIndex(m => m.Key).IsUnique();
        }
    }
}