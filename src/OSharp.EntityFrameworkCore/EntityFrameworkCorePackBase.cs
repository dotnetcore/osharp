// -----------------------------------------------------------------------
//  <copyright file="EntityFrameworkCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-14 15:57</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Core.Packs;
using OSharp.EventBuses;


namespace OSharp.Entity
{
    /// <summary>
    /// EntityFrameworkCore基模块
    /// </summary>
    [DependsOnPacks(typeof(EventBusPack))]
    public abstract class EntityFrameworkCorePackBase : OsharpPack
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
            services.TryAddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddOsharpDbContext<DefaultDbContext>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            IEntityManager manager = provider.GetService<IEntityManager>();
            manager?.Initialize();
            IsEnabled = true;
        }
    }
}