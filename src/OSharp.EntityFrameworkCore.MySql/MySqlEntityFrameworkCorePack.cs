// -----------------------------------------------------------------------
//  <copyright file="MySqlEntityFrameworkCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using OSharp.Core.Packs;
using OSharp.Entity.KeyGenerate;


namespace OSharp.Entity.MySql
{
    /// <summary>
    /// MySqlEntityFrameworkCore模块
    /// </summary>
    [Description("MySqlEntityFrameworkCore模块")]
    public class MySqlEntityFrameworkCorePack : EntityFrameworkCorePackBase
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services = base.AddServices(services);

            services.AddSingleton<ISequentialGuidGenerator, MySqlSequentialGuidGenerator>();
            services.AddScoped(typeof(ISqlExecutor<,>), typeof(MySqlDapperSqlExecutor<,>));
            services.AddSingleton<IDbContextOptionsBuilderDriveHandler, MySqlDbContextOptionsBuilderDriveHandler>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            bool? hasMySql = provider.GetOSharpOptions()?.DbContexts?.Values.Any(m => m.DatabaseType == DatabaseType.MySql);
            if (hasMySql == null || !hasMySql.Value)
            {
                return;
            }

            base.UsePack(provider);
        }
    }
}