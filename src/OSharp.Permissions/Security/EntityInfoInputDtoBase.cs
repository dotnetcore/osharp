// -----------------------------------------------------------------------
//  <copyright file="EntityInfoInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-15 17:21</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.Security
{
    /// <summary>
    /// 输入Dto基类：实体信息
    /// </summary>
    public abstract class EntityInfoInputDtoBase : IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 是否启用数据审计
        /// </summary>
        public bool AuditEnabled { get; set; }

        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        public Guid Id { get; set; }
    }
}