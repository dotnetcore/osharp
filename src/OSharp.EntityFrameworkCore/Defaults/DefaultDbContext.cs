// -----------------------------------------------------------------------
//  <copyright file="DefaultDbContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 1:50</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;


namespace OSharp.Entity
{
    /// <summary>
    /// 默认EntityFramework数据上下文
    /// </summary>
    public class DefaultDbContext : DbContextBase
    {
        /// <summary>
        /// 初始化一个<see cref="DefaultDbContext"/>类型的新实例
        /// </summary>
        public DefaultDbContext(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
            : base(options, typeFinder)
        {
            
        }
    }
}