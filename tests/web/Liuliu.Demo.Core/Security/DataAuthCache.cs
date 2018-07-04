// -----------------------------------------------------------------------
//  <copyright file="DataAuthCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 18:25</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Security.Entities;

using OSharp.Core.EntityInfos;
using OSharp.Security;


namespace Liuliu.Demo.Security
{
    /// <summary>
    /// 数据权限缓存
    /// </summary>
    public class DataAuthCache : DataAuthCacheBase<EntityRole, Role, EntityInfo, int>
    { }
}