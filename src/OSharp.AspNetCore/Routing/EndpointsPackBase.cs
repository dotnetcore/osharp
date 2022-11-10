// -----------------------------------------------------------------------
//  <copyright file="EndpointsPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 19:06</last-date>
// -----------------------------------------------------------------------

namespace OSharp.AspNetCore.Routing;

/// <summary>
/// Endpoints模块，处理MVC和SignalR的路由结点配置
/// </summary>
[DependsOnPacks(typeof(AspNetCorePack))]
public abstract class EndpointsPackBase : AspOsharpPack
{
    /// <summary>
    /// 获取 模块级别，级别越小越先启动
    /// </summary>
    public override PackLevel Level => PackLevel.Application;

    /// <summary>
    /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
    /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
    /// </summary>
    public override int Order => 99;
        
    /// <summary>
    /// 应用AspNetCore的服务业务
    /// </summary>
    /// <param name="app">Web应用程序</param>
    public override void UsePack(WebApplication app)
    {
        MapMvc(app);
        MapSignalR(app);
        MapOther(app);

        IsEnabled = true;
    }

    /// <summary>
    /// 重写以配置MVC的路由
    /// </summary>
    /// <param name="app">Web应用程序</param>
    protected virtual WebApplication MapMvc(WebApplication app)
    {
        app.MapControllersWithAreaRoute();

        return app;
    }

    /// <summary>
    /// 重写以配置SignalR的路由
    /// </summary>
    /// <param name="app">Web应用程序</param>
    protected virtual WebApplication MapSignalR(WebApplication app)
    {
        return app;
    }

    /// <summary>
    /// 重写以配置其他路由
    /// </summary>
    /// <param name="app">Web应用程序</param>
    /// <returns></returns>
    protected virtual WebApplication MapOther(WebApplication app)
    {
        return app;
    }
}