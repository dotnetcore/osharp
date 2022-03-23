// -----------------------------------------------------------------------
//  <copyright file="CodeTemplateInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-10 22:07</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.CodeGeneration.Generates;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Dtos
{
    [MapTo(typeof(CodeTemplate))]
    [MapFrom(typeof(CodeTemplate))]
    public class CodeTemplateInputDto : IInputDto<Guid>
    {
        /// <summary>获取或设置 主键，唯一标识</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 配置名称
        /// </summary>
        [Required, StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 元数据类型
        /// </summary>
        public MetadataType MetadataType { get; set; }

        /// <summary>
        /// 获取或设置 模板文件，默认内置，也可以由用户自定义加载
        /// </summary>
        [Required, StringLength(500)]
        public string TemplateFile { get; set; }

        /// <summary>
        /// 获取或设置 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 获取或设置 代码输出文件名格式
        /// </summary>
        [Required, StringLength(300)]
        public string OutputFileFormat { get; set; }

        /// <summary>
        /// 获取或设置 是否只生成一次
        /// </summary>
        public bool IsOnce { get; set; }

        /// <summary>
        /// 获取或设置 系统类型
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>获取或设置 是否锁定当前信息</summary>
        public bool IsLocked { get; set; }
    }
}
