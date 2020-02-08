// -----------------------------------------------------------------------
//  <copyright file="AspOsharpPackManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-09 22:34</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Core.Packs;
using OSharp.Exceptions;
using System;
using System.ComponentModel;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// AspNetCore 模块管理器
    /// </summary>
    public class AspOsharpPackManager : OsharpPackManager, IAspUsePack
    {
        /// <summary>
        /// 应用模块服务，仅在非AspNetCore环境下调用，AspNetCore环境请执行<see cref="UsePack(IApplicationBuilder)"/>功能
        /// </summary>
        /// <param name="provider">服务提供者</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void UsePack(IServiceProvider provider)
        {
            IWebHostEnvironment environment = provider.GetService<IWebHostEnvironment>();
            if (environment != null)
            {
                throw new OsharpException("当前处于AspNetCore环境，请使用UsePack(IApplicationBuilder)进行初始化");
            }

            base.UsePack(provider);
        }

        /// <summary>
        /// 应用模块服务，仅在AspNetCore环境下调用，非AspNetCore环境请执行<see cref="UsePack(IServiceProvider)"/>功能
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public void UsePack(IApplicationBuilder app)
        {
            ILogger logger = app.ApplicationServices.GetLogger<AspOsharpPackManager>();
            logger.LogInformation("Osharp框架初始化开始");
            DateTime dtStart = DateTime.Now;

            foreach (OsharpPack pack in LoadedPacks)
            {
                if (pack is AspOsharpPack aspPack)
                {
                    aspPack.UsePack(app);
                }
                else
                {
                    pack.UsePack(app.ApplicationServices);
                }
            }

            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            logger.LogInformation($"Osharp框架初始化完成，耗时：{ts:g}");
        }
    }
}