// -----------------------------------------------------------------------
//  <copyright file="IdentityClaim.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 1:33</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：身份声明
    /// </summary>
    [Description("身份声明")]
    [TableNamePrefix("Id4")]
    public class IdentityClaim : UserClaim
    {
        /// <summary>
        /// 获取或设置 身份资源编号
        /// </summary>
        [DisplayName("身份资源编号")]
        public int IdentityResourceId { get; set; }

        /// <summary>
        /// 获取或设置 身份资源
        /// </summary>
        [DisplayName("身份资源")]
        public virtual IdentityResource IdentityResource { get; set; }
    }
}