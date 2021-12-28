// -----------------------------------------------------------------------
//  <copyright file="CodeEntity.cs" company="OSharp开源团队">
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
using System.Diagnostics;

using Newtonsoft.Json;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Entities
{
    /// <summary>
    /// 实体类：代码实体信息
    /// </summary>
    [Description("代码实体信息")]
    [TableNamePrefix("CodeGen")]
    [MapTo(typeof(CodeEntity))]
    [DebuggerDisplay("{Display}[{Name}]")]
    public class CodeEntity : EntityBase<Guid>, ILockable, ICreatedTime
    {
        /// <summary>
        /// 获取或设置 类型名称
        /// </summary>
        [Required(), StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 类型显示名称
        /// </summary>
        [Required(), StringLength(200)]
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 主键类型全名
        /// </summary>
        [Required(), StringLength(500)]
        public string PrimaryKeyTypeFullName { get; set; }

        /// <summary>
        /// 获取或设置 图标
        /// </summary>
        [StringLength(200)]
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置 是否可列表
        /// </summary>
        public bool Listable { get; set; }

        /// <summary>
        /// 获取或设置 是否可添加
        /// </summary>
        public bool Addable { get; set; }

        /// <summary>
        /// 获取或设置 是否可更新
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可删除
        /// </summary>
        public bool Deletable { get; set; }

        /// <summary>
        /// 获取或设置 是否数据权限控制
        /// </summary>
        public bool IsDataAuth { get; set; }

        /// <summary>
        /// 获取或设置 是否有创建时间
        /// </summary>
        public bool HasCreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 是否有锁定
        /// </summary>
        public bool HasLocked { get; set; }

        /// <summary>
        /// 获取或设置 是否有软删除
        /// </summary>
        public bool HasSoftDeleted { get; set; }

        /// <summary>
        /// 获取或设置 是否有创建审计
        /// </summary>
        public bool HasCreationAudited { get; set; }

        /// <summary>
        /// 获取或设置 是否有更新审计
        /// </summary>
        public bool HasUpdateAudited { get; set; }

        /// <summary>
        /// 获取或设置 排序号
        /// </summary>
        public int Order { get; set; }

        /// <summary>获取或设置 创建时间</summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 所属模块编号
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// 获取或设置 所属模块
        /// </summary>
        [JsonIgnore]
        public virtual CodeModule Module { get; set; }

        /// <summary>
        /// 获取或设置 实体的属性集合
        /// </summary>
        public virtual ICollection<CodeProperty> Properties { get; set; } = new List<CodeProperty>();

        /// <summary>
        /// 获取或设置 外键集合
        /// </summary>
        public virtual ICollection<CodeForeign> Foreigns { get; set; } = new List<CodeForeign>();
    }
}
