// -----------------------------------------------------------------------
//  <copyright file="SwaggerPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-24 22:29</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.Data;

using Swashbuckle.AspNetCore.Swagger;


namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// Swagger Api模块 
    /// </summary>
    [Description("Swagger Api模块 ")]
    public class SwaggerPack : AspOsharpPack
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
            if (Singleton<IHostingEnvironment>.Instance.IsDevelopment())
            {
                services.AddMvcCore().AddApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info() { Title = "OSharp API", Version = "v1" });
                    Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                    {
                        options.IncludeXmlComments(file);
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
            IHostingEnvironment environment = app.ApplicationServices.GetService<IHostingEnvironment>();
            if (environment.IsDevelopment())
            {
                app.UseSwagger().UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "OSharp API V1");
                });
                IsEnabled = true;
            }
        }
    }
}