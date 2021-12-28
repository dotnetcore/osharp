// -----------------------------------------------------------------------
//  <copyright file="CodeProjectInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-10 21:35</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services.Dtos
{
    [MapTo(typeof(CodeProject))]
    public class CodeProjectInputDto : IInputDto<Guid>
    {
        /// <summary>获取或设置 主键，唯一标识</summary>
        public Guid Id { get; set; }

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

        /// <summary>
        /// 获取或设置 根目录
        /// </summary>
        [StringLength(200)]
        public string RootPath { get; set; }
    }
}
