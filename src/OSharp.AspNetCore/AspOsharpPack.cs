// -----------------------------------------------------------------------
//  <copyright file="AspOsharpPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-10 19:07</last-date>
// -----------------------------------------------------------------------

namespace OSharp.AspNetCore;

/// <summary>
///  基于AspNetCore环境的Pack模块基类
/// </summary>
public abstract class AspOsharpPack : OsharpPack
{
    /// <summary>
    /// 应用AspNetCore的服务业务
    /// </summary>
    /// <param name="app">Web应用程序</param>
    public virtual void UsePack(WebApplication app)
    {
        IServiceProvider provider = app.Services;
        base.UsePack(provider);
    }
}
