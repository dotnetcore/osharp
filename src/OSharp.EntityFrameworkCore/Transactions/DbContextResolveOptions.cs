// -----------------------------------------------------------------------
//  <copyright file="DbContextResolveOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-22 14:55</last-date>
// -----------------------------------------------------------------------

using System;
using System.Data.Common;

using OSharp.Data;


namespace OSharp.Entity.Transactions
{
    /// <summary>
    /// 数据上下文创建选项
    /// </summary>
    public class DbContextResolveOptions
    {
        /// <summary>
        /// 初始化一个<see cref="DbContextResolveOptions"/>类型的新实例
        /// </summary>
        public DbContextResolveOptions()
        { }

        /// <summary>
        /// 初始化一个<see cref="DbContextResolveOptions"/>类型的新实例
        /// </summary>
        public DbContextResolveOptions(OsharpDbContextConfig config)
        {
            DbContextType = config.DbContextType;
            ConnectionString = config.ConnectionString;
            DatabaseType = config.DatabaseType;
        }

        /// <summary>
        /// 获取或设置 上下文类型
        /// </summary>
        public Type DbContextType { get; set; }

        /// <summary>
        /// 获取或设置 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置 已存在的连接对象
        /// </summary>
        public DbConnection ExistingConnection { get; set; }

        /// <summary>
        /// 获取或设置 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }
    }
}