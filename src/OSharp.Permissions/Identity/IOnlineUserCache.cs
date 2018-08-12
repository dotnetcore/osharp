// -----------------------------------------------------------------------
//  <copyright file="IOnlineUserCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-09 11:20</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;


namespace OSharp.Identity
{
    /// <summary>
    /// 定义在线用户缓存
    /// </summary>
    public interface IOnlineUserCache
    {
        /// <summary>
        /// 获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        OnlineUser GetOrRefresh(string userName);

        /// <summary>
        /// 异步获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        Task<OnlineUser> GetOrRefreshAsync(string userName);

        /// <summary>
        /// 移除在线用户信息
        /// </summary>
        /// <param name="userNames">用户名</param>
        /// <returns>移除的用户信息</returns>
        void Remove(params string[] userNames);
    }
}