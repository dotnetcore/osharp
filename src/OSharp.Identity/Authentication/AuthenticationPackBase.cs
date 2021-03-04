// -----------------------------------------------------------------------
//  <copyright file="AuthenticationPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-02 22:23</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

using OSharp.AspNetCore;
using OSharp.Authentication.Cookies;
using OSharp.Authentication.JwtBearer;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Identity.Entities;
using OSharp.Identity.JwtBearer;


namespace OSharp.Authentication
{
    /// <summary>
    /// 身份认证模块基类
    /// </summary>
    [Description("身份认证模块")]
    [DependsOnPacks(typeof(AspNetCorePack))]
    public abstract class AuthenticationPackBase<TUser, TUserKey> : AspOsharpPack
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
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
            OsharpOptions options = services.GetOsharpOptions();
            services.TryAddScoped<IAccessClaimsProvider, AccessClaimsProvider<TUser, TUserKey>>();

            string defaultSchema = IdentityConstants.ApplicationScheme;
            if (options.Jwt?.Enabled == true && options.Cookie?.Enabled != true)
            {
                defaultSchema = JwtBearerDefaults.AuthenticationScheme;
            }
            AuthenticationBuilder builder = services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = defaultSchema;
                opts.DefaultAuthenticateScheme = defaultSchema;
            });
            AddJwtBearer(services, builder);
            AddCookie(services, builder);
            AddOAuth2(services, builder);

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseAuthentication();

            IsEnabled = true;
        }

        /// <summary>
        /// 添加JwtBearer支持
        /// </summary>
        protected virtual AuthenticationBuilder AddJwtBearer(IServiceCollection services, AuthenticationBuilder builder)
        {
            OsharpOptions option = services.GetOsharpOptions();
            JwtOptions jwt = option.Jwt;
            if (jwt?.Enabled != true)
            {
                return builder;
            }

            services.TryAddScoped<IJwtBearerService, JwtBearerService<TUser, TUserKey>>();
            builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                opts =>
                {
                    string secret = jwt.Secret;
                    if (secret.IsNullOrEmpty())
                    {
                        throw new OsharpException("配置文件中节点OSharp:Jwt:Secret不能为空");
                    }

                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = jwt.Issuer ?? "osharp identity",
                        ValidAudience = jwt.Audience ?? "osharp client",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        LifetimeValidator = (nbf, exp, token, param) => exp > DateTime.UtcNow
                    };

                    opts.Events = new OsharpJwtBearerEvents();
                });

            return builder;
        }

        protected virtual AuthenticationBuilder AddCookie(IServiceCollection services, AuthenticationBuilder builder)
        {
            OsharpOptions options = services.GetOsharpOptions();
            CookieOptions cookie = options.Cookie;
            if (cookie?.Enabled != true)
            {
                return builder;
            }

            services.AddScoped<OsharpCookieAuthenticationEvents>();
            builder.AddIdentityCookies(b =>
            {
                b.ApplicationCookie.Configure(opts =>
                {
                    if (cookie.CookieName != null)
                    {
                        opts.Cookie.Name = cookie.CookieName;
                    }

                    opts.LoginPath = cookie.LoginPath ?? opts.LoginPath;
                    opts.LogoutPath = cookie.LogoutPath ?? opts.LogoutPath;
                    opts.AccessDeniedPath = cookie.AccessDeniedPath ?? opts.AccessDeniedPath;
                    opts.ReturnUrlParameter = cookie.ReturnUrlParameter ?? opts.ReturnUrlParameter;
                    opts.SlidingExpiration = cookie.SlidingExpiration;
                    if (cookie.ExpireMins > 0)
                    {
                        opts.ExpireTimeSpan = TimeSpan.FromMinutes(cookie.ExpireMins);
                    }

                    opts.EventsType = typeof(OsharpCookieAuthenticationEvents);
                });
            });
            return builder;
        }

        /// <summary>
        /// 添加OAuth2第三方登录配置
        /// </summary>
        protected virtual AuthenticationBuilder AddOAuth2(IServiceCollection services, AuthenticationBuilder builder)
        {
            OsharpOptions osharpOptions = services.GetOsharpOptions();
            IDictionary<string, OAuth2Options> dict = osharpOptions.OAuth2S;
            if (dict == null || dict.Count == 0)
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
