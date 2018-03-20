// -----------------------------------------------------------------------
//  <copyright file="SqlServerEntityFrameworkCoreModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 21:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.Core.Modules;
using OSharp.Core.Options;


namespace OSharp.Entity.SqlServer
{
    /// <summary>
    /// SqlServerEntityFrameworkCore模块
    /// </summary>
    [DependsOnModules(typeof(EntityFrameworkCoreModule))]
    public class SqlServerEntityFrameworkCoreModule : OSharpModule
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override ModuleLevel Level => ModuleLevel.Framework;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IDbContextOptionsBuilderCreator, DbContextOptionsBuilderCreator>();
            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public override void UseModule(IServiceProvider provider)
        {
            using (provider.GetService<IServiceScopeFactory>().CreateScope())
            {
                DefaultDbContext context = CreateDbContext(provider);
                //context?.CheckAndMigration();
            }

            IsEnabled = true;
        }

        private static DefaultDbContext CreateDbContext(IServiceProvider provider)
        {
            string connString = AppSettingsManager.Get("OSharp:DbContexts:SqlServer:ConnectionString")
                ?? AppSettingsManager.Get("ConnectionStrings:DefaultDbContext");
            if (connString == null)
            {
                return null;
            }
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<DefaultDbContext>();
            string entryAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            builder.UseSqlServer(connString, b => b.MigrationsAssembly(entryAssemblyName));
            IEntityConfigurationTypeFinder typeFinder = provider.GetService<IEntityConfigurationTypeFinder>();

            DefaultDbContext context = new DefaultDbContext(builder.Options, typeFinder);
            return context;
        }
    }
}