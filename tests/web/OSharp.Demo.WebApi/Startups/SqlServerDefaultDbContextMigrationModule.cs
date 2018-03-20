using System;
using OSharp.Entity;
using OSharp.Entity.SqlServer;

namespace OSharp.Demo.WebApi.Startups
{
    /// <summary>
    /// SqlServer-DefaultDbContext迁移模块
    /// </summary>
    public class SqlServerDefaultDbContextMigrationModule : SqlServerMigrationModuleBase<DefaultDbContext>
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        protected override DefaultDbContext CreateDbContext(IServiceProvider provider)
        {
            return new SqlServerDesignTimeDefaultDbContextFactory(provider).CreateDbContext(new string[0]);
        }
    }
}