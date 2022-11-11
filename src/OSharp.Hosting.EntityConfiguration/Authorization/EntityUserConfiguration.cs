// -----------------------------------------------------------------------
//  <copyright file="EntityUserConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:48</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Authorization.Entities;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.EntityConfiguration.Authorization;

public partial class EntityUserConfiguration : EntityTypeConfigurationBase<EntityUser, long>
{
    /// <summary>
    /// 重写以实现实体类型各个属性的数据库配置
    /// </summary>
    /// <param name="builder">实体类型创建器</param>
    public override void Configure(EntityTypeBuilder<EntityUser> builder)
    {
        builder.HasIndex(m => new { m.EntityId, m.UserId }).HasDatabaseName("EntityUserIndex");

        builder.HasOne<EntityInfo>(eu => eu.EntityInfo).WithMany().HasForeignKey(m => m.EntityId);
        builder.HasOne<User>(eu => eu.User).WithMany().HasForeignKey(m => m.UserId);

        EntityConfigurationAppend(builder);
    }

    /// <summary>
    /// 额外的数据映射
    /// </summary>
    partial void EntityConfigurationAppend(EntityTypeBuilder<EntityUser> builder);
}
