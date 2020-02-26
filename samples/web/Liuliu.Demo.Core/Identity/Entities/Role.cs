// -----------------------------------------------------------------------
//  <copyright file="Role.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using OSharp.Identity.Entities;


namespace Liuliu.Demo.Identity.Entities
{
    /// <summary>
    /// 实体类：角色信息
    /// </summary>
    [Description("角色信息")]
    public class Role : RoleBase<int>
    {
        /// <summary>
        /// 获取或设置 分配的用户角色信息集合
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        /// <summary>
        /// 获取或设置 角色声明信息集合
        /// </summary>
        public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}