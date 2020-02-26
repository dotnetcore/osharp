// -----------------------------------------------------------------------
//  <copyright file="ModuleInputDtoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 12:42</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.Authorization.Dtos
{
    /// <summary>
    /// 模块信息输入DTO基类
    /// </summary>
    /// <typeparam name="TModuleKey">模块编号类型</typeparam>
    public abstract class ModuleInputDtoBase<TModuleKey> : IInputDto<TModuleKey>
        where TModuleKey : struct, IEquatable<TModuleKey>
    {
        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        [DisplayName("编号")]
        public TModuleKey Id { get; set; }

        /// <summary>
        /// 获取或设置 模块名称
        /// </summary>
        [Required, DisplayName("模块名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        [DisplayName("模块描述")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 模块代码
        /// </summary>
        [Required]
        [DisplayName("模块代码")]
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置 节点内排序码
        /// </summary>
        [DisplayName("排序码")]
        public double OrderCode { get; set; }

        /// <summary>
        /// 获取或设置 父模块编号
        /// </summary>
        [DisplayName("父模块编号")]
        public TModuleKey? ParentId { get; set; }
    }
}