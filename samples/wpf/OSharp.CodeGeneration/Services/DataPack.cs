// -----------------------------------------------------------------------
//  <copyright file="DataPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-14 12:53</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services.Seeds;
using OSharp.Core.Packs;
using OSharp.Entity;


namespace OSharp.CodeGeneration.Services
{
    public class DataPack : OsharpPack
    {
        /// <summary>将模块服务添加到依赖注入服务容器中</summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IDataContract, DataService>();
            services.AddSingleton<ISeedDataInitializer, CodeTemplateSeedDataInitializer>();
            services.AddSingleton<ISeedDataInitializer, CodeProjectSeedDataInitializer>();
            services.AddSingleton<ISeedDataInitializer, CodeModuleSeedDataInitializer>();
            services.AddSingleton<ISeedDataInitializer, CodeEntitySeedDataInitializer>();
            services.AddSingleton<ISeedDataInitializer, CodePropertySeedDataInitializer>();

            return base.AddServices(services);
        }
    }
}
