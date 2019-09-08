// -----------------------------------------------------------------------
//  <copyright file="IdentityPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

using OSharp.Core.Options;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Identity;
using OSharp.Identity.JwtBearer;


namespace Liuliu.Demo.Identity
{
    /// <summary>
    /// 身份认证模块，此模块必须在MVC模块之前启动
    /// </summary>
    [Description("身份认证模块")]
    public class IdentityPack : IdentityPackBase<UserStore, RoleStore, User, Role, int, int>
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
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
            services.AddScoped<IIdentityContract, IdentityService>();

            return base.AddServices(services);
        }

        /// <summary>
        /// 重写以实现<see cref="IdentityOptions"/>的配置
        /// </summary>
        /// <returns></returns>
        protected override Action<IdentityOptions> IdentityOptionsAction()
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
        protected override Action<CookieAuthenticationOptions> CookieOptionsAction()
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
        /// 添加Authentication服务
        /// </summary>
        /// <param name="services">服务集合</param>
        protected override void AddAuthentication(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();

            // JwtBearer
            AuthenticationBuilder authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            services.TryAddScoped<IJwtBearerService, JwtBearerService<User, int>>();
            services.TryAddScoped<IAccessClaimsProvider, AccessClaimsProvider<User, int>>();
            authenticationBuilder.AddJwtBearer(jwt =>
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

                jwt.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        // 生成SignalR的用户信息
                        string token = context.Request.Query["access_token"];
                        string path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.Contains("hub"))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // OAuth2
            IConfigurationSection section = configuration.GetSection("OSharp:OAuth2");
            IDictionary<string, OAuth2Options> dict = section.Get<Dictionary<string, OAuth2Options>>();
            if (dict == null)
            {
                return;
            }
            foreach (KeyValuePair<string, OAuth2Options> pair in dict)
            {
                OAuth2Options value = pair.Value;
                if (!value.Enabled)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(value.ClientId))
                {
                    throw new OsharpException($"配置文件中OSharp:OAuth2配置的{pair.Key}节点的ClientId不能为空");
                }
                if (string.IsNullOrEmpty(value.ClientSecret))
                {
                    throw new OsharpException($"配置文件中OSharp:OAuth2配置的{pair.Key}节点的ClientSecret不能为空");
                }

                switch (pair.Key)
                {
                    case "QQ":
                        authenticationBuilder.AddQQ(opts =>
                        {
                            opts.AppId = value.ClientId;
                            opts.AppKey = value.ClientSecret;
                        });
                        break;
                    case "Microsoft":
                        authenticationBuilder.AddMicrosoftAccount(opts =>
                        {
                            opts.ClientId = value.ClientId;
                            opts.ClientSecret = value.ClientSecret;
                        });
                        break;
                    case "GitHub":
                        authenticationBuilder.AddGitHub(opts =>
                        {
                            opts.ClientId = value.ClientId;
                            opts.ClientSecret = value.ClientSecret;
                        });
                        break;
                }
            }
        }

        /// <summary>
        /// 重写以实现 AddIdentity 之后的构建逻辑
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IdentityBuilder OnIdentityBuild(IdentityBuilder builder)
        {
            //如需要昵称唯一，启用下面这个验证码
            //builder.AddUserValidator<UserNickNameValidator<User, int>>();
            return builder.AddDefaultTokenProviders();
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
    }
}