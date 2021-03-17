// -----------------------------------------------------------------------
//  <copyright file="DbContextOptionsBuilderDriveHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-21 13:37</last-date>
// -----------------------------------------------------------------------

using System;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace OSharp.Entity.Sqlite
{
    /// <summary>
    /// Sqlite的<see cref="DbContextOptionsBuilder"/>数据库驱动差异处理器
    /// </summary>
    public class SqliteDbContextOptionsBuilderDriveHandler : IDbContextOptionsBuilderDriveHandler
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="SqliteDbContextOptionsBuilderDriveHandler"/>类型的新实例
        /// </summary>
        public SqliteDbContextOptionsBuilderDriveHandler(IServiceProvider provider)
        {
            _logger = provider.GetLogger(this);
        }

        /// <summary>
        /// 获取 数据库类型名称，如 SQLSERVER，MYSQL，SQLITE等
        /// </summary>
        public DatabaseType Type { get; } = DatabaseType.Sqlite;

        /// <summary>
        /// 处理<see cref="DbContextOptionsBuilder"/>驱动差异
        /// </summary>
        /// <param name="builder">创建器</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="existingConnection">已存在的连接对象</param>
        /// <returns></returns>
        public DbContextOptionsBuilder Handle(DbContextOptionsBuilder builder, string connectionString, DbConnection existingConnection)
        {
            DbContextOptionsBuilderAction(builder);
            Action<SqliteDbContextOptionsBuilder> action = null;
            if (ServiceExtensions.MigrationAssemblyName != null)
            {
                action = b =>
                {
                    b.MigrationsAssembly(ServiceExtensions.MigrationAssemblyName);
                    SqliteDbContextOptionsBuilderAction(b);
                };
            }

            if (existingConnection == null)
            {
                _logger.LogDebug($"使用新连接“{connectionString}”应用Sqlite数据库");
                builder.UseSqlite(connectionString, action);
            }
            else
            {
                _logger.LogDebug($"使用已存在的连接“{existingConnection.ConnectionString}”应用Sqlite数据库");
                builder.UseSqlite(existingConnection, action);
            }

            ServiceExtensions.MigrationAssemblyName = null;
            return builder;
        }

        /// <summary>
        /// 重写以实现<see cref="SqliteDbContextOptionsBuilder"/>的自定义行为
        /// </summary>
        protected virtual void SqliteDbContextOptionsBuilderAction(SqliteDbContextOptionsBuilder options)
        { }

        /// <summary>
        /// 重写以实现<see cref="DbContextOptionsBuilder"/>的自定义行为
        /// </summary>
        protected virtual void DbContextOptionsBuilderAction(DbContextOptionsBuilder builder)
        { }
    }
}