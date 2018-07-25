// -----------------------------------------------------------------------
//  <copyright file="SqlServerMigrationModuleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-20 16:57</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core;
using OSharp.Core.Options;
using OSharp.Core.Packs;


namespace OSharp.Entity.SqlServer
{
    /// <summary>
    /// SqlServer数据迁移模块基类
    /// </summary>
    public abstract class SqlServerMigrationModuleBase<TDbContext> : OsharpPack
        where TDbContext : DbContext
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            IServiceProvider provider = app.ApplicationServices;
            using (IServiceScope scope = provider.CreateScope())
            {
                ILogger logger = provider.GetService<ILoggerFactory>().CreateLogger(GetType());
                TDbContext context = CreateDbContext(scope.ServiceProvider);
                if (context != null)
                {
                    OSharpOptions options = scope.ServiceProvider.GetOSharpOptions();
                    OSharpDbContextOptions contextOptions = options.GetDbContextOptions(context.GetType());
                    if (contextOptions == null)
                    {
                        logger.LogWarning($"上下文类型“{context.GetType()}”的数据库上下文配置不存在");
                        return;
                    }
                    if (contextOptions.DatabaseType != DatabaseType.SqlServer)
                    {
                        logger.LogWarning($"上下文类型“{contextOptions.DatabaseType}”不是 {nameof(DatabaseType.SqlServer)} 类型");
                        return;
                    }
                    if (contextOptions.AutoMigrationEnabled)
                    {
                        context.CheckAndMigration();
                    }
                }
            }

            IsEnabled = true;
        }

        /// <summary>
        /// 重写实现获取数据上下文实例
        /// </summary>
        /// <param name="scopedProvider">服务提供者</param>
        /// <returns></returns>
        protected abstract TDbContext CreateDbContext(IServiceProvider scopedProvider);
    }
}