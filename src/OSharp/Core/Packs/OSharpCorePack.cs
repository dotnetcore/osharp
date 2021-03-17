// -----------------------------------------------------------------------
//  <copyright file="OsharpCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:19</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using OSharp.Caching;
using OSharp.Core.Options;
using OSharp.Core.Systems;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Http;
using OSharp.Logging;
using OSharp.Net;
using OSharp.Threading;


namespace OSharp.Core.Packs
{
    /// <summary>
    /// OSharp核心模块
    /// </summary>
    [Description("OSharp核心模块")]
    public class OsharpCorePack : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Core;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<IConfigureOptions<OsharpOptions>, OsharpOptionsSetup>();
            services.TryAddSingleton<ICancellationTokenProvider, NoneCancellationTokenProvider>();
            services.TryAddSingleton<IEmailSender, DefaultEmailSender>();
            services.TryAddSingleton<StartupLogger>();

            services.TryAddSingleton<ICacheService, CacheService>();
            services.TryAddScoped<IFilterService, FilterService>();
            services.TryAddScoped<IKeyValueStore, KeyValueStore>();

            services.TryAddTransient<IClientHttpCrypto, ClientHttpCrypto>();
            services.AddTransient<ClientHttpCryptoHandler>();

            services.AddDistributedMemoryCache();

            return services;
        }
    }
}