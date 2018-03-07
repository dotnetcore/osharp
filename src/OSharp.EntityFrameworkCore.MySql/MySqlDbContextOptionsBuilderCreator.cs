// -----------------------------------------------------------------------
//  <copyright file="MySqlDbContextOptionsBuilderCreator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>hejiyong</last-editor>
//  <last-date>2017-09-19 5:09</last-date>
// -----------------------------------------------------------------------

using System.Data.Common;

using Microsoft.EntityFrameworkCore;


namespace OSharp.Entity.MySql
{
    /// <summary>
    /// MySql上下文选项建立者创建器
    /// </summary>
    public class MySqlDbContextOptionsBuilderCreator : IDbContextOptionsBuilderCreator
    {
        /// <summary>
        /// 获取 数据库类型名称，如 SQLSERVER，MYSQL，SQLITE等
        /// </summary>
        public DatabaseType Type { get; } = DatabaseType.MySql;

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
                return new DbContextOptionsBuilder().UseMySql(connectionString);
            }
            return new DbContextOptionsBuilder().UseMySql(existingConnection);
        }
    }
}