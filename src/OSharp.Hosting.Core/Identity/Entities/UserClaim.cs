// -----------------------------------------------------------------------
//  <copyright file="UserClaim.cs" company="OSharp开源团队">
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
    /// 实体类：用户声明信息
    /// </summary>
    [Description("用户声明信息")]
    public class UserClaim : UserClaimBase<int, int>
    {
        /// <summary>
        /// 获取或设置 所属用户
        /// </summary>
        public virtual User User { get; set; }
    }
}