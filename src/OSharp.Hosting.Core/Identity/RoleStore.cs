// -----------------------------------------------------------------------
//  <copyright file="RoleStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;

using OSharp.Entity;
using OSharp.Identity;


namespace OSharp.Hosting.Identity
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class RoleStore : RoleStoreBase<Role, int, RoleClaim, int>
    {
        /// <summary>
        /// 初始化一个<see cref="RoleStoreBase{TRole,TRoleKey,TRoleClaim, TRoleClaimKey}"/>类型的新实例
        /// </summary>
        public RoleStore(IRepository<Role, int> roleRepository, IRepository<RoleClaim, int> roleClaimRepository)
            : base(roleRepository, roleClaimRepository)
        { }
    }
}