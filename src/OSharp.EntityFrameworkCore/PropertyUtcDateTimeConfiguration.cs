// -----------------------------------------------------------------------
//  <copyright file="EntityUtcDateTimeConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-17 23:48</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// 配置实体的时间属性的Utc时间转换，在数据库中保存Utc时间，在代码运行时使用当前时区的时间
    /// </summary>
    public class PropertyUtcDateTimeConfiguration : IEntityBatchConfiguration
    {
        private readonly IEntityManager _entityManager;
        private readonly OsharpOptions _osharpOptions;
        private readonly ValueConverter<DateTime, DateTime> _dateTimeConverter;
        private readonly ValueConverter<DateTime?, DateTime?> _nullableDateTimeConverter;

        /// <summary>
        /// 初始化一个<see cref="PropertyUtcDateTimeConfiguration"/>类型的新实例
        /// </summary>
        public PropertyUtcDateTimeConfiguration(IServiceProvider provider)
        {
            _entityManager = provider.GetService<IEntityManager>();
            _osharpOptions = provider.GetOSharpOptions();

            _dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                local => local.ToUniversalTime(),
                utc => utc.ToLocalTime());
            _nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                local => local.HasValue ? local.Value.ToUniversalTime() : local,
                utc => utc.HasValue ? utc.Value.ToLocalTime() : utc);
        }

        /// <summary>
        /// 配置指定的<see cref="IMutableEntityType"/>
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="mutableEntityType">实体的<see cref="IMutableEntityType"/>类型</param>
        public void Configure(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        {
            foreach (IMutableProperty property in mutableEntityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(_dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(_nullableDateTimeConverter);
                }
            }
        }
    }
}