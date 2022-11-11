// -----------------------------------------------------------------------
//  <copyright file="UserTokenConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-06 14:57</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.EntityConfiguration.Identity;

public partial class UserTokenConfiguration : EntityTypeConfigurationBase<UserToken, long>
{
    /// <summary>
    /// 重写以实现实体类型各个属性的数据库配置
    /// </summary>
    /// <param name="builder">实体类型创建器</param>
    public override void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasIndex(m => new { m.UserId, m.LoginProvider, m.Name }).HasDatabaseName("UserTokenIndex").IsUnique();
        builder.HasOne(ut => ut.User).WithMany(u => u.UserTokens).HasForeignKey(ut => ut.UserId).IsRequired();

        EntityConfigurationAppend(builder);
    }

    /// <summary>
    /// 额外的数据映射
    /// </summary>
    partial void EntityConfigurationAppend(EntityTypeBuilder<UserToken> builder);
}
