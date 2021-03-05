// -----------------------------------------------------------------------
//  <copyright file="MenuInfoInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-02-28 23:51</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;
using OSharp.Hosting.Systems.Entities;
using OSharp.Mapping;


namespace OSharp.Hosting.Systems.Dtos
{
    [MapTo(typeof(Menu))]
    public class MenuInputDto : IInputDto<int>
    {
        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 菜单名称
        /// </summary>
        [Required, StringLength(500)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 菜单显示
        /// </summary>
        [Required, StringLength(500)]
        public string Text { get; set; }

        /// <summary>
        /// 获取或设置 菜单图标
        /// </summary>
        [StringLength(50)]
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置 菜单链接
        /// </summary>
        [StringLength(2000)]
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置 目标
        /// </summary>
        [StringLength(50)]
        public string Target { get; set; }

        /// <summary>
        /// 获取或设置 权限访问控制列表
        /// </summary>
        [StringLength(500)]
        public string Acl { get; set; }

        /// <summary>
        /// 获取或设置 节点内排序
        /// </summary>
        public double OrderCode { get; set; }

        /// <summary>
        /// 获取或设置 菜单数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 获取或设置 父节点树形路径
        /// </summary>
        public string TreePathString { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 获取或设置 是否系统
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 获取或设置 父级菜单编号
        /// </summary>
        public int? ParentId { get; set; }
    }
}