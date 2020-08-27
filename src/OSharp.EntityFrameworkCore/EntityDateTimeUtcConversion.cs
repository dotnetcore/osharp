// -----------------------------------------------------------------------
//  <copyright file="EntityDateTimeUtcConversion.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-26 23:44</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体时间属性UTC转换器
    /// </summary>
    /// <seealso cref="OSharp.Entity.IEntityDateTimeUtcConversion" />
    public class EntityDateTimeUtcConversion : IEntityDateTimeUtcConversion
    {
        private readonly ValueConverter<DateTime, DateTime> _dateTimeConverter;
        private readonly ValueConverter<DateTime?, DateTime?> _nullableDateTimeConverter;

        /// <summary>
        /// 初始化一个<see cref="EntityDateTimeUtcConversion"/>类型的新实例
        /// </summary>
        public EntityDateTimeUtcConversion()
        {
            _dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                local => local.ToUniversalTime(),
                utc => utc.ToLocalTime());
            _nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                local => local.HasValue ? local.Value.ToUniversalTime() : local,
                utc => utc.HasValue ? utc.Value.ToLocalTime() : utc);
        }

        /// <summary>
        /// 转换指定的实体类型。
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public void Convert(IMutableEntityType entityType)
        {
            foreach (IMutableProperty property in entityType.GetProperties())
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