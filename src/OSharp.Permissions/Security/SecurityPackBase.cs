// -----------------------------------------------------------------------
//  <copyright file="SecurityPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:29</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Core.Packs;
using OSharp.Entity;
using OSharp.Secutiry;


namespace OSharp.Security
{
    /// <summary>
    /// 权限安全模块基类
    /// </summary>
    /// <typeparam name="TSecurityManager">安全管理器类型</typeparam>
    /// <typeparam name="TFunctionAuthorization">功能权限检查类型</typeparam>
    /// <typeparam name="TFunctionAuthCache">功能权限缓存类型</typeparam>
    /// <typeparam name="TDataAuthCache">数据权限缓存类型</typeparam>
    /// <typeparam name="TModuleHandler">模块处理器类型</typeparam>
    /// <typeparam name="TFunction">功能类型</typeparam>
    /// <typeparam name="TFunctionInputDto">功能输入DTO类型</typeparam>
    /// <typeparam name="TEntityInfo">实体信息类型</typeparam>
    /// <typeparam name="TEntityInfoInputDto">实体输入DTO类型</typeparam>
    /// <typeparam name="TModule">模块类型</typeparam>
    /// <typeparam name="TModuleInputDto">模块输入DTO类型</typeparam>
    /// <typeparam name="TModuleKey">模块编号类型</typeparam>
    /// <typeparam name="TModuleFunction">模块功能类型</typeparam>
    /// <typeparam name="TModuleRole">模块角色类型</typeparam>
    /// <typeparam name="TModuleUser">模块用户类型</typeparam>
    /// <typeparam name="TEntityRole">实体角色类型</typeparam>
    /// <typeparam name="TEntityRoleInputDto">实体角色输入DTO类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    public abstract class SecurityPackBase<TSecurityManager, TFunctionAuthorization, TFunctionAuthCache, TDataAuthCache, TModuleHandler, TFunction, TFunctionInputDto, TEntityInfo,
        TEntityInfoInputDto, TModule, TModuleInputDto, TModuleKey, TModuleFunction, TModuleRole, TModuleUser, TEntityRole, TEntityRoleInputDto, TRoleKey, TUserKey> : OsharpPack
        where TSecurityManager : class, IFunctionStore<TFunction, TFunctionInputDto>,
        IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>,
        IModuleStore<TModule, TModuleInputDto, TModuleKey>,
        IModuleFunctionStore<TModuleFunction, TModuleKey>,
        IModuleRoleStore<TModuleRole, TRoleKey, TModuleKey>,
        IModuleUserStore<TModuleUser, TUserKey, TModuleKey>,
        IEntityRoleStore<TEntityRole, TEntityRoleInputDto, TRoleKey>
        where TFunctionAuthorization : IFunctionAuthorization
        where TFunctionAuthCache : IFunctionAuthCache
        where TDataAuthCache : IDataAuthCache
        where TModuleHandler : IModuleHandler
        where TFunction : IFunction
        where TFunctionInputDto : FunctionInputDtoBase
        where TEntityInfo : IEntityInfo
        where TEntityInfoInputDto : EntityInfoInputDtoBase
        where TModule : ModuleBase<TModuleKey>
        where TModuleInputDto : ModuleInputDtoBase<TModuleKey>
        where TModuleFunction : ModuleFunctionBase<TModuleKey>
        where TModuleRole : ModuleRoleBase<TModuleKey, TRoleKey>
        where TModuleUser : ModuleUserBase<TModuleKey, TUserKey>
        where TEntityRole : EntityRoleBase<TRoleKey>
        where TEntityRoleInputDto : EntityRoleInputDtoBase<TRoleKey>
        where TModuleKey : struct, IEquatable<TModuleKey>
        where TRoleKey : IEquatable<TRoleKey>
        where TUserKey : IEquatable<TUserKey>
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
            services.AddScoped<TSecurityManager>();

            services.AddSingleton(typeof(IFunctionAuthorization), typeof(TFunctionAuthorization));
            services.AddSingleton(typeof(IFunctionAuthCache), typeof(TFunctionAuthCache));
            services.AddSingleton(typeof(IDataAuthCache), typeof(TDataAuthCache));
            services.AddSingleton(typeof(IModuleHandler), typeof(TModuleHandler));

            services.AddScoped(typeof(IFunctionStore<TFunction, TFunctionInputDto>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleStore<TModule, TModuleInputDto, TModuleKey>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleFunctionStore<TModuleFunction, TModuleKey>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleRoleStore<TModuleRole, TRoleKey, TModuleKey>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleUserStore<TModuleUser, TUserKey, TModuleKey>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IEntityRoleStore<TEntityRole, TEntityRoleInputDto, TRoleKey>), provider => provider.GetService<TSecurityManager>());

            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            IModuleHandler moduleHandler = provider.GetService<IModuleHandler>();
            moduleHandler.Initialize();

            //初始化各种缓存
            IFunctionHandler functionHandler = provider.GetService<IFunctionHandler>();
            functionHandler.RefreshCache();

            IEntityInfoHandler entityInfoHandler = provider.GetService<IEntityInfoHandler>();
            entityInfoHandler.RefreshCache();

            IFunctionAuthCache functionAuthCache = provider.GetService<IFunctionAuthCache>();
            functionAuthCache.BuildRoleCaches();

            IDataAuthCache dataAuthCache = provider.GetService<IDataAuthCache>();
            dataAuthCache.BuildCaches();

            IsEnabled = true;
        }
    }
}