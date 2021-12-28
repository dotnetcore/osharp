// -----------------------------------------------------------------------
//  <copyright file="CodeForeign.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 23:11</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Entities
{
    /// <summary>
    /// 实体类：外键信息
    /// </summary>
    [Description("实体外键")]
    [TableNamePrefix("CodeGen")]
    public class CodeForeign : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 己方导航属性
        /// </summary>
        public string SelfNavigation { get; set; }

        /// <summary>
        /// 获取或设置 己方外键属性
        /// </summary>
        public string SelfForeignKey { get; set; }

        /// <summary>
        /// 获取或设置 对方实体
        /// </summary>
        public string OtherEntity { get; set; }

        /// <summary>
        /// 获取或设置 对方导航属性
        /// </summary>
        public string OtherNavigation { get; set; }

        /// <summary>
        /// 获取或设置 是否必须
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 获取或设置 关系数据删除行为
        /// </summary>
        public DeleteBehavior? DeleteBehavior { get; set; }

        /// <summary>
        /// 获取或设置 外键关系
        /// </summary>
        public ForeignRelation ForeignRelation { get; set; }

        /// <summary>
        /// 获取或设置 实体编号
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// 获取或设置 所属实体信息
        /// </summary>
        [JsonIgnore]
        public virtual CodeEntity Entity { get; set; }
    }
}
