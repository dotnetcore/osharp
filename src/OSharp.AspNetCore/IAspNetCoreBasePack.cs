// -----------------------------------------------------------------------
//  <copyright file="AspNetCoreBasePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-24 22:26</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 基于AspNetCore的执行<see cref="IAspNetCoreBasePack"/>逻辑的模块接口
    /// </summary>
    public interface IAspNetCoreBasePack
    {
        /// <summary>
        /// 应用AspNetCore的模块初始化逻辑
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        /// <returns>应用程序构建器</returns>
        IApplicationBuilder UsePack(IApplicationBuilder app);
    }
}