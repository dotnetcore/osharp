// -----------------------------------------------------------------------
//  <copyright file="ICorsInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-12-13 13:15</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace OSharp.AspNetCore.Cors
{
    /// <summary>
    /// 定义Cors初始化器
    /// </summary>
    public interface ICorsInitializer
    {
        /// <summary>
        /// 添加Cors
        /// </summary>
        IServiceCollection AddCors(IServiceCollection services);

        /// <summary>
        /// 应用Cors
        /// </summary>
        IApplicationBuilder UseCors(IApplicationBuilder app);
    }
}