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
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;


namespace OSharp.Entity.MySql
{
    /// <summary>
    /// MySql<see cref="DbContextOptionsBuilder"/>数据库驱动差异处理器
    /// </summary>
    [Dependency(ServiceLifetime.Singleton)]
    public class DbContextOptionsBuilderDriveHandler : IDbContextOptionsBuilderDriveHandler
    {
        /// <summary>
        /// 获取 数据库类型名称，如 SQLSERVER，MYSQL，SQLITE等
        /// </summary>
        public DatabaseType Type { get; } = DatabaseType.MySql;

        /// <summary>
        /// 处理<see cref="DbContextOptionsBuilder"/>驱动差异
        /// </summary>
        /// <param name="builder">创建器</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="existingConnection">已存在的连接对象</param>
        /// <returns></returns>
        public DbContextOptionsBuilder Handle(DbContextOptionsBuilder builder, string connectionString, DbConnection existingConnection)
        {
            if (existingConnection == null)
            {
                return builder.UseMySql(connectionString);
            }
            return builder.UseMySql(existingConnection);
        }
    }
}