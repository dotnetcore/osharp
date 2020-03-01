// -----------------------------------------------------------------------
//  <copyright file="IdentityServerPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-29 20:37</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using IdentityServer4.Stores;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OSharp.AspNetCore;
using OSharp.AutoMapper;
using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.IdentityServer.Options;
using OSharp.IdentityServer.Services;
using OSharp.IdentityServer.Storage;
using OSharp.IdentityServer.Storage.Mappers;


namespace OSharp.IdentityServer
{
    /// <summary>
    /// IdentityServer4模块
    /// </summary>
    [Description("IdentityServer4模块")]
    public abstract class IdentityServerPackBase<TUser> : AspOsharpPack
        where TUser : class
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 3;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            IdentityServerOptions options = configuration.GetIdentityServerOptions();
            Singleton<IdentityServerOptions>.Instance = options;

            services.AddSingleton<IAutoMapperConfiguration, ApiResourceMapperConfiguration>();
            services.AddSingleton<IAutoMapperConfiguration, ClientMapperConfiguration>();
            services.AddSingleton<IAutoMapperConfiguration, IdentityResourceMapperConfiguration>();
            services.AddSingleton<IAutoMapperConfiguration, PersistedGrantMapperConfiguration>();

            services.AddScoped<IClientStore, ClientStore>();
            services.AddScoped<IDeviceFlowStore, DeviceFlowStore>();
            services.AddScoped<IPersistedGrantStore, PersistedGrantStore>();
            services.AddScoped<IResourceStore, ResourceStore>();

            services.AddScoped<ITokenCleanupService, TokenCleanupService>();
            services.AddSingleton<IHostedService, TokenCleanupHostedService>();

            return services;
        }

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            //app.UseIdentityServer();
            IsEnabled = true;
        }
    }
}