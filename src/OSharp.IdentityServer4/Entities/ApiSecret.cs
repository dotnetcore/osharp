// -----------------------------------------------------------------------
//  <copyright file="ApiSecret.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 23:22</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：API密钥
    /// </summary>
    [Description("API密钥")]
    [TableNamePrefix("Id4")]
    public class ApiSecret : Secret
    {
        /// <summary>
        /// 获取或设置 API资源编号
        /// </summary>
        [DisplayName("API资源编号")]
        public int ApiResourceId { get; set; }

        /// <summary>
        /// 获取或设置 所属API资源
        /// </summary>
        [DisplayName("API资源")]
        public virtual ApiResource ApiResource { get; set; }
    }
}