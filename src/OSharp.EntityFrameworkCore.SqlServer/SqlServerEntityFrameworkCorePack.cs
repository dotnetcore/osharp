// -----------------------------------------------------------------------
//  <copyright file="SqlServerEntityFrameworkCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:24</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity.SqlServer;

/// <summary>
/// SqlServerEntityFrameworkCore模块
/// </summary>
[Description("SqlServerEntityFrameworkCore模块")]
public class SqlServerEntityFrameworkCorePack : EntityFrameworkCorePackBase
{
    /// <summary>
    /// 获取 模块级别
    /// </summary>
    public override PackLevel Level => PackLevel.Framework;

    /// <summary>
    /// 获取 数据库类型
    /// </summary>
    protected override DatabaseType DatabaseType => DatabaseType.SqlServer;

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
        services.AddSingleton<ISequentialGuidGenerator, SqlServerSequentialGuidGenerator>();
        services.AddScoped(typeof(ISqlExecutor<,>), typeof(SqlServerDapperSqlExecutor<,>));
        services.AddSingleton<IDbContextOptionsBuilderDriveHandler, SqlServerDbContextOptionsBuilderDriveHandler>();

        return services;
    }
}
