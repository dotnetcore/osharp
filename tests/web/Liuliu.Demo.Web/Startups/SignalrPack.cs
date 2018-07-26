// -----------------------------------------------------------------------
//  <copyright file="SignalrPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-26 12:15</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Packs;


namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// SignalR模块
    /// </summary>
    public class SignalRPack : OsharpPack
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseSignalR(options =>
            {
                //options.MapHub<>();
            });
            IsEnabled = true;
        }
    }
}