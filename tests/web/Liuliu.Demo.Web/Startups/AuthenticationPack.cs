// -----------------------------------------------------------------------
//  <copyright file="AuthenticationPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-24 23:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Identity.JwtBearer;


namespace Liuliu.Demo.Web.Startups
{
    public class AuthenticationPack : OsharpPack, IAspNetCoreBasePack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            //IConfiguration configuration = Singleton<IConfiguration>.Instance;
             
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(jwt =>
            //{
            //    string secret = configuration["OSharp:Jwt:Secret"];
            //    jwt.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidIssuer = configuration["OSharp:Jwt:Issuer"],
            //        ValidAudience = configuration["OSharp:Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
            //    };

            //    jwt.SecurityTokenValidators.Clear();
            //    jwt.SecurityTokenValidators.Add(new OnlineUserJwtSecurityTokenHandler());
            //}).AddQQ(qq =>
            //{
            //    qq.AppId = configuration["Authentication:QQ:AppId"];
            //    qq.AppKey = configuration["Authentication:QQ:AppKey"];
            //    qq.CallbackPath = new PathString("/api/identity/OAuth2Callback");
            //});
            return services;
        }

        /// <summary>
        /// 应用AspNetCore的模块初始化逻辑
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        /// <returns>应用程序构建器</returns>
        public IApplicationBuilder UsePack(IApplicationBuilder app)
        {
            //app.UseAuthentication();
            ILogger logger = ServiceLocator.Instance.GetLogger(GetType());
            logger.LogInformation("AuthenticationPack 模块初始化完毕");
            return app;
        }
    }
}