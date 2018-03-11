// -----------------------------------------------------------------------
//  <copyright file="SecurityManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 14:58</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Demo.Security.Dtos;
using OSharp.Demo.Security.Entities;
using OSharp.Entity;
using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Security;


namespace OSharp.Demo.Security
{
    /// <summary>
    /// 权限安全管理器
    /// </summary>
    public class SecurityManager
        : SecurityManagerBase<Function, FunctionInputDto, EntityInfo, EntityInfoInputDto,
            Module, ModuleInputDto, int, ModuleFunction, ModuleRole, ModuleUser, int, int>
    {
        /// <summary>
        /// 初始化一个<see cref="SecurityManager"/>类型的新实例
        /// </summary>
        public SecurityManager(
            IRepository<Function, Guid> functionRepository,
            IRepository<EntityInfo, Guid> entityInfoRepository,
            IRepository<Module, int> moduleRepository,
            IRepository<ModuleFunction, Guid> moduleFunctionRepository,
            IRepository<ModuleRole, Guid> moduleRoleRepository,
            IRepository<ModuleUser, Guid> moduleUserRepository)
            : base(functionRepository, entityInfoRepository, moduleRepository, moduleFunctionRepository, moduleRoleRepository, moduleUserRepository)
        { }
    }
}
