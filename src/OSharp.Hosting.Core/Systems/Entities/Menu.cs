// -----------------------------------------------------------------------
//  <copyright file="MenuInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-02-28 14:03</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.Hosting.Systems.Entities
{
    /// <summary>
    /// 实体类：菜单信息
    /// </summary>
    [TableNamePrefix("Systems")]
    [Description("菜单信息")]
    public class Menu : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 菜单名称
        /// </summary>
        [Required, StringLength(500), DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 显示名称
        /// </summary>
        [Required, StringLength(500), DisplayName("显示")]
        public string Text { get; set; }

        /// <summary>
        /// 获取或设置 菜单图标
        /// </summary>
        [StringLength(50), DisplayName("图标")]
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置 菜单链接
        /// </summary>
        [StringLength(2000), DisplayName("链接")]
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置 链接目标
        /// </summary>
        [StringLength(50), DisplayName("链接目标")]
        public string Target { get; set; }

        /// <summary>
        /// 获取或设置 权限访问控制列表
        /// </summary>
        [StringLength(500), DisplayName("访问控制列表")]
        public string Acl { get; set; }

        /// <summary>
        /// 获取或设置 节点内排序
        /// </summary>
        [DisplayName("节点内排序")]
        public double OrderCode { get; set; }

        /// <summary>
        /// 获取或设置 菜单数据
        /// </summary>
        [DisplayName("菜单数据"), StringLength(1000)]
        public string Data { get; set; }

        /// <summary>
        /// 获取或设置 父节点树形路径
        /// </summary>
        [DisplayName("父节点树形路径"), StringLength(1000)]
        public string TreePathString { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        [DisplayName("是否启用")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否系统
        /// </summary>
        [DisplayName("是否系统")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 获取或设置 父菜单编号
        /// </summary>
        [DisplayName("父菜单编号")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 获取或设置 父级菜单
        /// </summary>
        public virtual Menu Parent { get; set; }

        /// <summary>
        /// 获取或设置 子菜单集合
        /// </summary>
        public virtual ICollection<Menu> Children { get; set; } = new List<Menu>();
    }
}