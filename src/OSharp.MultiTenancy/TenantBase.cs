// -----------------------------------------------------------------------
//  <copyright file="TenantBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-03 1:14</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 租户信息基类
    /// </summary>
    public abstract class TenantBase<TTenantKey> : EntityBase<TTenantKey>
        where TTenantKey : IEquatable<TTenantKey>
    {
        /// <summary>
        /// 获取或设置 租户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 租户域名
        /// </summary>
        public string Host { get; set; }
    }
}