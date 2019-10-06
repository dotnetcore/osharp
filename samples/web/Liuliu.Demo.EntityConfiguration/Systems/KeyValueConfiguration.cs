// -----------------------------------------------------------------------
//  <copyright file="KeyValueConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-12 16:02</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Core.Systems;
using OSharp.Entity;
using System;


namespace Liuliu.Demo.EntityConfiguration.Systems
{
    public class KeyValueConfiguration : EntityTypeConfigurationBase<KeyValue, Guid>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<KeyValue> builder)
        {
            builder.HasData(
                new KeyValue() { Id = Guid.Parse("534d7813-0eea-44cc-b88e-a9cb010c6981"), Key = SystemSettingKeys.SiteName, Value = "OSHARP" },
                new KeyValue() { Id = Guid.Parse("977e4bba-97b2-4759-a768-a9cb010c698c"), Key = SystemSettingKeys.SiteDescription, Value = "Osharp with .NetStandard2.0 & Angular6" }
            );
        }
    }
}