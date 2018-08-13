// -----------------------------------------------------------------------
//  <copyright file="OnlineUserCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-09 12:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Caching;
using OSharp.Dependency;


namespace OSharp.Identity
{
    /// <summary>
    /// 在线用户缓存，以数据库中最新数据为来源的用户信息缓存
    /// </summary>
    /// <typeparam name="TUser">用户类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    /// <typeparam name="TRole">角色类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    public class OnlineUserCache<TUser, TUserKey, TRole, TRoleKey> : IOnlineUserCache
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
        where TRole : RoleBase<TRoleKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 初始化一个<see cref="OnlineUserCache{TUser, TUserKey, TRole, TRoleKey}"/>类型的新实例
        /// </summary>
        public OnlineUserCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public virtual OnlineUser GetOrRefresh(string userName)
        {
            string key = $"Identity_OnlineUser_{userName}";

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            return _cache.Get<OnlineUser>(key,
                () =>
                {
                    return ServiceLocator.Instance.ExcuteScopedWork<OnlineUser>(provider =>
                    {
                        UserManager<TUser> userManager = provider.GetService<UserManager<TUser>>();
                        TUser user = userManager.FindByNameAsync(userName).Result;
                        if (user == null)
                        {
                            return null;
                        }
                        IList<string> roles = userManager.GetRolesAsync(user).Result;

                        RoleManager<TRole> roleManager = provider.GetService<RoleManager<TRole>>();
                        bool isAdmin = roleManager.Roles.Any(m => roles.Contains(m.Name) && m.IsAdmin);

                        return GetOnlineUser(user, roles.ToArray(), isAdmin);
                    });
                },
                options);
        }

        /// <summary>
        /// 异步获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public async Task<OnlineUser> GetOrRefreshAsync(string userName)
        {
            string key = $"Identity_OnlineUser_{userName}";

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            return await _cache.GetAsync<OnlineUser>(key,
                () =>
                {
                    return ServiceLocator.Instance.ExcuteScopedWorkAsync<OnlineUser>(async provider =>
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

                        return GetOnlineUser(user, roles.ToArray(), isAdmin);
                    });
                },
                options);
        }

        /// <summary>
        /// 移除在线用户信息
        /// </summary>
        /// <param name="userNames">用户名</param>
        public void Remove(params string[] userNames)
        {
            foreach (string userName in userNames)
            {
                string key = $"Identity_OnlineUser_{userName}";
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 从用户实例中获取在线用户信息
        /// </summary>
        /// <param name="user">来自数据库的用户实例</param>
        /// <param name="roles">用户拥有的角色</param>
        /// <param name="isAdmin">是否管理</param>
        /// <returns>在线用户信息</returns>
        private static OnlineUser GetOnlineUser(TUser user, string[] roles, bool isAdmin)
        {
            return new OnlineUser()
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                NickName = user.NickName,
                Email = user.Email,
                HeadImg = user.HeadImg,
                IsAdmin = isAdmin,
                Roles = roles
            };
        }
    }
}