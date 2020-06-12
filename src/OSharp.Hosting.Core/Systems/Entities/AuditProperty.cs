// -----------------------------------------------------------------------
//  <copyright file="AuditProperty.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Audits;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Hosting.Systems.Entities
{
    /// <summary>
    /// 实体类：审计实体属性信息
    /// </summary>
    [MapFrom(typeof(AuditPropertyEntry))]
    [TableNamePrefix("Systems")]
    [Description("审计实体属性信息")]
    public class AuditProperty : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取或设置 字段
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 获取或设置 旧值
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// 获取或设置 新值
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// 获取或设置 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 获取或设置 所属审计实体编号
        /// </summary>
        public Guid AuditEntityId { get; set; }

        /// <summary>
        /// 获取或设置 所属审计实体
        /// </summary>
        public virtual AuditEntity AuditEntity { get; set; }
    }
}