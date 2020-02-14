// -----------------------------------------------------------------------
//  <copyright file="MvcPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-29 12:10</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Serialization;

using OSharp.AspNetCore.Mvc.Conventions;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Core.Packs;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// Mvc模块基类
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    public abstract class MvcPackBase : AspOsharpPack
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
            services = AddCors(services);
            services.AddControllersWithViews(options =>
            {
                options.Conventions.Add(new DashedRoutingConvention());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddScoped<UnitOfWorkFilterImpl>();
            services.AddHttpsRedirection(opts => opts.HttpsPort = 443);
            services.AddDistributedMemoryCache();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseRouting();
            UseCors(app);

            IsEnabled = true;
        }

        /// <summary>
        /// 重写以实现添加Cors服务
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        protected virtual IServiceCollection AddCors(IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// 重写以应用Cors
        /// </summary>
        protected virtual IApplicationBuilder UseCors(IApplicationBuilder app)
        {
            return app;
        }
    }
}