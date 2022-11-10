// -----------------------------------------------------------------------
//  <copyright file="EndpointsPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 19:05</last-date>
// -----------------------------------------------------------------------

namespace OSharp.AspNetCore.Routing;

/// <summary>
/// Endpoints模块
/// </summary>
[Description("Endpoints模块")]
public class EndpointsPack : EndpointsPackBase
{
    /// <summary>
    /// 重写以配置SignalR的路由
    /// </summary>
    /// <param name="app">Web应用程序</param>
    protected override WebApplication MapSignalR(WebApplication app)
    {
        // 在这实现Hub的路由映射
        // 例如：app.MapHub<ChatHub>();
        return app;
    }
}