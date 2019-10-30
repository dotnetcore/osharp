using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.EventBuses;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace OSharp.Identity.IdentityServer4
{
    /// <summary>
    /// 基于IdentityServer4身份认证模块基类
    /// </summary>
    [DependsOnPacks(typeof(EventBusPack), typeof(AspNetCorePack))]
    public abstract class IdentityServer4PackBase<TUserStore, TRoleStore, TUser, TRole, TUserKey, TRoleKey> : AspOsharpPack
        where TUserStore : class, IUserStore<TUser>
        where TRoleStore : class, IRoleStore<TRole>
        where TUser : UserBase<TUserKey>
        where TRole : RoleBase<TRoleKey>
        where TUserKey : IEquatable<TUserKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            //在线用户缓存
            services.TryAddScoped<IOnlineUserProvider, OnlineUserProvider<TUser, TUserKey, TRole, TRoleKey>>();

            // 替换 IPrincipal ，设置用户主键类型，用以在Repository进行审计时注入正确用户主键类型
            services.Replace(new ServiceDescriptor(typeof(IPrincipal),
                provider =>
                {
                    IHttpContextAccessor accessor = provider.GetService<IHttpContextAccessor>();
                    ClaimsPrincipal principal = accessor?.HttpContext?.User;
                    if (principal != null && principal.Identity is ClaimsIdentity identity)
                    {
                        PropertyInfo property = typeof(TUser).GetProperty("Id");
                        if (property != null)
                        {
                            identity.AddClaim(new Claim("userIdTypeName", property.PropertyType.FullName));
                        }
                    }

                    return principal;
                },
                ServiceLifetime.Transient));

            Action<IdentityOptions> identityOptionsAction = IdentityOptionsAction();
            IdentityBuilder builder = services.AddIdentity<TUser, TRole>(identityOptionsAction)
                .AddUserStore<TUserStore>()
                .AddRoleStore<TRoleStore>()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<TUser, TRole>>();

            services.Replace(new ServiceDescriptor(typeof(IdentityErrorDescriber), typeof(IdentityErrorDescriberZhHans), ServiceLifetime.Scoped));

            AddIdentityBuild(builder);

            //IdentityServer4
            Action<IdentityServerOptions> identityServerOptionsAction = IdentityServerOptionsAction();
            var identityBuilder = services.AddIdentityServer(identityServerOptionsAction);

            AddIdentityServerBuild(identityBuilder, services);

            Action<CookieAuthenticationOptions> cookieOptionsAction = CookieOptionsAction();
            if (cookieOptionsAction != null)
            {
                services.ConfigureApplicationCookie(cookieOptionsAction);
            }

            AddAuthentication(services);

            return services;
        }

        /// <summary>
        /// 重写以实现<see cref="IdentityOptions"/>的配置
        /// </summary>
        /// <returns></returns>
        protected virtual Action<IdentityOptions> IdentityOptionsAction()
        {
            return null;
        }

        /// <summary>
        /// 重写以实现<see cref="IdentityServerOptions"/>的配置
        /// </summary>
        /// <example>
        /// return options =>
        /// {
        ///    options.Events.RaiseErrorEvents = true;
        ///    options.Events.RaiseInformationEvents = true;
        ///    options.Events.RaiseFailureEvents = true;
        ///    options.Events.RaiseSuccessEvents = true;
        /// };
        /// </example>
        /// <returns></returns>
        protected virtual Action<IdentityServerOptions> IdentityServerOptionsAction()
        {
            return null;
        }

        /// <summary>
        /// 重写以实现<see cref="CookieAuthenticationOptions"/>的配置
        /// </summary>
        /// <returns></returns>
        protected virtual Action<CookieAuthenticationOptions> CookieOptionsAction()
        {
            return null;
        }

        /// <summary>
        /// 添加Authentication服务
        /// </summary>
        /// <param name="services">服务集合</param>
        protected virtual void AddAuthentication(IServiceCollection services)
        { }

        /// <summary>
        /// 重写以实现 AddIdentity 之后的构建逻辑
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual IdentityBuilder AddIdentityBuild(IdentityBuilder builder)
        {
            return builder;
        }

        /// <summary>
        /// 重写以实现 AddIdentityServer 之后的构建逻辑
        /// </summary>
        /// <param name="builder"></param>
        /// <example>
        /// .AddInMemoryIdentityResources(Config.GetIdentityResources())
        /// .AddInMemoryApiResources(Config.GetApis())
        /// .AddInMemoryClients(Config.GetClients());
        /// </example>
        protected virtual IIdentityServerBuilder AddIdentityServerBuild(IIdentityServerBuilder builder, IServiceCollection services)
        {
            return builder;
        }
    }
}
