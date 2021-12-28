// -----------------------------------------------------------------------
//  <copyright file="CodeProjectTemplate.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-09 12:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Newtonsoft.Json;

using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Entities
{
    /// <summary>
    /// 实体类：项目模板配对
    /// </summary>
    [Description("项目模板配对")]
    [TableNamePrefix("CodeGen")]
    public class CodeProjectTemplate : EntityBase<Guid>, ILockable
    {
        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 项目编号
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 获取或设置 项目信息
        /// </summary>
        [JsonIgnore]
        public virtual CodeProject Project { get; set; }

        /// <summary>
        /// 获取或设置 模板编号
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// 获取或设置 模板信息
        /// </summary>
        public virtual CodeTemplate Template { get; set; }
    }
    
}
