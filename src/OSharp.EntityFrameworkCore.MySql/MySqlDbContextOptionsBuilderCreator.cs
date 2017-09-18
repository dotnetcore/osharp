using Microsoft.EntityFrameworkCore;
using OSharp.Dependency;
using OSharp.Entity;
using System;
using System.Data.Common;

namespace OSharp.Entity.MySql
{
    public class MySqlDbContextOptionsBuilderCreator : IDbContextOptionsBuilderCreator, ISingletonDependency
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