// -----------------------------------------------------------------------
//  <copyright file="CodeModuleInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-10 21:47</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Dtos
{
    [MapTo(typeof(CodeModule))]
    public class CodeModuleInputDto : IInputDto<Guid>
    {
        /// <summary>获取或设置 主键，唯一标识</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 模块名称
        /// </summary>
        [Required(), StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模块显示名称
        /// </summary>
        [Required(), StringLength(200)]
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 图标
        /// </summary>
        [StringLength(200)]
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置 排序号
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 所属项目编号
        /// </summary>
        public Guid ProjectId { get; set; }
    }
}
