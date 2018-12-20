// -----------------------------------------------------------------------
//  <copyright file="HangfirePackCore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-20 13:01</last-date>
// -----------------------------------------------------------------------


using Hangfire;
using Hangfire.MemoryStorage;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.Core.Packs;


namespace OSharp.Hangfire
{
    /// <summary>
    /// Hangfire后台任务模块基类
    /// </summary>
    public abstract class HangfirePackCore : AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 0;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddHangfire(opt => opt.UseMemoryStorage());
            return services;
        }

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard("/api/hangfire");

            IsEnabled = true;
        }
    }
}