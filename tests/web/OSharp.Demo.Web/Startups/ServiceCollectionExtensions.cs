// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 23:45</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Demo.Identity;
using OSharp.Dependency;
using OSharp.Entity;

namespace Microsoft.AspNetCore.Builder
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 将OSharp服务添加到<see cref="IServiceCollection"/>
        /// </summary>
        public static IServiceCollection AddOSharp(this IServiceCollection services)
        {
            services.AddAppServices();
            
            return services;
        }
    }
}
