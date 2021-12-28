// -----------------------------------------------------------------------
//  <copyright file="CodeEntityInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-10 21:54</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Dtos
{
    [MapTo(typeof(CodeEntity))]
    public class CodeEntityInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public Guid Id { get; set; }

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
        
        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 所属模块编号
        /// </summary>
        public Guid ModuleId { get; set; }
    }
}
