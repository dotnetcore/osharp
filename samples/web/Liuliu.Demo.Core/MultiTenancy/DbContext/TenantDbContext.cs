// -----------------------------------------------------------------------
//  <copyright file="TenantDbContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-08 4:44</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace OSharp.Entity;

/// <summary>
/// 默认EntityFramework数据上下文
/// </summary>
public class TenantDbContext : DbContextBase
{
    /// <summary>
    /// 初始化一个<see cref="TenantDbContext"/>类型的新实例
    /// </summary>
    public TenantDbContext(DbContextOptions<TenantDbContext> options, IServiceProvider serviceProvider)
        : base(options, serviceProvider)
    { }
}
