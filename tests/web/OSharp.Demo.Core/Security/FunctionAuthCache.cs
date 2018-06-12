// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-11 1:02</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.Caching.Distributed;

using OSharp.Core.Functions;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security.Entities;
using OSharp.Security;


namespace OSharp.Demo.Security
{
    public class FunctionAuthCache : FunctionAuthCacheBase<ModuleFunction, ModuleRole, ModuleUser, Function, Module, int, Role, int, User, int>
    {
        /// <summary>
        /// 初始化一个<see cref="FunctionAuthCacheBase{TFunction,TModule,TModuleKey,TModuleFunction,TModuleRole,TModuleUser,TRole,TRoleKey,TUser,TUserKey}"/>类型的新实例
        /// </summary>
        public FunctionAuthCache(IDistributedCache cache)
            : base(cache)
        { }
    }
}