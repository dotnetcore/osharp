// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Authorization.Entities;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Authorization;

/// <summary>
/// 功能权限缓存
/// </summary>
public class FunctionAuthCache : FunctionAuthCacheBase<ModuleFunction, ModuleRole, ModuleUser, Function, Module, long, Role, long, User, long>
{
    /// <summary>
    /// 初始化一个<see cref="FunctionAuthCacheBase{TModuleFunction, TModuleRole, TModuleUser, TFunction, TModule, TModuleKey,TRole, TRoleKey, TUser, TUserKey}"/>类型的新实例
    /// </summary>
    public FunctionAuthCache(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }
}
