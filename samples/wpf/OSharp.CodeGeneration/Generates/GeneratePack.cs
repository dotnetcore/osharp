// -----------------------------------------------------------------------
//  <copyright file="GeneratePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 21:39</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Packs;


namespace OSharp.CodeGeneration.Generates
{
    /// <summary>
    /// 代码生成模块
    /// </summary>
    public class GeneratePack : OsharpPack
    {
        /// <summary>将模块服务添加到依赖注入服务容器中</summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<ICodeGenerator, RazorCodeGenerator>();
            return base.AddServices(services);
        }
    }
}
