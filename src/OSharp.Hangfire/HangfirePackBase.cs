// -----------------------------------------------------------------------
//  <copyright file="HangfirePackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 19:11</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hangfire;

/// <summary>
/// Hangfire后台任务模块基类
/// </summary>
[DependsOnPacks(typeof(AspNetCorePack))]
public abstract class HangfirePackBase : AspOsharpPack
{
    /// <summary>
    /// 获取 模块级别，级别越小越先启动
    /// </summary>
    public override PackLevel Level => PackLevel.Framework;

    /// <summary>
    /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
    /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
    /// </summary>
    public override int Order => 0;

    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public override IServiceCollection AddServices(IServiceCollection services)
    {
        Action<IGlobalConfiguration> hangfireAction = GetHangfireAction(services);
        services.AddHangfire(hangfireAction);
        IConfiguration configuration = services.GetConfiguration();
        var optionsAction = GetBackgroundJobServerOptionsAction(configuration);
        services.AddHangfireServer(optionsAction);
        return services;
    }

    /// <summary>
    /// 应用AspNetCore的服务业务
    /// </summary>
    /// <param name="app">应用程序</param>
    public override void UsePack(WebApplication app)
    {
        IServiceProvider serviceProvider = app.Services;
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

        IGlobalConfiguration globalConfiguration = serviceProvider.GetService<IGlobalConfiguration>();
        globalConfiguration.UseLogProvider(new AspNetCoreLogProvider(serviceProvider.GetService<ILoggerFactory>()));

        string url = configuration["OSharp:Hangfire:DashboardUrl"].CastTo("/hangfire");
        DashboardOptions dashboardOptions = GetDashboardOptions(configuration);
        app.UseHangfireDashboard(url, dashboardOptions);

        IHangfireJobRunner jobRunner = serviceProvider.GetService<IHangfireJobRunner>();
        jobRunner?.Start();

        IsEnabled = true;
    }

    /// <summary>
    /// AddHangfire委托，重写可配置Hangfire服务，比如使用UseSqlServerStorage等
    /// </summary>
    /// <param name="services">服务容器</param>
    /// <returns></returns>
    protected virtual Action<IGlobalConfiguration> GetHangfireAction(IServiceCollection services)
    {
        IConfiguration configuration = services.GetConfiguration();
        string storageConnectionString = configuration["OSharp:Hangfire:StorageConnectionString"].CastTo<string>();
        if (storageConnectionString != null)
        {
            return config => config.UseSqlServerStorage(storageConnectionString);
        }

        return config => config.UseMemoryStorage();
    }

    /// <summary>
    /// 获取后台作业服务器选项
    /// </summary>
    /// <param name="configuration">系统配置信息</param>
    /// <returns></returns>
    protected virtual Action<IServiceProvider, BackgroundJobServerOptions> GetBackgroundJobServerOptionsAction(IConfiguration configuration)
    {
        int workerCount = configuration["OSharp:Hangfire:WorkerCount"].CastTo(0);
        return (provider, options) =>
        {
            if (workerCount > 0)
            {
                options.WorkerCount = workerCount;
            }
        };
    }

    /// <summary>
    /// 获取Hangfire仪表盘选项
    /// </summary>
    /// <param name="configuration">系统配置信息</param>
    /// <returns></returns>
    protected virtual DashboardOptions GetDashboardOptions(IConfiguration configuration)
    {
        string[] roles = configuration["OSharp:Hangfire:Roles"].CastTo("").Split(",", true);
        DashboardOptions dashboardOptions = new DashboardOptions();
        //限制角色存在时，才启用角色限制
        if (roles.Length > 0)
        {
            dashboardOptions.Authorization = new[] { new RoleDashboardAuthorizationFilter(roles) };
        }

        return dashboardOptions;
    }
}
