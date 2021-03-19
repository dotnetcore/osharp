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
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Caching;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Identity.Entities;
using OSharp.Identity.JwtBearer;
using OSharp.Threading.Asyncs;


namespace OSharp.Identity
{
    /// <summary>
    /// 在线用户信息提供者
    /// </summary>
    public class OnlineUserProvider<TUser, TUserKey, TUserClaim, TUserClaimKey, TRole, TRoleKey> : IOnlineUserProvider
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
        where TUserClaim : UserClaimBase<TUserClaimKey, TUserKey>
        where TUserClaimKey : IEquatable<TUserClaimKey>
        where TRole : RoleBase<TRoleKey>
        where TRoleKey : IEquatable<TRoleKey>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedCache _cache;
        private readonly AsyncLock _asyncLock = new AsyncLock();

        /// <summary>
        /// 初始化一个<see cref="OnlineUserProvider{TUser, TUserKey, TUserClaim, TUserClaimKey, TRole, TRoleKey}"/>类型的新实例
        /// </summary>
        public OnlineUserProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cache = serviceProvider.GetService<IDistributedCache>();
        }

        /// <summary>
        /// 获取或创建在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public virtual async Task<OnlineUser> GetOrCreate(string userName)
        {
            string key = GetKey(userName);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            using (await _asyncLock.LockAsync())
            {
                return await _cache.GetAsync<OnlineUser>(key,
                    async () =>
                    {
                        UserManager<TUser> userManager = _serviceProvider.GetService<UserManager<TUser>>();
                        TUser user = await userManager.FindByNameAsync(userName);
                        if (user == null)
                        {
                            return null;
                        }

                        IList<string> roles = await userManager.GetRolesAsync(user);
                        RoleManager<TRole> roleManager = _serviceProvider.GetService<RoleManager<TRole>>();
                        bool isAdmin = roleManager.Roles.ToList().Any(m => roles.Contains(m.Name) && m.IsAdmin);
                        RefreshToken[] refreshTokens = await GetRefreshTokens(user);
                        OnlineUser onlineUser = new OnlineUser()
                        {
                            Id = user.Id.ToString(),
                            UserName = user.UserName,
                            NickName = user.NickName,
                            Email = user.Email,
                            HeadImg = user.HeadImg,
                            IsAdmin = isAdmin,
                            Roles = roles.ToArray(),
                            RefreshTokens = refreshTokens.ToDictionary(m => m.ClientId, m => m)
                        };

                        // UserClaim都添加到扩展数据
                        IList<Claim> claims = await userManager.GetClaimsAsync(user);
                        foreach (Claim claim in claims)
                        {
                            onlineUser.ExtendData.Add(claim.Type, claim.Value);
                        }

                        return onlineUser;
                    },
                    options);
            }
        }

        /// <summary>
        /// 移除在线用户信息
        /// </summary>
        /// <param name="userNames">用户名</param>
        public virtual void Remove(params string[] userNames)
        {
            Check.NotNull(userNames, nameof(userNames));
            foreach (string userName in userNames)
            {
                string key = GetKey(userName);
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 获取指定用户所有刷新Token，并清除过期Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<RefreshToken[]> GetRefreshTokens(TUser user)
        {
            IOsharpUserAuthenticationTokenStore<TUser> store =
                _serviceProvider.GetService<IUserStore<TUser>>() as IOsharpUserAuthenticationTokenStore<TUser>;
            if (store == null)
            {
                return new RefreshToken[0];
            }
            const string loginProvider = "JwtBearer";
            string[] jsons = await store.GetTokensAsync(user, loginProvider, CancellationToken.None);
            if (jsons.Length == 0)
            {
                return new RefreshToken[0];
            }

            RefreshToken[] tokens = jsons.Select(m => m.FromJsonString<RefreshToken>()).ToArray();
            RefreshToken[] expiredTokens = tokens.Where(m => m.EndUtcTime < DateTime.UtcNow).ToArray();
            if (expiredTokens.Length <= 0)
            {
                return tokens;
            }

            //删除过期的Token
            using (var scope = _serviceProvider.CreateScope())
            {
                IServiceProvider scopedProvider = scope.ServiceProvider;
                UserManager<TUser> userManager = scopedProvider.GetService<UserManager<TUser>>();
                IUnitOfWork unitOfWork = scopedProvider.GetUnitOfWork(true);
                foreach (RefreshToken expiredToken in expiredTokens)
                {
                    await userManager.RemoveRefreshToken(user, expiredToken.ClientId);
                }

#if NET5_0
                await unitOfWork.CommitAsync();
#else
                unitOfWork.Commit();
#endif
            }

            return tokens.Except(expiredTokens).ToArray();
        }
        
        private static string GetKey(string userName)
        {
            return $"Identity:OnlineUser:{userName}";
        }
    }
}