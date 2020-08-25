// -----------------------------------------------------------------------
//  <copyright file="DataAuthorizationPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 0:35</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Authorization.Entities;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.EntityInfos;
using OSharp.AutoMapper;


namespace OSharp.Hosting.Authorization
{
    public class DataAuthorizationPack
        : DataAuthorizationPackBase<DataAuthManager, DataAuthCache, EntityInfo, EntityInfoInputDto, EntityRole, EntityRoleInputDto, int>
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<IAutoMapperConfiguration, AutoMapperConfiguration>();

            return base.AddServices(services);
        }
    }
}