using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Demo.Identity.Entities;
using OSharp.Identity;


namespace OSharp.Demo.Identity
{
    /// <summary>
    /// 身份认证模块
    /// </summary>
    public class IdentityModule : IdentityModuleBase<UserStore, RoleStore, User, Role, int, int>
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IIdentityContract, IdentityService>();

            return base.AddServices(services);
        }

        /// <summary>
        /// 重写以实现 AddIdentity 之后的构建逻辑
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IdentityBuilder OnIdentityBuild(IdentityBuilder builder)
        {
            return builder.AddDefaultTokenProviders();
        }
    }
}
