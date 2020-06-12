// -----------------------------------------------------------------------
//  <copyright file="AuthenticationPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-02 21:21</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;
using OSharp.Hosting.Identity.Events;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Authentication;
using OSharp.Core.Packs;


namespace OSharp.Hosting.Identity
{
    /// <summary>
    /// 身份认证模块
    /// </summary>
    [DependsOnPacks(typeof(IdentityPack))]
    public class AuthenticationPack : AuthenticationPackBase<User, int>
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddEventHandler<Logout_RemoveRefreshTokenEventHandler>();
            
            return base.AddServices(services);
        }
    }
}