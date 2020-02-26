// -----------------------------------------------------------------------
//  <copyright file="RoleInputDtoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 13:30</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.Identity.Dtos
{
    /// <summary>
    /// 角色信息输入DTO基类
    /// </summary>
    public abstract class RoleInputDtoBase<TRoleKey> : IInputDto<TRoleKey>
    {
        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        [DisplayName("编号")]
        public TRoleKey Id { get; set; }

        /// <summary>
        /// 获取或设置 角色名称
        /// </summary>
        [Required, DisplayName("角色名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 角色描述
        /// </summary>
        [StringLength(512)]
        [DisplayName("角色描述")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 是否管理员角色
        /// </summary>
        [DisplayName("是否管理")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获取或设置 是否默认角色，用户注册后拥有此角色
        /// </summary>
        [DisplayName("是否默认")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }
    }
}