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

using OSharp.Collections;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Core.Options
{
    /// <summary>
    /// OSharp配置选项创建器
    /// </summary>
    public class OsharpOptionsSetup : IConfigureOptions<OsharpOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化一个<see cref="OsharpOptionsSetup"/>类型的新实例
        /// </summary>
        public OsharpOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>Invoked to configure a TOptions instance.</summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(OsharpOptions options)
        {
            SetDbContextOptions(options);

            IConfigurationSection section;
            //OAuth2
            section = _configuration.GetSection("OSharp:OAuth2");
            IDictionary<string, OAuth2Options> dict = section.Get<Dictionary<string, OAuth2Options>>();
            if (dict != null)
            {
                foreach (KeyValuePair<string, OAuth2Options> item in dict)
                {
                    options.OAuth2S.Add(item.Key, item.Value);
                }
            }

            //MailSender
            section = _configuration.GetSection("OSharp:MailSender");
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

            //CookieOptions
            section = _configuration.GetSection("OSharp:Cookie");
            CookieOptions cookie = section.Get<CookieOptions>();
            if (cookie != null)
            {
                options.Cookie = cookie;
            }

            //CorsOptions
            section = _configuration.GetSection("OSharp:Cors");
            CorsOptions cors = section.Get<CorsOptions>();
            if (cors != null)
            {
                options.Cors = cors;
            }

            // RedisOptions
            section = _configuration.GetSection("OSharp:Redis");
            RedisOptions redis = section.Get<RedisOptions>();
            if (redis != null)
            {
                if (redis.Configuration.IsMissing())
                {
                    throw new OsharpException("配置文件中Redis节点的Configuration不能为空");
                }
                options.Redis = redis;
            }

            // SwaggerOptions
            section = _configuration.GetSection("OSharp:Swagger");
            SwaggerOptions swagger = section.Get<SwaggerOptions>();
            if (swagger != null)
            {
                if (swagger.Endpoints.IsNullOrEmpty())
                {
                    throw new OsharpException("配置文件中Swagger节点的EndPoints不能为空");
                }

                if (swagger.RoutePrefix == null)
                {
                    swagger.RoutePrefix = "swagger";
                }
                options.Swagger = swagger;
            }

            // HttpEncrypt
            section = _configuration.GetSection("OSharp:HttpEncrypt");
            HttpEncryptOptions httpEncrypt = section.Get<HttpEncryptOptions>();
            if (httpEncrypt != null)
            {
                options.HttpEncrypt = httpEncrypt;
            }
        }

        /// <summary>
        /// 初始化上下文配置信息，首先以OSharp配置节点中的为准，
        /// 不存在OSharp节点，才使用ConnectionStrings的数据连接串来构造SqlServer的配置，
        /// 保证同一上下文类型只有一个配置节点
        /// </summary>
        /// <param name="options"></param>
        private void SetDbContextOptions(OsharpOptions options)
        {
            IConfigurationSection section = _configuration.GetSection("OSharp:DbContexts");
            IDictionary<string, OsharpDbContextOptions> dict = section.Get<Dictionary<string, OsharpDbContextOptions>>();
            if (dict == null || dict.Count == 0)
            {
                string connectionString = _configuration["ConnectionStrings:DefaultDbContext"];
                if (connectionString == null)
                {
                    return;
                }
                OsharpDbContextOptions dbContextOptions = new OsharpDbContextOptions()
                {
                    DbContextTypeName = "OSharp.Entity.DefaultDbContext,OSharp.EntityFrameworkCore",
                    ConnectionString = connectionString,
                    DatabaseType = DatabaseType.SqlServer
                };
                options.DbContexts.Add("DefaultDbContext", dbContextOptions);
                return;
            }
            var repeated = dict.Values.GroupBy(m => m.DbContextType).FirstOrDefault(m => m.Count() > 1);
            if (repeated != null)
            {
                throw new OsharpException($"数据上下文配置中存在多个配置节点指向同一个上下文类型：{repeated.First().DbContextTypeName}");
            }

            foreach (KeyValuePair<string, OsharpDbContextOptions> pair in dict)
            {
                options.DbContexts.Add(pair.Key, pair.Value);
            }
        }
    }
}
