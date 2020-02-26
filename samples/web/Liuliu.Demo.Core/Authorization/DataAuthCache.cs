// -----------------------------------------------------------------------
//  <copyright file="DataAuthCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 18:25</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Authorization.Entities;
using Liuliu.Demo.Identity.Entities;

using OSharp.Authorization;
using OSharp.Authorization.EntityInfos;


namespace Liuliu.Demo.Authorization
{
    /// <summary>
    /// 数据权限缓存
    /// </summary>
    public class DataAuthCache : DataAuthCacheBase<EntityRole, Role, EntityInfo, int>
    {
        /// <summary>
        /// 初始化一个<see cref="DataAuthCacheBase{TEntityRole, TRole, TEntityInfo, TRoleKey}"/>类型的新实例
        /// </summary>
        public DataAuthCache(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }
}