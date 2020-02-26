// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizationManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 0:26</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Authorization.Entities;
using Liuliu.Demo.Identity.Entities;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.Functions;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.EventBuses;


namespace Liuliu.Demo.Authorization
{
    /// <summary>
    /// 功能权限管理器
    /// </summary>
    [Dependency(ServiceLifetime.Scoped, AddSelf = true)]
    public class FunctionAuthManager
        : FunctionAuthorizationManagerBase<Function, FunctionInputDto, Module, ModuleInputDto, int, ModuleFunction, ModuleRole, ModuleUser, UserRole,
            Guid, Role, int, User, int>
    {
        /// <summary>
        /// 初始化一个 SecurityManager 类型的新实例
        /// </summary>
        /// <param name="eventBus">事件总线</param>
        /// <param name="functionRepository">功能仓储</param>
        /// <param name="moduleRepository">模块仓储</param>
        /// <param name="moduleFunctionRepository">模块功能仓储</param>
        /// <param name="moduleRoleRepository">模块角色仓储</param>
        /// <param name="moduleUserRepository">模块用户仓储</param>
        /// <param name="roleRepository">角色仓储</param>
        /// <param name="userRepository">用户仓储</param>
        /// <param name="userRoleRepository">用户角色仓储</param>
        public FunctionAuthManager(IEventBus eventBus,
            IRepository<Function, Guid> functionRepository,
            IRepository<Module, int> moduleRepository,
            IRepository<ModuleFunction, Guid> moduleFunctionRepository,
            IRepository<ModuleRole, Guid> moduleRoleRepository,
            IRepository<ModuleUser, Guid> moduleUserRepository,
            IRepository<UserRole, Guid> userRoleRepository,
            IRepository<Role, int> roleRepository,
            IRepository<User, int> userRepository)
            : base(eventBus,
                functionRepository,
                moduleRepository,
                moduleFunctionRepository,
                moduleRoleRepository,
                moduleUserRepository,
                userRoleRepository,
                roleRepository,
                userRepository)
        { }
    }
}