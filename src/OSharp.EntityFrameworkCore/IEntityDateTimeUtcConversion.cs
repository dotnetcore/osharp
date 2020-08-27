// -----------------------------------------------------------------------
//  <copyright file="IEntityUtcConverter.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-26 23:37</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体时间属性UTC转换器
    /// </summary>
    public interface IEntityDateTimeUtcConversion
    {
        /// <summary>
        /// 转换指定的实体类型。
        /// </summary>
        /// <param name="entityType">实体类型</param>
        void Convert(IMutableEntityType entityType);
    }
}