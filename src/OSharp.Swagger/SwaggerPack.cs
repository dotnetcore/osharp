// -----------------------------------------------------------------------
//  <copyright file="SwaggerPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-14 23:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// SwaggerApi模块
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    [Description("SwaggerApi模块 ")]
    public class SwaggerPack : AspOsharpPack
    {
        private string _title, _url;
        private int _version;

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
            _url = configuration["OSharp:Swagger:Url"];
            if (_url.IsNullOrEmpty())
            {
                throw new OsharpException("配置文件中Swagger节点的Url不能为空");
            }

            _title = configuration["OSharp:Swagger:Title"];
            _version = configuration["OSharp:Swagger:Version"].CastTo(1);
            bool enabled = configuration["OSharp:Swagger:Enabled"].CastTo(false);

            if (enabled)
            {
                services.AddMvcCore().AddApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc($"v{_version}", new Info() { Title = _title, Version = $"v{_version}" });
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
            }
            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            IConfiguration configuration = app.ApplicationServices.GetService<IConfiguration>();
            bool enabled = configuration["OSharp:Swagger:Enabled"].CastTo(false);
            bool miniProfilerEnabled = configuration["OSharp:Swagger:MiniProfiler"].CastTo(false);
            if (enabled)
            {
                app.UseSwagger().UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(_url, $"{_title} V{_version}");
                    if (miniProfilerEnabled)
                    {
                        options.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("OSharp.Swagger.index.html"); 
                    }
                });
                IsEnabled = true;
            }
        }
    }
}