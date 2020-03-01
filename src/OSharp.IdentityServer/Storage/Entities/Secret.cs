// -----------------------------------------------------------------------
//  <copyright file="Secret.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 23:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 密钥基类
    /// </summary>
    public abstract class Secret : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 描述
        /// </summary>
        [DisplayName("描述"), StringLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置 密钥
        /// </summary>
        [DisplayName("密钥"), StringLength(4000), Required]
        public string Value { get; set; }

        /// <summary>
        /// 获取或设置 过期时间
        /// </summary>
        [DisplayName("过期时间")]
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// 获取或设置 密钥类型
        /// </summary>
        [DisplayName("密钥类型"), StringLength(250), Required]
        public string Type { get; set; } = "SharedSecret";

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}