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
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Exceptions;
using OSharp.Json;


namespace OSharp.Entity
{
    /// <summary>
    /// 设计时数据上下文实例工厂基类，用于执行数据迁移
    /// </summary>
    public abstract class DesignTimeDbContextFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
        where TDbContext : DbContext, IDbContext
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="DesignTimeDbContextFactoryBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DesignTimeDbContextFactoryBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _logger = serviceProvider?.GetLogger(GetType());
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
                ServiceProvider = CreateDesignTimeServiceProvider();
            }

            IEntityManager entityManager = ServiceProvider.GetService<IEntityManager>();
            entityManager.Initialize();

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<TDbContext>();
            builder = ServiceProvider.BuildDbContextOptionsBuilder<TDbContext>(builder);

            TDbContext context = (TDbContext)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(TDbContext), builder.Options);
            return context;
        }

        /// <summary>
        /// 创建设计时使用的ServiceProvider，主要用于执行 Add-Migration 功能
        /// </summary>
        /// <returns></returns>
        protected abstract IServiceProvider CreateDesignTimeServiceProvider();
    }
}