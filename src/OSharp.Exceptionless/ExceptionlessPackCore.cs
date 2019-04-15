// -----------------------------------------------------------------------
//  <copyright file="ExceptionlessPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-02-28 1:42</last-date>
// -----------------------------------------------------------------------

using System;

using Exceptionless;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Exceptionless
{
    /// <summary>
    /// Exceptionless分布式异常日志模块基类
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    public abstract class ExceptionlessPackCore : AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            bool enabled = configuration["OSharp:Exceptionless:Enabled"].CastTo(false);
            if (!enabled)
            {
                return services;
            }

            services.AddSingleton<ILoggerProvider, ExceptionlessLoggerProvider>();
            return services;
        }

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            IConfiguration configuration = app.ApplicationServices.GetService<IConfiguration>();
            bool enabled = configuration["OSharp:Exceptionless:Enabled"].CastTo(false);
            if (!enabled)
            {
                return;
            }

            string apiKey = configuration["OSharp:Exceptionless:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new OsharpException("配置文件中Exceptionless节点的ApiKey不能为空");
            }

            ExceptionlessClient.Default.Configuration.ApiKey = apiKey;
            string serverUrl = configuration["OSharp:Exceptionless:ServerUrl"];
            if (!string.IsNullOrEmpty(serverUrl))
            {
                ExceptionlessClient.Default.Configuration.ServerUrl = serverUrl;
            }

            app.UseExceptionless();

            UsePack(app.ApplicationServices);
        }

    }
}