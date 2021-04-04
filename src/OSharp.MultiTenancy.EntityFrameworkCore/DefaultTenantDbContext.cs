// -----------------------------------------------------------------------
//  <copyright file="DefaultTenantDbContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-03 1:23</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;

using OSharp.Entity;


namespace OSharp.MultiTenancy.EntityFrameworkCore
{
    /// <summary>
    /// 默认多租户数据上下文
    /// </summary>
    public class DefaultTenantDbContext : TenantDbContextBase
    {
        /// <summary>
        /// 初始化一个<see cref="DbContextBase"/>类型的新实例
        /// </summary>
        public DefaultTenantDbContext(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider)
        { }
    }
}