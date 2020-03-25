// -----------------------------------------------------------------------
//  <copyright file="ConnectionUserCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-04 20:30</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

using OSharp.Caching;
using OSharp.Collections;


namespace OSharp.AspNetCore.SignalR
{
    /// <summary>
    /// SignalR连接用户缓存
    /// </summary>
    public class ConnectionUserCache : IConnectionUserCache
    {
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 初始化一个<see cref="ConnectionUserCache"/>类型的新实例
        /// </summary>
        public ConnectionUserCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 获取缓存的用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public virtual async Task<ConnectionUser> GetUser(string userName)
        {
            string key = GetKey(userName);
            return await _cache.GetAsync<ConnectionUser>(key);
        }

        /// <summary>
        /// 获取指定用户的所有连接Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public virtual async Task<string[]> GetConnectionIds(string userName)
        {
            ConnectionUser user = await GetUser(userName);
            return user?.ConnectionIds.ToArray() ?? new string[0];
        }

        /// <summary>
        /// 设置用户缓存
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public virtual async Task SetUser(string userName, ConnectionUser user)
        {
            string key = GetKey(userName);
            await _cache.SetAsync(key, user);
        }

        /// <summary>
        /// 添加指定用户的连接
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="connectionId">连接Id</param>
        /// <returns></returns>
        public virtual async Task AddConnectionId(string userName, string connectionId)
        {
            ConnectionUser user = await GetUser(userName) ?? new ConnectionUser() { UserName = userName };
            user.ConnectionIds.AddIfNotExist(connectionId);
            await SetUser(userName, user);
        }

        /// <summary>
        /// 移除指定用户的连接
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="connectionId">连接Id</param>
        /// <returns></returns>
        public virtual async Task RemoveConnectionId(string userName, string connectionId)
        {
            ConnectionUser user = await GetUser(userName);
            if (user == null || !user.ConnectionIds.Contains(connectionId))
            {
                return;
            }
            user.ConnectionIds.Remove(connectionId);
            if (user.ConnectionIds.Count == 0)
            {
                await RemoveUser(userName);
                return;
            }
            await SetUser(userName, user);
        }

        /// <summary>
        /// 移除指定用户的缓存
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public virtual async Task RemoveUser(string userName)
        {
            string key = GetKey(userName);
            await _cache.RemoveAsync(key);
        }

        private static string GetKey(string userName)
        {
            return $"SignalR:Connection_User:{userName}";
        }
    }
}