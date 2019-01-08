// -----------------------------------------------------------------------
//  <copyright file="CodeModule.cs" company="OSharp开源团队">
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
    /// 实体类：代码模块信息
    /// </summary>
    [Description("代码模块信息")]
    public class CodeModule : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模块显示名称
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 所属项目编号
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 获取或设置 所属项目
        /// </summary>
        public virtual CodeProject Project { get; set; }

        /// <summary>
        /// 获取或设置 模块的实体集合
        /// </summary>
        public virtual ICollection<CodeEntity> Entities { get; set; } = new List<CodeEntity>();

        /// <summary>
        /// 获取 模块命名空间，由“项目命名空间前缀.模块名称”组成
        /// </summary>
        public string Namespace => $"{(Project == null ? "" : Project.NamespacePrefix + ".")}{Name}";
    }
}