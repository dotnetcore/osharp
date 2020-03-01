// -----------------------------------------------------------------------
//  <copyright file="ApiResource.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 22:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：API资源
    /// </summary>
    [Description("API资源")]
    [TableNamePrefix("Id4")]
    public class ApiResource : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        [DisplayName("名称"), Required, StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 显示名称
        /// </summary>
        [DisplayName("显示名称"), StringLength(200)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取或设置 描述
        /// </summary>
        [DisplayName("描述"), StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置 访问令牌签名算法。如果为空，将使用服务器默认签名算法。
        /// </summary>
        [DisplayName("访问令牌签名算法"), StringLength(100)]
        public string AllowedAccessTokenSigningAlgorithms { get; set; }

        /// <summary>
        /// 获取或设置 是否可用
        /// </summary>
        [DisplayName("是否可用")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime? Updated { get; set; }

        /// <summary>
        /// 获取或设置 最后访问时间
        /// </summary>
        [DisplayName("最后访问时间")]
        public DateTime? LastAccessed { get; set; }

        /// <summary>
        /// 获取或设置 是否禁止修改
        /// </summary>
        [DisplayName("是否禁止修改")]
        public bool NonEditable { get; set; }

        /// <summary>
        /// 获取或设置 API密钥集合
        /// </summary>
        public virtual ICollection<ApiSecret> Secrets { get; set; }

        /// <summary>
        /// 获取或设置 API作用域集合
        /// </summary>
        public virtual ICollection<ApiScope> Scopes { get; set; }

        /// <summary>
        /// 获取或设置 API资源声明集合
        /// </summary>
        public virtual ICollection<ApiResourceClaim> UserClaims { get; set; }

        /// <summary>
        /// 获取或设置 API资源属性集合
        /// </summary>
        public virtual ICollection<ApiResourceProperty> Properties { get; set; }
    }
}