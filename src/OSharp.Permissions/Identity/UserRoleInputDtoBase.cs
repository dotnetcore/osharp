// -----------------------------------------------------------------------
//  <copyright file="UserRoleInputDtoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 14:37</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.Identity
{
    /// <summary>
    /// 用户角色输入DTO基类
    /// </summary>
    public abstract class UserRoleInputDtoBase<TUserKey, TRoleKey> : IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 获取或设置 角色编号
        /// </summary>
        public TRoleKey RoleId { get; set; }

        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }
    }
}