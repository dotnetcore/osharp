// -----------------------------------------------------------------------
//  <copyright file="SwaggerPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-04-15 16:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.Exceptions;
using OSharp.Extensions;

using Swashbuckle.AspNetCore.Swagger;


namespace OSharp.Swagger
{
    /// <summary>
    /// Swagger模块基类
    /// </summary>
    public abstract class SwaggerPackBase : AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            bool enabled = configuration["OSharp:Swagger:Enabled"].CastTo(false);
            if (!enabled)
            {
                return services;
            }

            string url = configuration["OSharp:Swagger:Url"];
            if (string.IsNullOrEmpty(url))
            {
                throw new OsharpException("配置文件中Swagger节点的Url不能为空");
            }

            string title = configuration["OSharp:Swagger:Title"];
            int version = configuration["OSharp:Swagger:Version"].CastTo(1);

            services.AddMvcCore().AddApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"v{version}", new Info() { Title = title, Version = $"v{version}" });
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file);
                });
                //权限Token
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "请输入带有Bearer的Token，形如 “Bearer {Token}” ",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", Enumerable.Empty<string>() }
                });
            });

            return services;
        }

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            IConfiguration configuration = app.ApplicationServices.GetService<IConfiguration>();
            bool enabled = configuration["OSharp:Swagger:Enabled"].CastTo(false);
            if (!enabled)
            {
                return;
            }

            app.UseSwagger().UseSwaggerUI(options =>
            {
                string url = configuration["OSharp:Swagger:Url"];
                string title = configuration["OSharp:Swagger:Title"];
                int version = configuration["OSharp:Swagger:Version"].CastTo(1);
                options.SwaggerEndpoint(url, $"{title} V{version}");
                bool miniProfilerEnabled = configuration["OSharp:Swagger:MiniProfiler"].CastTo(false);
                if (miniProfilerEnabled)
                {
                    options.IndexStream = () => GetType().Assembly.GetManifestResourceStream("OSharp.Swagger.index.html");
                }
            });
            IsEnabled = true;
        }
    }
}