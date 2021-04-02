// -----------------------------------------------------------------------
//  <copyright file="MultiTenancyEntityFrameworkCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-03 0:52</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.MultiTenancy.EntityFrameworkCore
{
    /// <summary>
    /// 多租户EFCore模块
    /// </summary>
    [Description("多租户EFCore模块")]
    public class MultiTenancyEntityFrameworkCorePack : MultiTenancyPackBase
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            return base.AddServices(services);
        }
    }
}