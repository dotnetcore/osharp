// -----------------------------------------------------------------------
//  <copyright file="DataAuthorizationManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 0:31</last-date>
// -----------------------------------------------------------------------

using OSharp.Authorization.Dtos;
using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Authorization.Entities;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Authorization;

/// <summary>
/// 数据权限管理器
/// </summary>
//[Dependency(ServiceLifetime.Scoped, AddSelf = true)]
public class DataAuthManager : DataAuthorizationManagerBase<EntityInfo, EntityInfoInputDto, EntityRole, EntityRoleInputDto, Role, long>
{
    /// <summary>
    /// 初始化一个 SecurityManager 类型的新实例
    /// </summary>
    public DataAuthManager(IServiceProvider provider)
        : base(provider)
    { }
}
