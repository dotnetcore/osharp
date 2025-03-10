using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Liuliu.Demo.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Caching;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Identity;
using OSharp.Identity.Entities;
using OSharp.Identity.JwtBearer;
using OSharp.Threading.Asyncs;

namespace Liuliu.Demo.MultiTenancy
{
    /// <summary>
    /// 在线用户信息提供者
    /// </summary>
    public class TenantOnlineUserProvider : IOnlineUserProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedCache _cache;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly AsyncLock _asyncLock = new AsyncLock();

        /// <summary>
        /// 初始化一个<see cref="TenantOnlineUserProvider"/>类型的新实例
        /// </summary>
        public TenantOnlineUserProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cache = serviceProvider.GetService<IDistributedCache>();
            _tenantAccessor = serviceProvider.GetService<ITenantAccessor>();
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
                        UserManager<User> userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
                        User user = await userManager.FindByNameAsync(userName);
                        if (user == null)
                        {
                            return null;
                        }

                        IList<string> roles = await userManager.GetRolesAsync(user);
                        RoleManager<Role> roleManager = _serviceProvider.GetRequiredService<RoleManager<Role>>();
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
        private async Task<RefreshToken[]> GetRefreshTokens(User user)
        {
            IOsharpUserAuthenticationTokenStore<User> store =
                _serviceProvider.GetService<IUserStore<User>>() as IOsharpUserAuthenticationTokenStore<User>;
            if (store == null)
            {
                return Array.Empty<RefreshToken>();
            }
            const string loginProvider = "JwtBearer";
            string[] jsons = await store.GetTokensAsync(user, loginProvider, CancellationToken.None);
            if (jsons.Length == 0)
            {
                return Array.Empty<RefreshToken>();
            }

            RefreshToken[] tokens = jsons.Select(m => m.FromJsonString<RefreshToken>()).ToArray();
            RefreshToken[] expiredTokens = tokens.Where(m => m.EndUtcTime < DateTime.UtcNow).ToArray();
            if (expiredTokens.Length <= 0)
            {
                return tokens;
            }

            //删除过期的Token
            UserManager<User> userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            IUnitOfWork unitOfWork = _serviceProvider.GetUnitOfWork(true);
            foreach (RefreshToken token in expiredTokens)
            {
                await userManager.RemoveRefreshToken(user, token.ClientId);
            }
            await unitOfWork.CommitAsync();

            return tokens.Except(expiredTokens).ToArray();
        }

        private string GetKey(string userName)
        {
            var tenantCacheKeyPre = _tenantAccessor.TenantCacheKeyPre;

            return $"{tenantCacheKeyPre}Identity:OnlineUser:{userName}";
        }
    }
}
