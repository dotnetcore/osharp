// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizationPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-26 23:37</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.Entities;
using OSharp.Authorization.Events;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Core.Packs;
using OSharp.EventBuses;


namespace OSharp.Authorization
{
    /// <summary>
    /// 功能权限模块基类
    /// </summary>
    /// <typeparam name="TFunctionAuthorizationManager">功能权限管理器</typeparam>
    /// <typeparam name="TFunctionAuthorization">功能权限验证器</typeparam>
    /// <typeparam name="TFunctionAuthCache">功能权限缓存</typeparam>
    /// <typeparam name="TModuleHandler">模块处理器</typeparam>
    /// <typeparam name="TFunction">功能类型</typeparam>
    /// <typeparam name="TFunctionInputDto">功能输入DTO类型</typeparam>
    /// <typeparam name="TModule">模块类型</typeparam>
    /// <typeparam name="TModuleInputDto">模块输入DTO类型</typeparam>
    /// <typeparam name="TModuleKey">模块编号类型</typeparam>
    /// <typeparam name="TModuleFunction">模块功能类型</typeparam>
    /// <typeparam name="TModuleRole">模块角色类型</typeparam>
    /// <typeparam name="TModuleUser">模块用户类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    [Description("功能权限模块")]
    [DependsOnPacks(typeof(EventBusPack), typeof(MvcFunctionPack))]
    public abstract class FunctionAuthorizationPackBase<TFunctionAuthorizationManager, TFunctionAuthorization, TFunctionAuthCache, TModuleHandler,
        TFunction, TFunctionInputDto,
        TModule, TModuleInputDto, TModuleKey, TModuleFunction, TModuleRole, TModuleUser, TRoleKey, TUserKey> : AspOsharpPack
        where TFunctionAuthorizationManager : class,
            IFunctionStore<TFunction, TFunctionInputDto>,
            IModuleStore<TModule, TModuleInputDto, TModuleKey>,
            IModuleFunctionStore<TModuleFunction, TModuleKey>,
            IModuleRoleStore<TModuleRole, TRoleKey, TModuleKey>,
            IModuleUserStore<TModuleUser, TUserKey, TModuleKey>
        where TFunctionAuthorization : class, IFunctionAuthorization
        where TFunctionAuthCache : class, IFunctionAuthCache
        where TModuleHandler : class, IModuleHandler
        where TFunction : IFunction
        where TFunctionInputDto : FunctionInputDtoBase
        where TModule : ModuleBase<TModuleKey>
        where TModuleInputDto : ModuleInputDtoBase<TModuleKey>
        where TModuleFunction : ModuleFunctionBase<TModuleKey>
        where TModuleRole : ModuleRoleBase<TModuleKey, TRoleKey>
        where TModuleUser : ModuleUserBase<TModuleKey, TUserKey>
        where TModuleKey : struct, IEquatable<TModuleKey>
        where TRoleKey : IEquatable<TRoleKey>
        where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddFunctionAuthorizationHandler();

            services.AddSingleton<IFunctionAuthorization, TFunctionAuthorization>();
            services.AddSingleton<IFunctionAuthCache, TFunctionAuthCache>();
            services.AddSingleton<IModuleHandler, TModuleHandler>();

            services.AddScoped<TFunctionAuthorizationManager>();
            services.AddScoped(typeof(IFunctionStore<TFunction, TFunctionInputDto>), provider => provider.GetService<TFunctionAuthorizationManager>());
            services.AddScoped(typeof(IModuleStore<TModule, TModuleInputDto, TModuleKey>), provider => provider.GetService<TFunctionAuthorizationManager>());
            services.AddScoped(typeof(IModuleFunctionStore<TModuleFunction, TModuleKey>), provider => provider.GetService<TFunctionAuthorizationManager>());
            services.AddScoped(typeof(IModuleRoleStore<TModuleRole, TRoleKey, TModuleKey>), provider => provider.GetService<TFunctionAuthorizationManager>());
            services.AddScoped(typeof(IModuleUserStore<TModuleUser, TUserKey, TModuleKey>), provider => provider.GetService<TFunctionAuthorizationManager>());

            services.AddEventHandler<FunctionAuthCacheRefreshEventHandler>();
            services.AddEventHandler<FunctionCacheRefreshEventHandler>();

            return services;
        }

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseCookiePolicy();
            app.UseFunctionAuthorization();
            IsEnabled = true;
        }
    }
}