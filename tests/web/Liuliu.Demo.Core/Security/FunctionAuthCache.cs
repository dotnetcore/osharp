// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Security.Entities;

using OSharp.Core.Functions;
using OSharp.Security;


namespace Liuliu.Demo.Security
{
    /// <summary>
    /// 功能权限缓存
    /// </summary>
    public class FunctionAuthCache : FunctionAuthCacheBase<ModuleFunction, ModuleRole, ModuleUser, Function, Module, int, Role, int, User, int>
    { }
}