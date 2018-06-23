// -----------------------------------------------------------------------
//  <copyright file="MvcFunctionPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Core.Packs;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// MVC功能点模块
    /// </summary>
    public class MvcFunctionPack : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IFunctionHandler, MvcFunctionHandler>();
            services.AddSingleton<IModuleInfoPicker, MvcModuleInfoPicker>();

            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public override void UseModule(IServiceProvider provider)
        {
            IFunctionHandler functionHandler = provider.GetServices<IFunctionHandler>().FirstOrDefault(m => m.GetType() == typeof(MvcFunctionHandler));
            if (functionHandler == null)
            {
                return;
            }
            functionHandler.Initialize();

            IsEnabled = true;
        }
    }
}