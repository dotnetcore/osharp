// -----------------------------------------------------------------------
//  <copyright file="FunctionAccessType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:13</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;


namespace OSharp.Authorization.Functions
{
    /// <summary>
    /// 功能访问类型
    /// </summary>
    public enum FunctionAccessType
    {
        /// <summary>
        /// 匿名用户可访问
        /// </summary>
        [Description("匿名访问")] Anonymous = 0,

        /// <summary>
        /// 登录用户可访问
        /// </summary>
        [Description("登录访问")] LoggedIn = 1,

        /// <summary>
        /// 指定角色可访问
        /// </summary>
        [Description("角色访问")] RoleLimit = 2
    }
}