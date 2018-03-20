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
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;

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
