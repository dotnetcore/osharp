// -----------------------------------------------------------------------
//  <copyright file="RoleClaim.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Identity.Entities;


namespace OSharp.Hosting.Identity.Entities
{
    /// <summary>
    /// 实体类：角色声明信息
    /// </summary>
    [Description("角色声明信息")]
    public class RoleClaim : RoleClaimBase<int, int>
    {
        /// <summary>
        /// 获取或设置 所属角色信息
        /// </summary>
        public virtual Role Role { get; set; }
    }
}