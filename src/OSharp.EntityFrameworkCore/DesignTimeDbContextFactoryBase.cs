// -----------------------------------------------------------------------
//  <copyright file="DesignTimeDbContextFactoryBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-20 17:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Exceptions;
using OSharp.Json;


namespace OSharp.Entity
{
    /// <summary>
    /// 设计时数据上下文实例工厂基类，用于执行数据迁移
    /// </summary>
    public abstract class DesignTimeDbContextFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 初始化一个<see cref="DesignTimeDbContextFactoryBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DesignTimeDbContextFactoryBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// 创建一个数据上下文实例
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public virtual TDbContext CreateDbContext(string[] args)
        {
            if (ServiceProvider == null)
            {
                ServiceProvider = CreateServiceProvider();
            }

            OsharpOptions options = ServiceProvider.GetOSharpOptions();
            OsharpDbContextOptions contextOptions = options.GetDbContextOptions(typeof(TDbContext));
            if (contextOptions == null)
            {
                throw new OsharpException($"上下文“{typeof(TDbContext)}”的配置信息不存在");
            }

            string connString = contextOptions.ConnectionString;
            if (connString == null)
            {
                return null;
            }

            IEntityManager entityManager = ServiceProvider.GetService<IEntityManager>();
            entityManager.Initialize();

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<TDbContext>();
            if (contextOptions.LazyLoadingProxiesEnabled)
            {
                builder.UseLazyLoadingProxies();
            }
            builder = UseSql(builder, connString);
            return (TDbContext)Activator.CreateInstance(typeof(TDbContext), builder.Options, entityManager, null);
        }

        /// <summary>
        /// 创建ServiceProvider
        /// </summary>
        /// <returns></returns>
        protected virtual IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", true, true);
            IConfiguration configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddOSharp().AddPacks();
            IServiceProvider provider = services.BuildServiceProvider();
            return provider;
        }

        /// <summary>
        /// 重写以实现数据上下文选项构建器加载数据库驱动程序
        /// </summary>
        /// <param name="builder">数据上下文选项构建器</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <returns>数据上下文选项构建器</returns>
        public abstract DbContextOptionsBuilder UseSql(DbContextOptionsBuilder builder, string connString);
    }
}