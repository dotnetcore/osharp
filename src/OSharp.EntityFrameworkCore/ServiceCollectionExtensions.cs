// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 4:09</last-date>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OSharp.Entity
{
    /// <summary>
    /// ServiceCollection扩展类
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 将EntityFrameworkCore的服务添加到<see cref="IServiceCollection"/>
        /// </summary>
        public static IServiceCollection AddOSharpEntityServices(this IServiceCollection services)
        {
            services.AddSingleton<IEntityConfigurationAssemblyFinder, EntityConfigurationAssemblyFinder>();
            services.AddSingleton<IEntityConfigurationTypeFinder, EntityConfigurationTypeFinder>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            
            return services;
        }
    }
}