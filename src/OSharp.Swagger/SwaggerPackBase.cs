// -----------------------------------------------------------------------
//  <copyright file="SwaggerPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-04-15 16:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.OpenApi.Models;

using OSharp.AspNetCore;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Exceptions;
using OSharp.Reflection;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace OSharp.Swagger
{
    /// <summary>
    /// Swagger模块基类
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    public abstract class SwaggerPackBase : AspOsharpPack
    {
        private OsharpOptions _osharpOptions;

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
            _osharpOptions = configuration.GetOsharpOptions();
            if (_osharpOptions?.Swagger?.Enabled != true)
            {
                return services;
            }

            services.AddMvcCore().AddApiExplorer();
            services.AddSwaggerGen(options =>
            {
                if (_osharpOptions?.Swagger?.Endpoints?.Count > 0)
                {
                    foreach (SwaggerEndpoint endpoint in _osharpOptions.Swagger.Endpoints)
                    {
                        options.SwaggerDoc($"{endpoint.Version}",
                            new OpenApiInfo() { Title = endpoint.Title, Version = endpoint.Version });
                    }

                    options.DocInclusionPredicate((version, desc) =>
                    {
                        if (!desc.TryGetMethodInfo(out MethodInfo method))
                        {
                            return false;
                        }

                        string[] versions = method.DeclaringType.GetAttributes<ApiExplorerSettingsAttribute>().Select(m => m.GroupName).ToArray();
                        if (version.ToLower()== "v1" && versions.Length == 0)
                        {
                            return true;
                        }

                        return versions.Any(m => m.ToString() == version);
                    });
                }

                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file);
                });
                //权限Token
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "请输入带有Bearer的Token，形如 “Bearer {Token}” ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
                });
                options.DocumentFilter<HiddenApiFilter>();
            });

            return services;
        }

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            if (_osharpOptions?.Swagger?.Enabled != true)
            {
                return;
            }

            SwaggerOptions swagger = _osharpOptions.Swagger;
            app.UseSwagger().UseSwaggerUI(options =>
            {
                if (swagger.IsHideSchemas)
                {
                    options.DefaultModelsExpandDepth(-1); 
                }
                if (swagger.Endpoints?.Count > 0)
                {
                    foreach (SwaggerEndpoint endpoint in swagger.Endpoints)
                    {
                        options.SwaggerEndpoint(endpoint.Url, endpoint.Title);
                    }
                }

                options.RoutePrefix = swagger.RoutePrefix;

                if (swagger.MiniProfiler)
                {
                    options.IndexStream = () => GetType().Assembly.GetManifestResourceStream("OSharp.Swagger.index.html");
                }
            });

            IsEnabled = true;
        }
    }
}