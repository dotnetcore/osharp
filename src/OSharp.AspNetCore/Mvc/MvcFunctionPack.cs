// -----------------------------------------------------------------------
//  <copyright file="MvcFunctionPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 19:08</last-date>
// -----------------------------------------------------------------------

using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;


namespace OSharp.AspNetCore.Mvc;

/// <summary>
/// MVC功能点模块
/// </summary>
[DependsOnPacks(typeof(MvcPack))]
[Description("MVC功能点模块")]
public class MvcFunctionPack : AspOsharpPack
{
    /// <summary>
    /// 获取 模块级别
    /// </summary>
    public override PackLevel Level => PackLevel.Application;

    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public override IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<IFunctionHandler, MvcFunctionHandler>();
        services.TryAddSingleton<IModuleInfoPicker, MvcModuleInfoPicker>();

        return services;
    }

    /// <summary>
    /// 应用模块服务
    /// </summary>
    /// <param name="app">Web应用程序</param>
    public override void UsePack(WebApplication app)
    {
        IServiceProvider provider = app.Services;
        IFunctionHandler functionHandler =
            provider.GetServices<IFunctionHandler>().FirstOrDefault(m => m.GetType() == typeof(MvcFunctionHandler));
        if (functionHandler == null)
        {
            return;
        }

        functionHandler.Initialize();

        IsEnabled = true;
    }
}