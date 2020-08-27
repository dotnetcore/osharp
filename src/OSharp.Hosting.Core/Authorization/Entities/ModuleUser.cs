// -----------------------------------------------------------------------
//  <copyright file="ModuleUser.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-06 15:25</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Hosting.Identity.Entities;

using OSharp.Authorization.Entities;


namespace OSharp.Hosting.Authorization.Entities
{
    /// <summary>
    /// 实体类：模块用户信息
    /// </summary>
    [Description("用户模块信息")]
    public class ModuleUser : ModuleUserBase<int, int>
    {
        /// <summary>
        /// 获取或设置 模块信息
        /// </summary>
        public virtual Module Module { get; set; }

        /// <summary>
        /// 获取或设置 用户信息
        /// </summary>
        public virtual User User { get; set; }
    }
}