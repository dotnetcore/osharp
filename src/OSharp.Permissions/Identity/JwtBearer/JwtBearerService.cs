// -----------------------------------------------------------------------
//  <copyright file="JwtBearerService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-15 13:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using OSharp.Core.Options;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Identity.Events;
using OSharp.Timing;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// JwtBearer服务实现
    /// </summary>
    public class JwtBearerService<TUser, TUserKey> : IJwtBearerService
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly IServiceProvider _provider;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        /// <summary>
        /// 初始化一个<see cref="JwtBearerService{TUser, TUserKey}"/>类型的新实例
        /// </summary>
        public JwtBearerService(IServiceProvider provider)
        {
            _provider = provider;
            _logger = provider.GetLogger(GetType());
            _jwtOptions = _provider.GetOSharpOptions().Jwt;
        }
        /// <summary>
        /// 创建指定用户的JwtToken信息
        /// </summary>
        /// <param name="userId">用户编号的字符串</param>
        /// <param name="userName">用户名的字符串</param>
        /// <param name="refreshToken">刷新Token模型</param>
        /// <returns>JwtToken信息</returns>
        public async Task<JsonWebToken> CreateToken(string userId, string userName, RefreshToken refreshToken = null)
        {
            Check.NotNullOrEmpty(userId, nameof(userId));
            Check.NotNullOrEmpty(userName, nameof(userName));

            // New RefreshToken
            string clientId = refreshToken?.ClientId ?? Guid.NewGuid().ToString();
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim("clientId", clientId)
            };
            var (token, expires) = CreateToken(claims, _jwtOptions, JwtTokenType.RefreshToken, refreshToken);
            string refreshTokenStr = token;
            await _provider.ExecuteScopedWorkAsync(async provider =>
                {
                    UserManager<TUser> userManager = provider.GetService<UserManager<TUser>>();
                    refreshToken = new RefreshToken() { ClientId = clientId, Value = refreshTokenStr, EndUtcTime = expires };
                    var result = await userManager.SetRefreshToken(userId, refreshToken);
                    if (result.Succeeded)
                    {
                        IUnitOfWork unitOfWork = provider.GetUnitOfWork<TUser, TUserKey>();
                        unitOfWork.Commit();
                        IEventBus eventBus = _provider.GetService<IEventBus>();
                        OnlineUserCacheRemoveEventData eventData = new OnlineUserCacheRemoveEventData() { UserNames = new[] { userName } };
                        eventBus.Publish(eventData);
                    }
                    return result;
                },
                false);

            // New AccessToken
            IAccessClaimsProvider claimsProvider = _provider.GetService<IAccessClaimsProvider>();
            claims = await claimsProvider.CreateClaims(userId);
            List<Claim> claimList = claims.ToList();
            claimList.Add(new Claim("clientId", clientId));
            (token, _) = CreateToken(claimList, _jwtOptions, JwtTokenType.AccessToken);

            return new JsonWebToken()
            {
                AccessToken = token,
                RefreshToken = refreshTokenStr,
                RefreshUctExpires = expires.ToJsGetTime().CastTo<long>(0)
            };
        }

        /// <summary>
        /// 使用RefreshToken获取新的JwtToken信息
        /// </summary>
        /// <param name="refreshToken">刷新Token字符串</param>
        /// <returns>JwtToken信息</returns>
        public virtual async Task<JsonWebToken> RefreshToken(string refreshToken)
        {
            Check.NotNull(refreshToken, nameof(refreshToken));
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                ValidIssuer = _jwtOptions.Issuer ?? "osharp identity",
                ValidAudience = _jwtOptions.Audience ?? "osharp client",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret))
            };
            JwtSecurityToken jwtSecurityToken = _tokenHandler.ReadJwtToken(refreshToken);
            string clientId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "clientId")?.Value;
            if (clientId == null)
            {
                throw new OsharpException("RefreshToken 中不包含 ClientId 声明");
            }
            string userId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "nameid")?.Value;
            if (userId == null)
            {
                throw new OsharpException("RefreshToken 的数据中无法找到 UserId 声明");
            }

            UserManager<TUser> userManager = _provider.GetService<UserManager<TUser>>();
            RefreshToken tokenModel = await userManager.GetRefreshToken(userId, clientId);
            if (tokenModel == null || tokenModel.Value != refreshToken || tokenModel.EndUtcTime <= DateTime.UtcNow)
            {
                if (tokenModel != null && tokenModel.EndUtcTime <= DateTime.UtcNow)
                {
                    //删除过期的Token
                    await _provider.ExecuteScopedWorkAsync(async provider =>
                        {
                            userManager = provider.GetService<UserManager<TUser>>();
                            var result = await userManager.RemoveRefreshToken(userId, clientId);
                            if (result.Succeeded)
                            {
                                IUnitOfWork unitOfWork = provider.GetUnitOfWork<TUser, TUserKey>();
                                unitOfWork.Commit();
                            }

                            return result;
                        },
                        false);
                }
                throw new OsharpException("RefreshToken 不存在或已过期");
            }

            ClaimsPrincipal principal = _tokenHandler.ValidateToken(refreshToken, parameters, out SecurityToken securityToken);

            string userName = principal.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                throw new OsharpException("RefreshToken 的数据中无法找到 UserName 声明");
            }

            JsonWebToken token = await CreateToken(userId, userName, tokenModel);
            return token;
        }

        private (string, DateTime) CreateToken(IEnumerable<Claim> claims, JwtOptions options, JwtTokenType tokenType, RefreshToken refreshToken = null)
        {
            string secret = options.Secret;
            if (secret == null)
            {
                throw new OsharpException("创建JwtToken时Secret为空，请在OSharp:Jwt:Secret节点中进行配置");
            }

            DateTime expires;
            DateTime now = DateTime.UtcNow;
            if (tokenType == JwtTokenType.AccessToken)
            {
                if (refreshToken != null)
                {
                    throw new OsharpException("创建 AccessToken 时不需要 refreshToken");
                }

                double minutes = options.AccessExpireMins > 0 ? options.AccessExpireMins : 5; //默认5分钟
                expires = now.AddMinutes(minutes);
            }
            else
            {
                if (refreshToken == null)
                {
                    double minutes = options.RefreshExpireMins > 0 ? options.RefreshExpireMins : 10080; // 默认7天
                    expires = now.AddMinutes(minutes);
                }
                else
                {
                    expires = refreshToken.EndUtcTime;
                }
            }
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Audience = options.Audience,
                Issuer = options.Issuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = expires
            };
            SecurityToken token = _tokenHandler.CreateToken(descriptor);
            string accessToken = _tokenHandler.WriteToken(token);
            return (accessToken, expires);
        }

        private enum JwtTokenType
        {
            AccessToken,

            RefreshToken
        }
    }
}