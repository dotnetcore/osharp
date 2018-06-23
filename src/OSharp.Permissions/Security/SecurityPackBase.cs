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
using OSharp.Core.Packs;
using OSharp.Entity;
using OSharp.Secutiry;


namespace OSharp.Security
{
    /// <summary>
    /// 权限安全模块基类
    /// </summary>
    public abstract class SecurityPackBase<TSecurityManager, TFunctionAuthorization, TFunctionAuthCache, TFunction, TFunctionInputDto, TEntityInfo,
        TEntityInfoInputDto, TModule, TModuleInputDto, TModuleKey, TModuleFunction, TModuleRole, TModuleUser, TRoleKey, TUserKey> : OsharpPack
        where TSecurityManager : class, IFunctionStore<TFunction, TFunctionInputDto>,
        IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>,
        IModuleStore<TModule, TModuleInputDto, TModuleKey>,
        IModuleFunctionStore<TModuleFunction, TModuleKey>,
        IModuleRoleStore<TModuleRole, TRoleKey, TModuleKey>,
        IModuleUserStore<TModuleUser, TUserKey, TModuleKey>
        where TFunctionAuthorization : IFunctionAuthorization
        where TFunctionAuthCache : IFunctionAuthCache
        where TFunction : IFunction, IEntity<Guid>
        where TFunctionInputDto : FunctionInputDtoBase
        where TEntityInfo : IEntityInfo, IEntity<Guid>
        where TEntityInfoInputDto : EntityInfoInputDtoBase
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

            services.AddScoped(typeof(IFunctionStore<TFunction, TFunctionInputDto>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleStore<TModule, TModuleInputDto, TModuleKey>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleFunctionStore<TModuleFunction, TModuleKey>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleRoleStore<TModuleRole, TRoleKey, TModuleKey>), provider => provider.GetService<TSecurityManager>());
            services.AddScoped(typeof(IModuleUserStore<TModuleUser, TUserKey, TModuleKey>), provider => provider.GetService<TSecurityManager>());

            return services;
        }
    }
}