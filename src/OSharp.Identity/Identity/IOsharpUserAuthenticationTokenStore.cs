// -----------------------------------------------------------------------
//  <copyright file="IOsharpUserAuthenticationTokenStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-03 11:44</last-date>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;


namespace OSharp.Identity
{
    /// <summary>
    /// 自定义UserAuthenticationTokenStore
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public interface IOsharpUserAuthenticationTokenStore<TUser> : IUserAuthenticationTokenStore<TUser>
        where TUser : class
    {
        /// <summary>
        /// 获取某个用户的所有指定登录提供者的权限标识
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="loginProvider">登录提供者</param>
        /// <param name="cancellationToken">任务取消标识</param>
        /// <returns>权限标识集合</returns>
        Task<string[]> GetTokensAsync(TUser user, string loginProvider, CancellationToken cancellationToken);
    }
}