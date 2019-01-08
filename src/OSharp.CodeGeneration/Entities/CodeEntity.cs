// -----------------------------------------------------------------------
//  <copyright file="CodeEntity.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-08 14:11</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.CodeGeneration.Entities
{
    /// <summary>
    /// 实体类：代码实体信息
    /// </summary>
    [Description("代码实体信息")]
    public class CodeEntity : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 类型显示名称
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 主键类型全名
        /// </summary>
        public string PrimaryKeyTypeFullName { get; set; }

        /// <summary>
        /// 获取或设置 是否数据权限控制
        /// </summary>
        public bool IsDataAuth { get; set; }

        /// <summary>
        /// 获取或设置 所属模块编号
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// 获取或设置 所属模块
        /// </summary>
        public virtual CodeModule Module { get; set; }

        /// <summary>
        /// 获取或设置 实体的属性集合
        /// </summary>
        public virtual ICollection<CodeProperty> Properties { get; set; } = new List<CodeProperty>();
    }
}