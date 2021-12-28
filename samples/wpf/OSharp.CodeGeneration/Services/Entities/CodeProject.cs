// -----------------------------------------------------------------------
//  <copyright file="CodeProject.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-04 23:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Data;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Entities
{
    /// <summary>
    /// 实体类：代码项目信息
    /// </summary>
    [Description("代码项目信息")]
    [TableNamePrefix("CodeGen")]
    public class CodeProject : EntityBase<Guid>, ICreatedTime
    {
        public CodeProject()
        {
            Id = SequentialGuid.Create(DatabaseType.Sqlite);
        }

        /// <summary>
        /// 获取或设置 项目名称
        /// </summary>
        [Required(), StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 项目命名空间前缀，通常形如“公司.项目”
        /// </summary>
        [Required(), StringLength(200)]
        public string NamespacePrefix { get; set; }

        /// <summary>
        /// 获取或设置 公司
        /// </summary>
        [StringLength(200)]
        public string Company { get; set; }

        /// <summary>
        /// 获取或设置 站点地址
        /// </summary>
        [StringLength(500)]
        public string SiteUrl { get; set; }

        /// <summary>
        /// 获取或设置 创建者
        /// </summary>
        [StringLength(200)]
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置 Copyright
        /// </summary>
        [StringLength(500)]
        public string Copyright { get; set; }

        /// <summary>获取或设置 创建时间</summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 根目录
        /// </summary>
        [StringLength(200)]
        public string RootPath { get; set; }

        /// <summary>
        /// 获取或设置 模块信息集合
        /// </summary>
        public virtual ICollection<CodeModule> Modules { get; set; } = new List<CodeModule>();

        /// <summary>
        /// 获取或设置 项目模板集合
        /// </summary>
        public virtual ICollection<CodeProjectTemplate> ProjectTemplates { get; set; } = new List<CodeProjectTemplate>();

        /// <summary>
        /// 获取项目名称
        /// </summary>
        public string GetName()
        {
            return $"{Name}[{NamespacePrefix}]";
        }
    }
}
