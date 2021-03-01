// -----------------------------------------------------------------------
//  <copyright file="SystemsPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-01 12:25</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using OSharp.AutoMapper;
using OSharp.Core.Packs;
using OSharp.Hosting.Systems.Dtos;


namespace OSharp.Hosting.Systems
{
    [Description("系统模块")]
    public class SystemsPack : OsharpPack
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
            services.AddScoped<ISystemsContract, SystemsService>();
            services.AddSingleton<IAutoMapperConfiguration, AutoMapperConfiguration>();

            return services;
        }
    }
}