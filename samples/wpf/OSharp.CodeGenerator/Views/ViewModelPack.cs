// -----------------------------------------------------------------------
//  <copyright file="ViewModelPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-05 15:38</last-date>
// -----------------------------------------------------------------------

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using OSharp.AutoMapper;
using OSharp.CodeGenerator.Data;
using OSharp.Core.Packs;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;


namespace OSharp.CodeGenerator.Views
{
    public class ViewModelPack : OsharpPack
    {
        /// <summary>将模块服务添加到依赖注入服务容器中</summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.AddValidators(assembly);
            services.AddViewModels(assembly);
            services.AddViews(assembly);

            services.AddSingleton<IMapTuple, AutoMapperConfiguration>();

            return services;
        }
    }
}
