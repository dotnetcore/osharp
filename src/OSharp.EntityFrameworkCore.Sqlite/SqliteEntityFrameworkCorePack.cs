// -----------------------------------------------------------------------
//  <copyright file="SqliteEntityFrameworkCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-11-05 16:29</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity.Sqlite
{
    /// <summary>
    /// SqliteEntityFrameworkCore模块
    /// </summary>
    [Description("SqliteEntityFrameworkCore模块")]
    public class SqliteEntityFrameworkCorePack : EntityFrameworkCorePackBase
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 获取 数据库类型
        /// </summary>
        protected override DatabaseType DatabaseType => DatabaseType.Sqlite;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services = base.AddServices(services);

            services.AddSingleton<ISequentialGuidGenerator, SqliteSequentialGuidGenerator>();
            services.AddScoped(typeof(ISqlExecutor<,>), typeof(SqliteDapperSqlExecutor<,>));
            services.AddSingleton<IDbContextOptionsBuilderDriveHandler, SqliteDbContextOptionsBuilderDriveHandler>();

            return services;
        }
    }
}
