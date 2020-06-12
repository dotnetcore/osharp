// -----------------------------------------------------------------------
//  <copyright file="RoleOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Hosting.Identity.Entities;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Hosting.Identity.Dtos
{
    /// <summary>
    /// 角色输出DTO
    /// </summary>
    [MapFrom(typeof(Role))]
    public class RoleOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        [Description("编号")]
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 角色名称
        /// </summary>
        [Description("角色名")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 角色备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 是否管理
        /// </summary>
        [Description("是否管理")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获取或设置 是否默认
        /// </summary>
        [Description("是否默认")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        [Description("是否锁定")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedTime { get; set; }

        #region Implementation of IDataAuthEnabled

        /// <summary>
        /// 获取或设置 是否可更新的数据权限状态
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可删除的数据权限状态
        /// </summary>
        public bool Deletable { get; set; }

        #endregion
    }
}