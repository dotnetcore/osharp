// -----------------------------------------------------------------------
//  <copyright file="MenuInfoOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-01 11:04</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using OSharp.Entity;
using OSharp.Hosting.Systems.Entities;
using OSharp.Mapping;


namespace OSharp.Hosting.Systems.Dtos
{
    /// <summary>
    /// 输出DTO：菜单信息
    /// </summary>
    [MapFrom(typeof(Menu))]
    public class MenuOutputDto : IOutputDto
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 菜单显示
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 获取或设置 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置 菜单链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置 目标
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 获取或设置 权限访问控制列表
        /// </summary>
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
        /// 获取或设置 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否系统
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 获取或设置 父菜单编号
        /// </summary>
        public int? ParentId { get; set; }
        
        /// <summary>
        /// 获取或设置 子菜单集合
        /// </summary>
        public List<MenuOutputDto> Children { get; set; } = new List<MenuOutputDto>();
    }
}