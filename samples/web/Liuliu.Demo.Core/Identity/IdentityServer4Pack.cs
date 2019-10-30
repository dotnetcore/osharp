// -----------------------------------------------------------------------
//  <copyright file="IdentityPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using IdentityServer4.Configuration;
using Liuliu.Demo.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OSharp.Core.Options;
using OSharp.Exceptions;
using OSharp.Identity.IdentityServer4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;


namespace Liuliu.Demo.Identity
{
    /// <summary>
    /// 身份认证模块，此模块必须在MVC模块之前启动
    /// </summary>
    [Description("身份认证模块")]
    public class IdentityServer4Pack : IdentityServer4PackBase<UserStore, RoleStore, User, Role, int, int>
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
        /// 重写以实现<see cref="IdentityServerOptions"/>的配置
        /// </summary>
        /// <returns></returns>
        protected override Action<IdentityServerOptions> IdentityServerOptionsAction()
        {
            return options =>
            {
                options.Authentication.CookieAuthenticationScheme = "Cookies";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
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

            IConfigurationSection configurationSection = configuration.GetSection("OSharp:LocalApiAuthentication");
            LocalApiAuthenticationOptions localApiAuthentication = configurationSection.Get<LocalApiAuthenticationOptions>();
            // JwtBearer
            //AuthenticationBuilder authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            AuthenticationBuilder authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = "Cookies";
            })
            .AddCookie("Cookies")
            .AddJwtBearer("Bearer", options =>
            {
                //identityserver4 地址 也就是本项目地址
                options.Authority = localApiAuthentication.Url;
                options.RequireHttpsMetadata = localApiAuthentication.UseHttps;
                options.Audience = localApiAuthentication.Audience;
                options.Events = new JwtBearerEvents()
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
                //https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
                switch (pair.Key)
                {
                    case "QQ":
                        authenticationBuilder.AddQQ(opts =>
                        {
                            opts.ClientId = value.ClientId;
                            opts.ClientSecret = value.ClientSecret;
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
        protected override IdentityBuilder AddIdentityBuild(IdentityBuilder builder)
        {
            //如需要昵称唯一，启用下面这个验证码
            //builder.AddUserValidator<UserNickNameValidator<User, Guid>>();
            //builder.AddDefaultUI();
            return builder.AddDefaultTokenProviders();
        }

        /// <summary>
        /// 重写以实现 AddIdentityServer 之后的构建逻辑
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IIdentityServerBuilder AddIdentityServerBuild(IIdentityServerBuilder builder, IServiceCollection services)
        {
            return builder.AddDeveloperSigningCredential()
                          .AddInMemoryIdentityResources(Config.GetIdentityResources())
                          .AddInMemoryApiResources(Config.GetApis())
                          .AddInMemoryClients(Config.GetClients())
                          .AddOSharpIdentity<User>();// 默认
                          //.AddProfileService<>() // 自定义
                          //.AddResourceOwnerValidator<>();

            ////持久化
            ////IConfiguration configuration = services.GetConfiguration();
            ////Add-Migration InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
            ////Add-Migration InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
            ////IConfigurationSection section = configuration.GetSection("OSharp:Idxxxxxx");
            //string entryAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            //return builder
            //        // this adds the config data from DB (clients, resources)
            //        .AddConfigurationStore(options =>
            //        {
            //            options.ConfigureDbContext = build =>
            //                build.UseSqlServer("",
            //                    sql => sql.MigrationsAssembly(entryAssemblyName));
            //        })
            //        // this adds the operational data from DB (codes, tokens, consents)
            //        .AddOperationalStore(options =>
            //        {
            //            options.ConfigureDbContext = build =>
            //                build.UseSqlServer("",
            //                    sql => sql.MigrationsAssembly(entryAssemblyName));

            //            // this enables automatic token cleanup. this is optional.
            //            options.EnableTokenCleanup = true;
            //            options.TokenCleanupInterval = 30; // interval in seconds
            //        });
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseIdentityServer();

            //app.UseAuthentication();

            IsEnabled = true;
        }
    }
}