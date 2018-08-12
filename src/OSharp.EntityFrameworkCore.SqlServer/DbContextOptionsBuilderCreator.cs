// -----------------------------------------------------------------------
//  <copyright file="SqlServerDbContextOptionsBuilderCreator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 1:07</last-date>
// -----------------------------------------------------------------------

using System;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Entity.SqlServer
{
    /// <summary>
    /// SqlServer的<see cref="DbContextOptionsBuilder"/>创建器
    /// </summary>
    public class DbContextOptionsBuilderCreator : IDbContextOptionsBuilderCreator
    {
        /// <summary>
        /// 获取 数据库类型名称，如 SQLSERVER，MYSQL，SQLITE等
        /// </summary>
        public DatabaseType Type { get; } = DatabaseType.SqlServer;

        /// <summary>
        /// 创建<see cref="DbContextOptionsBuilder"/>对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="existingConnection">已存在的连接对象</param>
        /// <returns></returns>
        public DbContextOptionsBuilder Create(string connectionString, DbConnection existingConnection)
        {
            if (existingConnection == null)
            {
                DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
                
                return optionsBuilder.UseSqlServer(connectionString, builder => builder.UseRowNumberForPaging());
            }
            return new DbContextOptionsBuilder().UseSqlServer(existingConnection, builder => builder.UseRowNumberForPaging());
        }
    }
}