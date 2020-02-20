// -----------------------------------------------------------------------
//  <copyright file="ApiScope.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 23:42</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer4.Entities
{
    /// <summary>
    /// 实体类：API作用域
    /// </summary>
    [Description("API作用域")]
    [TableNamePrefix("Id4")]
    public class ApiScope : EntityBase<int>
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
        /// 获取或设置 是否必须
        /// </summary>
        [DisplayName("是否必须")]
        public bool Required { get; set; }

        /// <summary>
        /// 获取或设置 是否着重
        /// </summary>
        [DisplayName("是否着重")]
        public bool Emphasize { get; set; }

        /// <summary>
        /// 获取或设置 是否在可发现文档中显示
        /// </summary>
        [DisplayName("是否在发现文档中显示")]
        public bool ShowInDiscoveryDocument { get; set; } = true;

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

        /// <summary>
        /// 获取或设置 API作用域声明集合
        /// </summary>
        public virtual ICollection<ApiScopeClaim> UserClaims { get; set; }

    }
}