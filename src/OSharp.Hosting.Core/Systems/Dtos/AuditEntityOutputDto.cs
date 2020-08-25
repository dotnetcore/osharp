// -----------------------------------------------------------------------
//  <copyright file="AuditEntityOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 18:40</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using OSharp.Audits;
using OSharp.Entity;


namespace OSharp.Hosting.Systems.Dtos
{
    /// <summary>
    /// 输入DTO：数据审计信息
    /// </summary>
    public class AuditEntityOutputDto : IOutputDto
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
        /// 获取或设置 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 数据编号
        /// </summary>
        public string EntityKey { get; set; }

        /// <summary>
        /// 获取或设置 操作类型
        /// </summary>
        public OperateType OperateType { get; set; }

        /// <summary>
        /// 获取或设置 当前用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 所属操作名称
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 获取或设置 信息添加时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 实体属性审计信息集合
        /// </summary>
        public IEnumerable<AuditPropertyOutputDto> Properties { get; set; } = new List<AuditPropertyOutputDto>();
    }
}