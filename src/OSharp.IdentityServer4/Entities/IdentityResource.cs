// -----------------------------------------------------------------------
//  <copyright file="IdentityResource.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 1:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：身份资源
    /// </summary>
    [Description("身份资源")]
    [TableNamePrefix("Id4")]
    public class IdentityResource : EntityBase<int>
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
        /// 获取或设置 是否允许
        /// </summary>
        [DisplayName("是否允许")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 获取或设置 是否必须
        /// </summary>
        [DisplayName("是否必须")]
        public bool Required { get; set; }

        /// <summary>
        /// 获取或设置 是否强调
        /// </summary>
        [DisplayName("是否强调")]
        public bool Emphasize { get; set; }

        /// <summary>
        /// 获取或设置 是否在可发现文档中显示
        /// </summary>
        [DisplayName("是否在发现文档中显示")]
        public bool ShowInDiscoveryDocument { get; set; } = true;

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
        /// 获取或设置 是否禁止修改
        /// </summary>
        [DisplayName("是否禁止修改")]
        public bool NonEditable { get; set; }

        /// <summary>
        /// 获取或设置 身份声明集合
        /// </summary>
        [DisplayName("身份声明集合")]
        public virtual ICollection<IdentityClaim> UserClaims { get; set; }

        /// <summary>
        /// 获取或设置 身份资源属性集合
        /// </summary>
        [DisplayName("身份资源属性集合")]
        public virtual ICollection<IdentityResourceProperty> Properties { get; set; }
    }
}