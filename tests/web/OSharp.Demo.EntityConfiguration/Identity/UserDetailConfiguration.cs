// -----------------------------------------------------------------------
//  <copyright file="UserDetailConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 18:08</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Demo.Identity.Entities;
using OSharp.Entity;


namespace OSharp.Demo.EntityConfiguration.Identity
{
    public class UserDetailConfiguration : EntityTypeConfigurationBase<UserDetail, int>
    {
        public override void Configure(EntityTypeBuilder<UserDetail> builder)
        { }
    }
}