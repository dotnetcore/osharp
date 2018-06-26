// -----------------------------------------------------------------------
//  <copyright file="IdentityViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-09 21:01</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Demo.Security.Dtos
{
    /// <summary>
    /// 用户设置权限DTO
    /// </summary>
    public class UserSetPermissionDto
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 要设置的角色编号
        /// </summary>
        public int[] RoleIds { get; set; }

        /// <summary>
        /// 获取或设置 要设置的模块编号
        /// </summary>
        public int[] ModuleIds { get; set; }
    }
}