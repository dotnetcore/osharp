// -----------------------------------------------------------------------
//  <copyright file="IAspUsePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-10 0:31</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Builder;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 定义AspNetCore环境下的应用模块服务 
    /// </summary>
    public interface IAspUsePack
    {
        /// <summary>
        /// 应用模块服务，仅在AspNetCore环境下调用，非AspNetCore环境请执行 UsePack(IServiceProvider) 功能
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        void UsePack(IApplicationBuilder app);
    }
}