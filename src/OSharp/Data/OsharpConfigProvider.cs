// -----------------------------------------------------------------------
//  <copyright file="OsharpConfigProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-23 13:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Exceptions;


namespace OSharp.Data
{
    /// <summary>
    /// 默认的OSharp配置信息提供者
    /// </summary>
    public class OsharpConfigProvider : IOsharpConfigProvider, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="OsharpConfigProvider"/>类型的新实例
        /// </summary>
        public OsharpConfigProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 创建OSharp配置信息
        /// </summary>
        /// <returns></returns>
        public OsharpConfig Create()
        {
            OsharpConfig config = new OsharpConfig
            {
                DbContexts = DbContextConfigs(_serviceProvider)
            };

            return config;
        }

        /// <summary>
        /// 获取 数据上下文配置信息
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns></returns>
        protected virtual IDictionary<string, OsharpDbContextConfig> DbContextConfigs(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            IConfigurationSection section = configuration.GetSection("OSharp");
            IDictionary<string, OsharpDbContextConfig> dbContextConfigsDict = new Dictionary<string, OsharpDbContextConfig>();
            if (section == null)
            {
                //使用默认数据上下文配置
                string connectionString = configuration["ConnectionStrings:DefaultDbContext"];
                if (connectionString == null)
                {
                    throw new OsharpException(
                        $"无法找到键名为“DefaultDbContext”的数据库连接字符串信息，请在 appsettings.json 添加 “ConnectionStrings:DefaultDbContext” 节点的数据库连接字符串配置信息");
                }
                OsharpDbContextConfig defaultDbContextConfig = new OsharpDbContextConfig()
                {
                    DbContextTypeName = "OSharp.Entity.DefaultDbContext,OSharp.EntityFrameworkCore",
                    ConnectionString = connectionString,
                    DatabaseType = DatabaseType.SqlServer
                };
                dbContextConfigsDict.Add("DefaultDbContext", defaultDbContextConfig);
                return dbContextConfigsDict;
            }
            section.GetSection("DbContexts").Bind(dbContextConfigsDict);
            return dbContextConfigsDict;
        }
    }
}