using System;
using System.ComponentModel;

using OSharp.Core.Packs;
using OSharp.Entity;
using OSharp.Entity.Sqlite;


namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// Sqlite-DefaultDbContext迁移模块
    /// </summary>
    [DependsOnPacks(typeof(SqliteEntityFrameworkCorePack))]
    [Description("Sqlite-DefaultDbContext迁移模块")]
    public class SqliteDefaultDbContextMigrationPack : MigrationPackBase<DefaultDbContext>
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// 获取 数据库类型
        /// </summary>
        protected override DatabaseType DatabaseType => DatabaseType.Sqlite;

        /// <summary>
        /// 重写实现获取数据上下文实例
        /// </summary>
        /// <param name="scopedProvider">服务提供者</param>
        /// <returns></returns>
        protected override DefaultDbContext CreateDbContext(IServiceProvider scopedProvider)
        {
            return new DesignTimeDefaultDbContextFactory(scopedProvider).CreateDbContext(new string[0]);
        }
    }
}
