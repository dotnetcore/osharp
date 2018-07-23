// -----------------------------------------------------------------------
//  <copyright file="OSharpOptionsSetup.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-03 12:32</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using OSharp.Entity;
using OSharp.Exceptions;


namespace OSharp.Core.Options
{
    /// <summary>
    /// OSharp配置选项创建器
    /// </summary>
    public class OSharpOptionsSetup : IConfigureOptions<OSharpOptions>
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

            //MailSender
            IConfigurationSection section = _configuration.GetSection("OSharp:MailSender");
            MailSenderOptions sender = section.Get<MailSenderOptions>();
            if (sender != null)
            {
                if (sender.Password == null)
                {
                    sender.Password = _configuration["OSharp:MailSender:Password"];
                }
                options.MailSender = sender;
            }

            //JwtOptions
            section = _configuration.GetSection("OSharp:Jwt");
            JwtOptions jwt = section.Get<JwtOptions>();
            if (jwt != null)
            {
                if (jwt.Secret == null)
                {
                    jwt.Secret = _configuration["OSharp:Jwt:Secret"];
                }
                options.Jwt = jwt;
            }
        }

        /// <summary>
        /// 初始化上下文配置信息，首先以OSharp配置节点中的为准，
        /// 不存在OSharp节点，才使用ConnectionStrings的数据连接串来构造SqlServer的配置，
        /// 保证同一上下文类型只有一个配置节点
        /// </summary>
        /// <param name="options"></param>
        private void SetDbContextOptionses(OSharpOptions options)
        { 
            IConfigurationSection section = _configuration.GetSection("OSharp:DbContexts");
            IDictionary<string, OSharpDbContextOptions> dict = section.Get<Dictionary<string, OSharpDbContextOptions>>();
            if (dict == null || dict.Count == 0)
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