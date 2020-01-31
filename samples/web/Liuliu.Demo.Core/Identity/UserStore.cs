// -----------------------------------------------------------------------
//  <copyright file="UserStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-01-31 19:34</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Identity.Entities;

using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Identity;


namespace Liuliu.Demo.Identity
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class UserStore : UserStoreBase<User, int, UserClaim, int, UserLogin, Guid, UserToken, Guid, Role, int, UserRole, Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="UserStoreBase{TUser, TUserKey, TUserClaim, TUserClaimKey, TUserLogin, TUserLoginKey, TUserToken, TUserTokenKey, TRole, TRoleKey, TUserRole, TUserRoleKey}"/>类型的新实例
        /// </summary>
        /// <param name="userRepository">用户仓储</param>
        /// <param name="userLoginRepository">用户登录仓储</param>
        /// <param name="userClaimRepository">用户声明仓储</param>
        /// <param name="userTokenRepository">用户令牌仓储</param>
        /// <param name="roleRepository">角色仓储</param>
        /// <param name="userRoleRepository">用户角色仓储</param>
        /// <param name="eventBus">事件总线</param>
        public UserStore(IRepository<User, int> userRepository,
            IRepository<UserLogin, Guid> userLoginRepository,
            IRepository<UserClaim, int> userClaimRepository,
            IRepository<UserToken, Guid> userTokenRepository,
            IRepository<Role, int> roleRepository,
            IRepository<UserRole, Guid> userRoleRepository,
            IEventBus eventBus)
            : base(userRepository, userLoginRepository, userClaimRepository, userTokenRepository, roleRepository, userRoleRepository, eventBus)
        { }
    }
}