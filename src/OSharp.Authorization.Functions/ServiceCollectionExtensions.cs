// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 23:41</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;


namespace OSharp.Authorization
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加名称为 OsharpPolicy 的授权策略
        /// </summary>
        public static IServiceCollection AddFunctionAuthorizationHandler(this IServiceCollection services)
        {
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(FunctionRequirement.OsharpPolicy, policy => policy.Requirements.Add(new FunctionRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, FunctionAuthorizationHandler>();

            return services;
        }

        /// <summary>
        /// 应用功能权限授权
        /// </summary>
        public static IApplicationBuilder UseFunctionAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthorization();

            IServiceProvider provider = app.ApplicationServices;

            IModuleHandler moduleHandler = provider.GetService<IModuleHandler>();
            moduleHandler.Initialize();

            IFunctionHandler functionHandler = provider.GetService<IFunctionHandler>();
            functionHandler.RefreshCache();

            return app;
        }
    }
}