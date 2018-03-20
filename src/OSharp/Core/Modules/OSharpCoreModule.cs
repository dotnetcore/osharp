// -----------------------------------------------------------------------
//  <copyright file="CoreModule.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 18:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Core.Options;
using OSharp.Entity;


namespace OSharp.Core.Modules
{
    /// <summary>
    /// OSharp核心模块
    /// </summary>
    public class OSharpCoreModule : OSharpModule
    {
        /// <summary>
        /// 获取 是否内部模块，内部模块将自动加载
        /// </summary>
        public override bool IsAutoLoad => true;

        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override ModuleLevel Level => ModuleLevel.Core;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigureOptions<OSharpOptions>, OSharpOptionsSetup>();
            ServiceLocator.Instance.TrySetServiceCollection(services);

            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public override void UseModule(IServiceProvider provider)
        {
            //应用程序级别的服务定位器
            ServiceLocator.Instance.TrySetApplicationServiceProvider(provider);
            
            IsEnabled = true;
        }
    }
}