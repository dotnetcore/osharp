// -----------------------------------------------------------------------
//  <copyright file="IdentityModuleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-08 18:42</last-date>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core;
using OSharp.Core.Modules;


namespace OSharp.Identity
{
    /// <summary>
    /// 身份论证模块基类
    /// </summary>
    public abstract class IdentityModuleBase<TUserStore, TRoleStore, TUser, TRole, TUserKey, TRoleKey> : OSharpModule
        where TUserStore : class, IUserStore<TUser>
        where TRoleStore : class, IRoleStore<TRole>
        where TUser : UserBase<TUserKey>
        where TRole : RoleBase<TRoleKey>
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserStore<TUser>, TUserStore>();
            services.AddScoped<IRoleStore<TRole>, TRoleStore>();

            Action<IdentityOptions> setupAction = SetupAction();
            IdentityBuilder builder = services.AddIdentity<TUser, TRole>(setupAction);
            OnIdentityBuild(builder);

            return services;
        }

        /// <summary>
        /// 重写以实现<see cref="IdentityOptions"/>的配置
        /// </summary>
        /// <returns></returns>
        protected virtual Action<IdentityOptions> SetupAction()
        {
            return null;
        }

        /// <summary>
        /// 重写以实现 AddIdentity 之后的构建逻辑
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual IdentityBuilder OnIdentityBuild(IdentityBuilder builder)
        {
            return builder;
        }
    }
}