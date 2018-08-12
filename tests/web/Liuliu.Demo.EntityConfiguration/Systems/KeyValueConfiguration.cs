// -----------------------------------------------------------------------
//  <copyright file="KeyValueConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-12 16:02</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Core.Systems;
using OSharp.Entity;


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
                new KeyValue() { Key = SystemSettingKeys.SiteName, Value = "OSHARP" },
                new KeyValue() { Key = SystemSettingKeys.SiteDescription, Value = "Osharp with .NetStandard2.0 & Angular6" }
            );
        }
    }
}