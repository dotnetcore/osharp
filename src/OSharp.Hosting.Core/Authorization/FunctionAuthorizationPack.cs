// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizationPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 0:29</last-date>
// -----------------------------------------------------------------------

using OSharp.Authorization.Dtos;
using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Authorization.Entities;
using OSharp.Hosting.Identity;


namespace OSharp.Hosting.Authorization;

[DependsOnPacks(typeof(IdentityPack), typeof(MvcFunctionPack))]
public class FunctionAuthorizationPack
    : FunctionAuthorizationPackBase<FunctionAuthManager, FunctionAuthorization, FunctionAuthCache, ModuleHandler, Function,
        FunctionInputDto, Module, ModuleInputDto, long, ModuleFunction, ModuleRole, ModuleUser, long, long>
{
    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public override IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<ISeedDataInitializer, ModuleSeedDataInitializer>();

        return base.AddServices(services);
    }
}
