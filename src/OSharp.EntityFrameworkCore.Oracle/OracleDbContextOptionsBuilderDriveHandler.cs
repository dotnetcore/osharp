using System;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Oracle.EntityFrameworkCore.Infrastructure;

using OSharp.Dependency;


namespace OSharp.Entity.Oracle
{
    /// <summary>
    /// Oracle<see cref="DbContextOptionsBuilder"/>数据库驱动差异处理器
    /// </summary>
    [Dependency(ServiceLifetime.Singleton)]
    public class OracleDbContextOptionsBuilderDriveHandler : IDbContextOptionsBuilderDriveHandler
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="OracleDbContextOptionsBuilderDriveHandler"/>类型的新实例
        /// </summary>
        public OracleDbContextOptionsBuilderDriveHandler(IServiceProvider provider)
        {
            _logger = provider.GetLogger(this);
        }

        /// <summary>
        /// 获取 数据库类型名称，如 SQLSERVER，MYSQL，SQLITE等
        /// </summary>
        public DatabaseType Type { get; } = DatabaseType.Oracle;

        /// <summary>
        /// 处理<see cref="DbContextOptionsBuilder"/>驱动差异
        /// </summary>
        /// <param name="builder">创建器</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="existingConnection">已存在的连接对象</param>
        /// <returns></returns>
        public virtual DbContextOptionsBuilder Handle(DbContextOptionsBuilder builder, string connectionString, DbConnection existingConnection)
        {
            DbContextOptionsBuilderAction(builder);
            Action<OracleDbContextOptionsBuilder> action = null;
            if (ServiceExtensions.MigrationAssemblyName != null)
            {
                action = b =>
                {
                    b.MigrationsAssembly(ServiceExtensions.MigrationAssemblyName);
                    OracleDbContextOptionsBuilderAction(b);
                };
            }

            if (existingConnection == null)
            {
                _logger.LogDebug($"使用新连接“{connectionString}”应用Oracle数据库");
                builder.UseOracle(connectionString, action);
            }
            else
            {
                _logger.LogDebug($"使用已存在的连接“{existingConnection.ConnectionString}”应用Oracle数据库");
                builder.UseOracle(existingConnection, action);
            }

            ServiceExtensions.MigrationAssemblyName = null;
            return builder;
        }

        /// <summary>
        /// 重写以实现<see cref="OracleDbContextOptionsBuilder"/>的自定义行为
        /// </summary>
        protected virtual void OracleDbContextOptionsBuilderAction(OracleDbContextOptionsBuilder options)
        { }

        /// <summary>
        /// 重写以实现<see cref="DbContextOptionsBuilder"/>的自定义行为
        /// </summary>
        protected virtual void DbContextOptionsBuilderAction(DbContextOptionsBuilder builder)
        { }
    }
}