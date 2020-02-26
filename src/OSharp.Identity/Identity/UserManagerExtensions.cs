// -----------------------------------------------------------------------
//  <copyright file="UserManagerExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 5:37</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Identity.JwtBearer;
using OSharp.Json;


namespace OSharp.Identity
{
    /// <summary>
    /// <see cref="UserManager{TUser}"/>扩展
    /// </summary>
    public static class UserManagerExtensions
    {
        /// <summary>
        /// 获取RefreshToken
        /// </summary>
        public static async Task<RefreshToken> GetRefreshToken<TUser>(this UserManager<TUser> userManager, string userId, string clientId)
            where TUser : class
        {
            TUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new OsharpException($"编号为“{userId}”的用户信息不存在");
            }
            return await userManager.GetRefreshToken(user, clientId);
        }

        /// <summary>
        /// 获取RefreshToken
        /// </summary>
        public static async Task<RefreshToken> GetRefreshToken<TUser>(this UserManager<TUser> userManager, TUser user, string clientId)
            where TUser : class
        {
            const string loginProvider = "JwtBearer";
            string tokenName = $"RefreshToken_{clientId}";
            string json = await userManager.GetAuthenticationTokenAsync(user, loginProvider, tokenName);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return json.FromJsonString<RefreshToken>();
        }

        /// <summary>
        /// 设置RefreshToken
        /// </summary>
        public static async Task<IdentityResult> SetRefreshToken<TUser>(this UserManager<TUser> userManager, string userId, RefreshToken token)
            where TUser : class
        {
            TUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new IdentityResult().Failed($"编号为“{userId}”的用户信息不存在");
            }
            return await userManager.SetRefreshToken(user, token);
        }

        /// <summary>
        /// 设置RefreshToken
        /// </summary>
        public static Task<IdentityResult> SetRefreshToken<TUser>(this UserManager<TUser> userManager, TUser user, RefreshToken token)
            where TUser : class
        {
            const string loginProvider = "JwtBearer";
            string tokenName = $"RefreshToken_{token.ClientId}";
            string json = token.ToJsonString();
            return userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenName, json);
        }

        /// <summary>
        /// 移除RefreshToken
        /// </summary>
        public static async Task<IdentityResult> RemoveRefreshToken<TUser>(this UserManager<TUser> userManager, string userId, string clientId)
            where TUser : class
        {
            TUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new IdentityResult().Failed($"编号为“{userId}”的用户信息不存在");
            }
            return await RemoveRefreshToken(userManager, user, clientId);
        }

        /// <summary>
        /// 移除RefreshToken
        /// </summary>
        public static Task<IdentityResult> RemoveRefreshToken<TUser>(this UserManager<TUser> userManager, TUser user, string clientId)
            where TUser : class
        {
            const string loginProvider = "JwtBearer";
            string tokenName = $"RefreshToken_{clientId}";
            return userManager.RemoveAuthenticationTokenAsync(user, loginProvider, tokenName);
        }
    }
}