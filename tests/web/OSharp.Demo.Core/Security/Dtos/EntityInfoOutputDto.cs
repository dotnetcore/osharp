// -----------------------------------------------------------------------
//  <copyright file="EntityInfoOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-26 14:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Text;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Demo.Security.Dtos
{
    /// <summary>
    /// 输出DTO:实体信息
    /// </summary>
    [MapFrom(typeof(EncodingInfo))]
    public class EntityInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 实体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 实体类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据日志
        /// </summary>
        public bool AuditEnabled { get; set; }
    }
}