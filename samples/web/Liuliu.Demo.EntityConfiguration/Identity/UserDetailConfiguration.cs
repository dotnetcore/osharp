// -----------------------------------------------------------------------
//  <copyright file="UserDetailConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:48</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Identity.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;


namespace Liuliu.Demo.EntityConfiguration.Identity
{
    public partial class UserDetailConfiguration : EntityTypeConfigurationBase<UserDetail, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<UserDetail> builder)
        {
            builder.HasOne(ud => ud.User).WithOne(u => u.UserDetail).HasForeignKey<UserDetail>(ud => ud.UserId).IsRequired();

            EntityConfigurationAppend(builder);
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void EntityConfigurationAppend(EntityTypeBuilder<UserDetail> builder);
    }
}