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

using OSharp.Entity;
using OSharp.Options;
using OSharp.Reflection;


namespace OSharp.Core
{
    /// <summary>
    /// OSharp核心模块
    /// </summary>
    public class OSharpCoreModule : OSharpModule
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IEntityTypeFinder, EntityTypeFinder>();
            services.AddSingleton<IConfigureOptions<OSharpOptions>, OSharpOptionsSetup>();

            services.AddSingleton<IEntityInfoHandler, EntityInfoHandler>();

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

            //实体信息初始化
            IEntityInfoHandler entityInfoHandler = provider.GetService<IEntityInfoHandler>();
            entityInfoHandler.Initialize();

            //功能信息初始化
            IFunctionHandler[] functionHandlers = provider.GetServices<IFunctionHandler>().ToArray();
            foreach (IFunctionHandler functionHandler in functionHandlers)
            {
                functionHandler.Initialize();
            }
        }
    }
}