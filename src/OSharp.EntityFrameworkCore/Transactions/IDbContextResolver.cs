// -----------------------------------------------------------------------
//  <copyright file="IDbContextResolver.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-19 19:11</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;

using OSharp.Entity.Transactions;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义从<see cref="IServiceProvider"/>中获取数据上下文对象的功能
    /// </summary>
    public interface IDbContextResolver
    {
        /// <summary>
        /// 获取指定类型的数据上下文对象
        /// </summary>
        /// <param name="resolveOptions">上下文解析选项</param>
        /// <returns></returns>
        IDbContext Resolve(DbContextResolveOptions resolveOptions);
    }
}