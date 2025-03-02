// -----------------------------------------------------------------------
//  <copyright file="DesignTimeDefaultDbContextFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-25 23:16</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.AutoMapper;
using OSharp.Core.Packs;
using OSharp.Entity;
using OSharp.Entity.Sqlite;
using OSharp.Log4Net;
using OSharp.Reflection;


namespace Liuliu.Demo.Web.Startups
{
    public class DesignTimeTenantDbContextFactory : DesignTimeDbContextFactoryBase<TenantDbContext>
    {
        public DesignTimeTenantDbContextFactory()
            : base(null)
        { }

        public DesignTimeTenantDbContextFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        /// <summary>
        /// 创建设计时使用的ServiceProvider，主要用于执行 Add-Migration 功能
        /// </summary>
        /// <returns></returns>
        protected override IServiceProvider CreateDesignTimeServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // 配置过滤器，替代原 Startup 中的配置
            string[] filters = { "dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows", "PropertyChanged" };
            AssemblyManager.AssemblyFilterFunc = name => name.Name != null && !filters.Any(m => name.Name.StartsWith(m));

            // 添加 OSharp 框架服务
            services.AddOSharp()
                .AddPack<Log4NetPack>()
                .AddPack<AutoMapperPack>()
                .AddPack<SqliteEntityFrameworkCorePack>()
                .AddPack<SqliteDefaultDbContextMigrationPack>();

            IServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }
    }
}
