// -----------------------------------------------------------------------
//  <copyright file="AccessClaimsProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-15 12:19</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Identity;
using OSharp.Identity.Entities;


namespace OSharp.Authentication.JwtBearer
{
    /// <summary>
    /// AccessToken的用户Claims提供器
    /// </summary>
    /// <typeparam name="TUser">用户类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    public class AccessClaimsProvider<TUser, TUserKey> : IAccessClaimsProvider
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="AccessClaimsProvider{TUser, TUserKey}"/>类型的新实例
        /// </summary>
        public AccessClaimsProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 创建用户标识
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Claim[]> CreateClaims(string userId)
        {
            Check.NotNullOrEmpty(userId, nameof(userId));

            UserManager<TUser> userManager = _provider.GetService<UserManager<TUser>>();
            TUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new OsharpException($"编号为“{userId}”的用户信息不存在。");
            }

            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            return claims;
        }

        /// <summary>
        /// 请求的Token验证成功后使用OnlineUser信息刷新Identity，将在线用户信息赋予Identity
        /// </summary>
        /// <param name="identity">待刷新的Identity</param>
        /// <returns>刷新后的Identity</returns>
        public async Task<OperationResult<ClaimsIdentity>> RefreshIdentity(ClaimsIdentity identity)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                IOnlineUserProvider onlineUserProvider = _provider.GetService<IOnlineUserProvider>();
                OnlineUser onlineUser = await onlineUserProvider.GetOrCreate(identity.Name);
                if (onlineUser == null)
                {
                    return new OperationResult<ClaimsIdentity>(OperationResultType.Error, "在线用户信息创建失败");
                }

                string clientId = identity.GetClaimValueFirstOrDefault("clientId");
                if (clientId != null && onlineUser.RefreshTokens.All(m => m.Value.ClientId != clientId))
                {
                    return new OperationResult<ClaimsIdentity>(OperationResultType.Error, "当前客户端的Token已过期");
                }

                if (!string.IsNullOrEmpty(onlineUser.NickName))
                {
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, onlineUser.NickName));
                }

                if (!string.IsNullOrEmpty(onlineUser.Email))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, onlineUser.Email));
                }

                if (onlineUser.Roles.Length > 0)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, onlineUser.Roles.ExpandAndToString()));
                }

                // 扩展数据
                foreach (KeyValuePair<string, string> pair in onlineUser.ExtendData)
                {
                    identity.AddClaim(new Claim(pair.Key, pair.Value));
                }
            }

            ScopedDictionary dict = _provider.GetService<ScopedDictionary>();
            if (dict != null)
            {
                dict.Identity = identity;
            }

            return new OperationResult<ClaimsIdentity>(OperationResultType.Success, "ok", identity);
        }
    }
}