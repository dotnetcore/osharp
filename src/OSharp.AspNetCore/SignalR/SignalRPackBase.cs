// -----------------------------------------------------------------------
//  <copyright file="SignalRPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-04 20:42</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json.Serialization;

using OSharp.Core.Packs;


namespace OSharp.AspNetCore.SignalR
{
    /// <summary>
    /// SignalR模块基类
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    public abstract class SignalRPackBase : AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

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
            services.TryAddSingleton<IUserIdProvider, UserNameUserIdProvider>();
            services.TryAddSingleton<IConnectionUserCache, ConnectionUserCache>();

            Action<HubOptions> hubOptions = GetHubOptionsAction(services);
            ISignalRServerBuilder builder = hubOptions == null
                ? services.AddSignalR()
                : services.AddSignalR(hubOptions);

            Action<ISignalRServerBuilder> buildAction = GetSignalRServerBuildAction(services);
            buildAction?.Invoke(builder);

            return services;
        }

        /// <summary>
        /// 重写以获取HubOptions创建委托
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        protected virtual Action<HubOptions> GetHubOptionsAction(IServiceCollection services)
        {
            return config =>
            {
                IWebHostEnvironment environment = services.GetWebHostEnvironment();
                if (environment.IsDevelopment())
                {
                    config.EnableDetailedErrors = true;
                }
            };
        }

        /// <summary>
        /// 重写以获取SignalR服务器创建委托
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        protected virtual Action<ISignalRServerBuilder> GetSignalRServerBuildAction(IServiceCollection services)
        {
            return builder => builder.AddNewtonsoftJsonProtocol(options =>
                options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver());
        }
    }
}