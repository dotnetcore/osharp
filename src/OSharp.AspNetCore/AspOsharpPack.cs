// -----------------------------------------------------------------------
//  <copyright file="AspOsharpPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-09 22:20</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Builder;

using OSharp.Core.Packs;


namespace OSharp.AspNetCore
{
    /// <summary>
    ///  基于AspNetCore环境的Pack模块基类
    /// </summary>
    public abstract class AspOsharpPack : OsharpPack
    {
#if NET6_0_OR_GREATER
        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Web应用程序</param>
        public virtual void UsePack(WebApplication app)
#else
        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Web应用程序构建器</param>
        public virtual void UsePack(IApplicationBuilder app)
#endif
        {
#if NET6_0_OR_GREATER
            IServiceProvider provider = app.Services;
#else
            IServiceProvider provider = app.ApplicationServices;
#endif
            base.UsePack(provider);
        }
    }
}