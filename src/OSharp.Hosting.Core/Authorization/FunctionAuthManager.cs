// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizationManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-27 0:26</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Authorization.Entities;
using OSharp.Hosting.Identity.Entities;

using OSharp.Authorization;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.Functions;


namespace OSharp.Hosting.Authorization
{
    /// <summary>
    /// 功能权限管理器
    /// </summary>
    public class FunctionAuthManager
        : FunctionAuthorizationManagerBase<Function, FunctionInputDto, Module, ModuleInputDto, int, ModuleFunction, ModuleRole, ModuleUser, UserRole,
            Guid, Role, int, User, int>
    {
        /// <summary>
        /// 初始化一个 SecurityManager 类型的新实例
        /// </summary>
        /// <param name="provider">服务提供程序</param>
        public FunctionAuthManager(IServiceProvider provider)
            : base(provider)
        { }
    }
}