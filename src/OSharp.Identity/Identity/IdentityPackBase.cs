// -----------------------------------------------------------------------
//  <copyright file="IdentityPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:27</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

using OSharp.AspNetCore;
using OSharp.Authentication.JwtBearer;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.EventBuses;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Identity.Entities;
using OSharp.Identity.JwtBearer;


namespace OSharp.Identity
{
    /// <summary>
    /// 身份论证模块基类
    /// </summary>
    [DependsOnPacks(typeof(EventBusPack), typeof(AspNetCorePack))]
    public abstract class IdentityPackBase<TUserStore, TRoleStore, TUser, TUserKey, TUserClaim, TUserClaimKey, TRole, TRoleKey> : AspOsharpPack
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

        #region Overrides of OsharpPack

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        #endregion

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserStore<TUser>, TUserStore>();
            services.AddScoped<IRoleStore<TRole>, TRoleStore>();

            //在线用户缓存
            services.TryAddScoped<IOnlineUserProvider, OnlineUserProvider<TUser, TUserKey, TUserClaim, TUserClaimKey, TRole, TRoleKey>>();

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
            IdentityBuilder builder = services.AddIdentity<TUser, TRole>(identityOptionsAction);

            services.Replace(new ServiceDescriptor(typeof(IdentityErrorDescriber), typeof(IdentityErrorDescriberZhHans), ServiceLifetime.Scoped));

            OnIdentityBuild(builder);

            Action<CookieAuthenticationOptions> cookieOptionsAction = CookieOptionsAction();
            if (cookieOptionsAction != null)
            {
                services.ConfigureApplicationCookie(cookieOptionsAction);
            }

            AddAuthentication(services);

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            IsEnabled = true;
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
        /// 重写以实现<see cref="CookieAuthenticationOptions"/>的配置
        /// </summary>
        /// <returns></returns>
        protected virtual Action<CookieAuthenticationOptions> CookieOptionsAction()
        {
            return options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "osharp.identity";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.LoginPath = "/#/identity/login";
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

        /// <summary>
        /// 添加Authentication认证服务
        /// </summary>
        /// <param name="services">服务集合</param>
        protected virtual void AddAuthentication(IServiceCollection services)
        {
            AuthenticationBuilder builder = services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            AddJwtBearer(services, builder);
            AddCookie(services, builder);
            AddOAuth2(services, builder);
        }

        /// <summary>
        /// 添加JwtBearer支持
        /// </summary>
        protected virtual AuthenticationBuilder AddJwtBearer(IServiceCollection services, AuthenticationBuilder builder)
        {
            services.TryAddScoped<IJwtBearerService, JwtBearerService<TUser, TUserKey>>();
            services.TryAddScoped<IAccessClaimsProvider, AccessClaimsProvider<TUser, TUserKey>>();

            IConfiguration configuration = services.GetConfiguration();
            builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                jwt =>
                {
                    string secret = configuration["OSharp:Jwt:Secret"];
                    if (secret.IsNullOrEmpty())
                    {
                        throw new OsharpException("配置文件中OSharp配置的Jwt节点的Secret不能为空");
                    }

                    jwt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = configuration["OSharp:Jwt:Issuer"] ?? "osharp identity",
                        ValidAudience = configuration["OSharp:Jwt:Audience"] ?? "osharp client",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        LifetimeValidator = (nbf, exp, token, param) => exp > DateTime.UtcNow
                    };

                    jwt.Events = new OsharpJwtBearerEvents();
                });

            return builder;
        }

        protected virtual AuthenticationBuilder AddCookie(IServiceCollection services, AuthenticationBuilder builder)
        {
            //builder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            //    opts =>
            //    {
            //        opts.Events = new OsharpCookieAuthenticationEvents();
            //    });
            return builder;
        }

        /// <summary>
        /// 添加OAuth2第三方登录配置
        /// </summary>
        protected virtual AuthenticationBuilder AddOAuth2(IServiceCollection services, AuthenticationBuilder builder)
        {
            IConfiguration configuration = services.GetConfiguration();
            IConfigurationSection section = configuration.GetSection("OSharp:OAuth2");
            IDictionary<string, OAuth2Options> dict = section.Get<Dictionary<string, OAuth2Options>>();
            if (dict == null)
            {
                return builder;
            }

            foreach (var (name, options) in dict)
            {
                if (!options.Enabled)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(options.ClientId))
                {
                    throw new OsharpException($"配置文件中OSharp:OAuth2配置的{name}节点的ClientId不能为空");
                }
                if (string.IsNullOrEmpty(options.ClientSecret))
                {
                    throw new OsharpException($"配置文件中OSharp:OAuth2配置的{name}节点的ClientSecret不能为空");
                }

                switch (name)
                {
                    case "QQ":
                        builder.AddQQ(opts =>
                        {
                            opts.AppId = options.ClientId;
                            opts.AppKey = options.ClientSecret;
                        });
                        break;
                    case "Microsoft":
                        builder.AddMicrosoftAccount(opts =>
                        {
                            opts.ClientId = options.ClientId;
                            opts.ClientSecret = options.ClientSecret;
                        });
                        break;
                    case "GitHub":
                        builder.AddGitHub(opts =>
                        {
                            opts.ClientId = options.ClientId;
                            opts.ClientSecret = options.ClientSecret;
                        });
                        break;
                }
            }

            return builder;
        }
    }
}