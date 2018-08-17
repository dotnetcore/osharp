// -----------------------------------------------------------------------
//  <copyright file="OnlineUserProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-17 22:36</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Identity
{
    /// <summary>
    /// 在线用户信息提供者
    /// </summary>
    public class OnlineUserProvider<TUser, TUserKey, TRole, TRoleKey> : IOnlineUserProvider
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
        where TRole : RoleBase<TRoleKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        /// <summary>
        /// 创建在线用户信息
        /// </summary>
        /// <param name="provider">服务提供器</param>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public virtual async Task<OnlineUser> Create(IServiceProvider provider, string userName)
        {
            UserManager<TUser> userManager = provider.GetService<UserManager<TUser>>();
            TUser user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }
            IList<string> roles = await userManager.GetRolesAsync(user);
            RoleManager<TRole> roleManager = provider.GetService<RoleManager<TRole>>();
            bool isAdmin = roleManager.Roles.Any(m => roles.Contains(m.Name) && m.IsAdmin);
            return new OnlineUser()
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                NickName = user.NickName,
                Email = user.Email,
                HeadImg = user.HeadImg,
                IsAdmin = isAdmin,
                Roles = roles.ToArray()
            };
        }
    }
}