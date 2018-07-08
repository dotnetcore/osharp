// -----------------------------------------------------------------------
//  <copyright file="EntityFrameworkCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:24</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Packs;


namespace OSharp.Entity
{
    /// <summary>
    /// EntityFrameworkCore模块
    /// </summary>
    public class EntityFrameworkCorePack : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IEntityConfigurationTypeFinder, EntityConfigurationTypeFinder>();
            services.AddSingleton<IDbContextResolver, DbContextResolver>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            return services;
        }
    }
}