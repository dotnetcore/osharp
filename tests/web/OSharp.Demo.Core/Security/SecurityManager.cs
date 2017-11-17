// -----------------------------------------------------------------------
//  <copyright file="SecurityManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-15 22:52</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Infrastructure;
using OSharp.Security;


namespace OSharp.Demo.Security
{
    /// <summary>
    /// 权限安全管理器
    /// </summary>
    public class SecurityManager : SecurityManagerBase<Function, FunctionInputDto, EntityInfo, EntityInfoInputDto>, IScopeDependency
    {
        /// <summary>
        /// 初始化一个<see cref="SecurityManager"/>类型的新实例
        /// </summary>
        public SecurityManager(
            IRepository<Function, Guid> functionRepository,
            IRepository<EntityInfo, Guid> entityInfoRepository)
            : base(functionRepository, entityInfoRepository)
        { }
    }
}