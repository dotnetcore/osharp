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
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 初始化一个<see cref="OnlineUserCache{TUser, TUserKey, TRole, TRoleKey}"/>类型的新实例
        /// </summary>
        public OnlineUserCache(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cache = serviceProvider.GetService<IDistributedCache>();
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
                    return _serviceProvider.ExecuteScopedWork<OnlineUser>(provider =>
                    {
                        IOnlineUserProvider onlineUserProvider = provider.GetService<IOnlineUserProvider>();
                        return onlineUserProvider.GetOrCreate(userName).Result;
                    });
                },
                options);
        }

        /// <summary>
        /// 异步获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public virtual async Task<OnlineUser> GetOrRefreshAsync(string userName)
        {
            string key = $"Identity_OnlineUser_{userName}";

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            return await _cache.GetAsync<OnlineUser>(key,
                () =>
                {
                    return _serviceProvider.ExecuteScopedWorkAsync<OnlineUser>(async provider =>
                    {
                        IOnlineUserProvider onlineUserProvider = provider.GetService<IOnlineUserProvider>();
                        return await onlineUserProvider.GetOrCreate(userName);
                    });
                },
                options);
        }

        /// <summary>
        /// 移除在线用户信息
        /// </summary>
        /// <param name="userNames">用户名</param>
        public virtual void Remove(params string[] userNames)
        {
            foreach (string userName in userNames)
            {
                string key = $"Identity_OnlineUser_{userName}";
                _cache.Remove(key);
            }
        }
    }
}