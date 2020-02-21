// -----------------------------------------------------------------------
//  <copyright file="IdentityServer4Pack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 0:56</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using IdentityServer4.Stores;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OSharp.AspNetCore;
using OSharp.AutoMapper;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.IdentityServer4.Mappers;
using OSharp.IdentityServer4.Options;
using OSharp.IdentityServer4.Services;
using OSharp.IdentityServer4.Stores;


namespace OSharp.IdentityServer4
{
    /// <summary>
    /// IdentityServer4模块
    /// </summary>
    [Description("IdentityServer4模块")]
    public class IdentityServer4Pack : AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
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
            IdentityServerOptions options = configuration.GetInstance("OSharp:IdentityServer", new IdentityServerOptions());
            Singleton<IdentityServerOptions>.Instance = options;

            services.AddSingleton<IAutoMapperConfiguration, ApiResourceMapperConfiguration>();
            services.AddSingleton<IAutoMapperConfiguration, ClientMapperConfiguration>();
            services.AddSingleton<IAutoMapperConfiguration, IdentityResourceMapperConfiguration>();
            services.AddSingleton<IAutoMapperConfiguration, PersistedGrantMapperConfiguration>();

            services.AddScoped<IClientStore, ClientStore>();
            services.AddScoped<IDeviceFlowStore, DeviceFlowStore>();
            services.AddScoped<IPersistedGrantStore, PersistedGrantStore>();
            services.AddScoped<IResourceStore, ResourceStore>();

            services.AddScoped<ITokenCleanupService, ITokenCleanupService>();
            services.AddSingleton<IHostedService, TokenCleanupHostedService>();

            return services;
        }
    }
}