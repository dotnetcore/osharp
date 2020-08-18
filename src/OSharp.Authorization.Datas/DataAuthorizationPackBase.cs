// -----------------------------------------------------------------------
//  <copyright file="DataAuthorizationPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-26 23:23</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Dtos;
using OSharp.Authorization.Entities;
using OSharp.Authorization.EntityInfos;
using OSharp.Authorization.Events;
using OSharp.Core.Packs;
using OSharp.EventBuses;


namespace OSharp.Authorization
{
    /// <summary>
    /// 数据权限模块基类
    /// </summary>
    /// <typeparam name="TDataAuthorizationManager">数据权限管理器</typeparam>
    /// <typeparam name="TDataAuthCache">数据权限缓存</typeparam>
    /// <typeparam name="TEntityInfo">数据实体类型</typeparam>
    /// <typeparam name="TEntityInfoInputDto">数据实体输入DTO类型</typeparam>
    /// <typeparam name="TEntityRole">实体角色类型</typeparam>
    /// <typeparam name="TEntityRoleInputDto">实体角色输入DTO类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    [Description("数据权限模块")]
    [DependsOnPacks(typeof(EventBusPack), typeof(EntityInfoPack))]
    public abstract class DataAuthorizationPackBase<TDataAuthorizationManager, TDataAuthCache, TEntityInfo, TEntityInfoInputDto, TEntityRole,
        TEntityRoleInputDto, TRoleKey> : OsharpPack
        where TDataAuthorizationManager : class,
            IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>,
            IEntityRoleStore<TEntityRole, TEntityRoleInputDto, TRoleKey>
        where TDataAuthCache : IDataAuthCache
        where TEntityInfo : IEntityInfo
        where TEntityInfoInputDto : EntityInfoInputDtoBase
        where TEntityRole : EntityRoleBase<TRoleKey>
        where TEntityRoleInputDto : EntityRoleInputDtoBase<TRoleKey>
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
            services.AddSingleton(typeof(IDataAuthCache), typeof(TDataAuthCache));

            services.AddScoped<TDataAuthorizationManager>();
            services.AddScoped(typeof(IEntityInfoStore<TEntityInfo, TEntityInfoInputDto>), provider => provider.GetService<TDataAuthorizationManager>());
            services.AddScoped(typeof(IEntityRoleStore<TEntityRole, TEntityRoleInputDto, TRoleKey>), provider => provider.GetService<TDataAuthorizationManager>());

            services.AddEventHandler<DataAuthCacheRefreshEventHandler>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            IEntityInfoHandler entityInfoHandler = provider.GetService<IEntityInfoHandler>();
            entityInfoHandler.RefreshCache();

            IDataAuthCache dataAuthCache = provider.GetService<IDataAuthCache>();
            dataAuthCache.BuildCaches();

            IsEnabled = true;
        }

    }
}