// -----------------------------------------------------------------------
//  <copyright file="OSharpOptionsSetup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-03 12:32</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Exceptions;


namespace OSharp.Options
{
    /// <summary>
    /// OSharp配置选项创建器
    /// </summary>
    public class OSharpOptionsSetup : IConfigureOptions<OSharpOptions>, ISingletonDependency
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化一个<see cref="OSharpOptionsSetup"/>类型的新实例
        /// </summary>
        public OSharpOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>Invoked to configure a TOptions instance.</summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(OSharpOptions options)
        {
            SetDbContextOptionses(options);
        }

        private void SetDbContextOptionses(OSharpOptions options)
        {
            IConfigurationSection section = _configuration.GetSection("OSharp:DbContexts");
            if (section?.Value == null)
            {
                string connectionString = _configuration["ConnectionStrings:DefaultDbContext"];
                if (connectionString == null)
                {
                    return;
                }
                OSharpDbContextOptions dbContextOptions = new OSharpDbContextOptions()
                {
                    DbContextTypeName = "OSharp.Entity.DefaultDbContext,OSharp.EntityFrameworkCore",
                    ConnectionString = connectionString,
                    DatabaseType = DatabaseType.SqlServer
                };
                options.DbContextOptionses.Add("DefaultDbContext", dbContextOptions);
                return;
            }
            IDictionary<string, OSharpDbContextOptions> dict = new Dictionary<string, OSharpDbContextOptions>();
            section.Bind(dict);
            var repeated = dict.Values.GroupBy(m => m.DbContextType).FirstOrDefault(m => m.Count() > 1);
            if (repeated != null)
            {
                throw new OsharpException($"数据上下文配置中存在多个配置节点指向同一个上下文类型：{repeated.First().DbContextTypeName}");
            }

            foreach (KeyValuePair<string, OSharpDbContextOptions> pair in dict)
            {
                options.DbContextOptionses.Add(pair.Key, pair.Value);
            }
        }
    }
}