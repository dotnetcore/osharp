// -----------------------------------------------------------------------
//  <copyright file="IdentityPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:27</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.EventBuses;
using OSharp.Identity.Entities;
using OSharp.Identity.Events;


namespace OSharp.Identity
{
    /// <summary>
    /// 身份标识模块基类
    /// </summary>
    [Description("身份标识模块")]
    [DependsOnPacks(typeof(EventBusPack))]
    public abstract class IdentityPackBase<TUserStore, TRoleStore, TUser, TUserKey, TUserClaim, TUserClaimKey, TRole, TRoleKey> : OsharpPack
        where TUserStore : class, IUserStore<TUser>
        where TRoleStore : class, IRoleStore<TRole>
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
        where TUserClaim : UserClaimBase<TUserClaimKey, TUserKey>
        where TUserClaimKey : IEquatable<TUserClaimKey>
        where TRole : RoleBase<TRoleKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 0;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserStore<TUser>, TUserStore>();
            services.AddScoped<IRoleStore<TRole>, TRoleStore>();
            services.AddEventHandler<OnlineUserCacheRemoveEventHandler>();

            //在线用户缓存
            services.TryAddScoped<IOnlineUserProvider, OnlineUserProvider<TUser, TUserKey, TUserClaim, TUserClaimKey, TRole, TRoleKey>>();

            //替换 IPrincipal，设置用户主键类型，用以在Repository进行审计时注入正确用户主键类型
            services.Replace(new ServiceDescriptor(typeof(IPrincipal),
                provider =>
                {
                    IHttpContextAccessor accessor = provider.GetService<IHttpContextAccessor>();
                    ClaimsPrincipal principal = accessor?.HttpContext?.User;
                    if (principal != null && principal.Identity is ClaimsIdentity identity)
                    {
                        PropertyInfo property = typeof(TUser).GetProperty("Id");
                        if (property != null && !identity.HasClaim(m => m.Type == OsharpConstants.UserIdTypeName))
                        {
                            identity.AddClaim(new Claim(OsharpConstants.UserIdTypeName, property.PropertyType.FullName));
                        }
                    }

                    return principal;
                },
                ServiceLifetime.Transient));

            Action<IdentityOptions> identityOptionsAction = IdentityOptionsAction();
            IdentityBuilder builder = services.AddIdentityCore<TUser>(identityOptionsAction)
                .AddRoles<TRole>().AddSignInManager();

            services.Replace(new ServiceDescriptor(typeof(IdentityErrorDescriber), typeof(IdentityErrorDescriberZhHans), ServiceLifetime.Scoped));

            OnIdentityBuild(builder);

            return services;
        }

        /// <summary>
        /// 重写以实现<see cref="IdentityOptions"/>的配置
        /// </summary>
        /// <returns></returns>
        protected virtual Action<IdentityOptions> IdentityOptionsAction()
        {
            return options =>
            {
                //登录
                options.SignIn.RequireConfirmedEmail = false;
                //密码
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                //用户
                options.User.RequireUniqueEmail = false;
                //锁定
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            };
        }

        /// <summary>
        /// 重写以实现 AddIdentity 之后的构建逻辑
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual IdentityBuilder OnIdentityBuild(IdentityBuilder builder)
        {
            return builder.AddDefaultTokenProviders();
        }
    }
}