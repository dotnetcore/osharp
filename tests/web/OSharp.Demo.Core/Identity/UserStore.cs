using System;
using System.Collections.Generic;
using System.Text;

using OSharp.Demo.Identity.Entities;
using OSharp.Entity;
using OSharp.Identity;


namespace OSharp.Demo.Identity
{
    public class UserStore : UserStoreBase<User, int, UserClaim, UserLogin, UserToken, Role, int, UserRole>
    {
        /// <summary>
        /// 初始化一个<see cref="UserStoreBase{TUser, TUserKey, TUserClaim, TUserLogin, TUserToken, TRole, TRoleKey, TUserRole}"/>类型的新实例
        /// </summary>
        /// <param name="userRepository">用户仓储</param>
        /// <param name="userLoginRepository">用户登录仓储</param>
        /// <param name="userClaimRepository">用户声明仓储</param>
        /// <param name="userTokenRepository">用户令牌仓储</param>
        /// <param name="roleRepository">角色仓储</param>
        /// <param name="userRoleRepository">用户角色仓储</param>
        public UserStore(IRepository<User, int> userRepository, IRepository<UserLogin, Guid> userLoginRepository, IRepository<UserClaim, int> userClaimRepository, IRepository<UserToken, Guid> userTokenRepository, IRepository<Role, int> roleRepository, IRepository<UserRole, Guid> userRoleRepository)
            : base(userRepository, userLoginRepository, userClaimRepository, userTokenRepository, roleRepository, userRoleRepository)
        { }
    }
}
