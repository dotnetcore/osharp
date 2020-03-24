// -----------------------------------------------------------------------
//  <copyright file="DataAuthorizationManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 0:31</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Authorization.Entities;
using Liuliu.Demo.Identity.Entities;

using OSharp.Authorization.DataAuthorization;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.EntityInfos;
using OSharp.Entity;
using OSharp.EventBuses;


namespace Liuliu.Demo.Authorization
{
    /// <summary>
    /// 数据权限管理器
    /// </summary>
    //[Dependency(ServiceLifetime.Scoped, AddSelf = true)]
    public class DataAuthManager : DataAuthorizationManagerBase<EntityInfo, EntityInfoInputDto, EntityRole, EntityRoleInputDto, Role, int>
    {
        /// <summary>
        /// 初始化一个 SecurityManager 类型的新实例
        /// </summary>
        /// <param name="eventBus">事件总线</param>
        /// <param name="entityInfoRepository">实体仓储</param>
        /// <param name="entityRoleRepository">实体角色仓储</param>
        /// <param name="roleRepository">角色仓储</param>
        public DataAuthManager(IEventBus eventBus,
            IRepository<EntityInfo, Guid> entityInfoRepository,
            IRepository<EntityRole, Guid> entityRoleRepository,
            IRepository<Role, int> roleRepository)
            : base(eventBus, entityInfoRepository, entityRoleRepository, roleRepository)
        { }
    }
}