// -----------------------------------------------------------------------
//  <copyright file="UserConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 11:35</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Demo.Identity.Entities;
using OSharp.Entity;


namespace OSharp.Demo.EntityConfiguration.Identity
{
    public class UserConfiguration : EntityTypeConfigurationBase<User, int>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(m => m.UserDetail).WithOne(n => n.User);
        }
    }
}