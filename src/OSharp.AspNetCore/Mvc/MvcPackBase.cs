// -----------------------------------------------------------------------
//  <copyright file="MvcPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 19:08</last-date>
// -----------------------------------------------------------------------

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore.Cors;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.Mvc.ModelBinding;
using OSharp.Core.Options;


namespace OSharp.AspNetCore.Mvc;

/// <summary>
/// Mvc模块基类
/// </summary>
[DependsOnPacks(typeof(AspNetCorePack))]
public abstract class MvcPackBase : AspOsharpPack
{
    private ICorsInitializer _corsInitializer;

    /// <summary>
    /// 获取 模块级别，级别越小越先启动
    /// </summary>
    public override PackLevel Level => PackLevel.Application;

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
        _corsInitializer = services.GetOrAddSingletonInstance(() => (ICorsInitializer)new DefaultCorsInitializer());
        _corsInitializer.AddCors(services);

        OsharpOptions osharp = services.GetOsharpOptions();
        services.AddControllersWithViews()
            .AddControllersAsServices()
            .AddNewtonsoftJson(options =>
            {
                if (osharp.Mvc?.IsLowercaseJsonProperty == false)
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }
                if (osharp.Mvc?.IsLongToStringConvert == true)
                {
                    //处理雪花算法生成的Id在前端丢失精度的问题
                    options.SerializerSettings.Converters.Add(new LongToStringConverter());
                }
            });

        services.AddRouting(opts => opts.LowercaseUrls = osharp.Mvc?.IsLowercaseUrls ?? false);

        services.AddHttpsRedirection(opts => opts.HttpsPort = 443);

        services.AddScoped<UnitOfWorkImpl>();
        services.AddTransient<UnitOfWorkAttribute>();
        services.TryAddSingleton<IVerifyCodeService, VerifyCodeService>();
        services.TryAddSingleton<IScopedServiceResolver, RequestScopedServiceResolver>();
        services.Replace<ICancellationTokenProvider, HttpContextCancellationTokenProvider>(ServiceLifetime.Singleton);
        services.Replace<IHybridServiceScopeFactory, HttpContextServiceScopeFactory>(ServiceLifetime.Singleton);

        return services;
    }

    /// <summary>
    /// 应用模块服务
    /// </summary>
    /// <param name="app">应用程序</param>
    public override void UsePack(WebApplication app)
    {
        _corsInitializer.UseCors(app);

        IsEnabled = true;
    }
}
