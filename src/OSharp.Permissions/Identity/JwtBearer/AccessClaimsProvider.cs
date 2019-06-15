// -----------------------------------------------------------------------
//  <copyright file="AccessClaimsProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-15 12:19</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Data;
using OSharp.Exceptions;


namespace OSharp.Identity.JwtBearer
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
    }
}