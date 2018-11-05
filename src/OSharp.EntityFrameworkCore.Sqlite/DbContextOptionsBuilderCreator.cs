// -----------------------------------------------------------------------
//  <copyright file="DbContextOptionsBuilderCreator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-11-05 14:25</last-date>
// -----------------------------------------------------------------------

using System.Data.Common;

using Microsoft.EntityFrameworkCore;


namespace OSharp.Entity.Sqlite
{
    /// <summary>
    /// Sqlite的<see cref="DbContextOptionsBuilder"/>创建器
    /// </summary>
    public class DbContextOptionsBuilderCreator : IDbContextOptionsBuilderCreator
    {
        /// <summary>
        /// 获取 数据库类型名称，如SqlServer，MySql，Sqlite等
        /// </summary>
        public DatabaseType Type { get; } = DatabaseType.Sqlite;

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
                return optionsBuilder.UseSqlite(connectionString);
            }
            return new DbContextOptionsBuilder().UseSqlite(existingConnection);
        }
    }
}