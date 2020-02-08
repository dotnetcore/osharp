// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-31 0:45</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// <see cref="IServiceCollection"/>扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 获取<see cref="IHostingEnvironment"/>环境信息
        /// </summary>
        public static IWebHostEnvironment GetWebHostEnvironment(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IWebHostEnvironment>();
        }
    }
}