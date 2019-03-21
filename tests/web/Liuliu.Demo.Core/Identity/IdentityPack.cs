// -----------------------------------------------------------------------
//  <copyright file="IdentityPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;
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
using Microsoft.IdentityModel.Tokens;

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
                options.SignIn.RequireConfirmedEmail = true;
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
            AuthenticationBuilder authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                string secret = configuration["Authentication:Jwt:Secret"];
                if (secret.IsNullOrEmpty())
                {
                    throw new OsharpException("配置文件中Authentication配置的Jwt节点的Secret不能为空");
                }

                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["Authentication:Jwt:Issuer"]?? "osharp identity",
                    ValidAudience = configuration["Authentication:Jwt:Audience"]?? "osharp client",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.Now,
                    ValidateLifetime = true
                };

                jwt.SecurityTokenValidators.Clear();
                jwt.SecurityTokenValidators.Add(new OnlineUserJwtSecurityTokenHandler());
                jwt.Events = new JwtBearerEvents()
                {
                    // 生成SignalR的用户信息
                    OnMessageReceived = context =>
                    {
                        string token = context.Request.Query["access_token"];
                        string path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.Contains("hub"))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            bool enabled = configuration["Authentication:QQ:Enabled"].CastTo(false);
            if (enabled)
            {
                string appId = configuration["Authentication:QQ:AppId"];
                if (string.IsNullOrEmpty(appId))
                {
                    throw new OsharpException("配置文件中Authentication配置的QQ节点的AppId不能为空");
                }
                string appKey = configuration["Authentication:QQ:AppKey"];
                if (string.IsNullOrEmpty(appKey))
                {
                    throw new OsharpException("配置文件中Authentication配置的QQ节点的AppKey不能为空");
                }
                authenticationBuilder.AddQQ(qq =>
                {
                    qq.AppId = appId;
                    qq.AppKey = appKey;
                });
            }

            enabled = configuration["Authentication:Microsoft:Enabled"].CastTo(false);
            if (enabled)
            {
                string clientId = configuration["Authentication:Microsoft:ClientId"];
                if (string.IsNullOrEmpty(clientId))
                {
                    throw new OsharpException("配置文件中Authentication配置的Microsoft节点的ClientId不能为空");
                }
                string clientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                if (string.IsNullOrEmpty(clientSecret))
                {
                    throw new OsharpException("配置文件中Authentication配置的Microsoft节点的ClientSecret不能为空");
                }
                authenticationBuilder.AddMicrosoftAccount(ms =>
                {
                    ms.ClientId = clientId;
                    ms.ClientSecret = clientSecret;
                });
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