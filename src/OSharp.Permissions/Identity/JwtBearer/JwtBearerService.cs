// -----------------------------------------------------------------------
//  <copyright file="JwtBearerService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 1:35</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// JwtBearer服务实现
    /// </summary>
    public class JwtBearerService<TUser, TUserKey> : IJwtBearerService
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
    {
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// 初始化一个<see cref="JwtBearerService{TUser, TUserKey}"/>类型的新实例
        /// </summary>
        public JwtBearerService(IServiceProvider provider)
        {
            _provider = provider;
            _logger = provider.GetLogger(GetType());
            _jwtOptions = _provider.GetOSharpOptions()?.Jwt;
        }

        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="claims">要加入Token的声明信息</param>
        /// <returns></returns>
        protected virtual RefreshToken CreateRefreshToken(Claim[] claims)
        {
            string clientId = Guid.NewGuid().ToString();
            List<Claim> newClaims = claims.ToList();
            newClaims.Add(new Claim("clientId", clientId));
            string token = CreateToken(newClaims, _jwtOptions, JwtTokenType.RefreshToken);
            //保存RefreshToken
            JwtSecurityToken jwtToken = _tokenHandler.ReadJwtToken(token);
            RefreshToken refreshToken = new RefreshToken() { ClientId = clientId, EndUtcTime = jwtToken.Payload.ValidTo, Value = token };
            return refreshToken;
        }

        /// <summary>
        /// 创建指定用户的AccessToken
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public async Task<string> CreateAccessToken(string userId)
        {
            Check.NotNullOrEmpty(userId, nameof(userId));
            using (IServiceScope scope = _provider.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;
                UserManager<TUser> userManager = provider.GetService<UserManager<TUser>>();
                TUser user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }

                IJwtClaimsProvider<TUser> jwtClaimsProvider = provider.GetService<IJwtClaimsProvider<TUser>>();
                Claim[] claims = await jwtClaimsProvider.Create(user);
                RefreshToken refreshToken = CreateRefreshToken(claims);

                List<Claim> claimList = claims.ToList();
                claimList.Add(new Claim("clientId", refreshToken.ClientId));
                string accessToken = CreateToken(claimList, _jwtOptions, JwtTokenType.AccessToken);

                IdentityResult identityResult = await userManager.SetRefreshToken(user, refreshToken);
                if (!identityResult.Succeeded)
                {
                    return null;
                }

                IUnitOfWork unitOfWork = provider.GetUnitOfWork<TUser, TUserKey>();
                unitOfWork.Commit();

                return accessToken;
            }
        }

        /// <summary>
        /// 使用AccessToken刷新一个新的AccessToken，如果AccessToken有效，原样返回，否则返回一个新的
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <returns>AccessToken</returns>
        public async Task<string> RefreshAccessToken(string accessToken)
        {
            Check.NotNull(accessToken, nameof(accessToken));
            (bool validated, IEnumerable<Claim> claims) = ValidateToken(accessToken);
            if (validated)
            {
                return accessToken;
            }

            // AccessToken 已失效，查找相应的 RefreshToken ，如果RefreshToken有效，则创建一新新的AccessToken
            List<Claim> claimList = claims.ToList();
            string userId = claimList.FirstOrDefault(m => m.Type == "nameid")?.Value;
            string clientId = claimList.FirstOrDefault(m => m.Type == "clientId")?.Value;
            UserManager<TUser> userManager = _provider.GetService<UserManager<TUser>>();
            TUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            RefreshToken refresh = await userManager.GetRefreshToken(user, clientId);
            if (refresh == null || refresh.EndUtcTime < DateTime.UtcNow)
            {
                return null;
            }

            IJwtClaimsProvider<TUser> jwtClaimsProvider = _provider.GetService<IJwtClaimsProvider<TUser>>();
            claimList = (await jwtClaimsProvider.Create(user)).ToList();
            claimList.Add(new Claim("clientId", clientId));

            accessToken = CreateToken(claimList, _jwtOptions, JwtTokenType.AccessToken);
            return accessToken;
        }

        /// <summary>
        /// 验证指定的Token有效性
        /// </summary>
        /// <param name="token">指定Token</param>
        /// <returns></returns>
        private (bool, IEnumerable<Claim>) ValidateToken(string token)
        {
            (bool, IEnumerable<Claim>) result = (false, null);
            if (token == null)
            {
                return result;
            }

            JwtSecurityToken jwtToken = _tokenHandler.ReadJwtToken(token);
            if (jwtToken == null)
            {
                return result;
            }

            return (jwtToken.Payload.ValidTo > DateTime.UtcNow, jwtToken.Claims);
        }

        private string CreateToken(IEnumerable<Claim> claims, JwtOptions options, JwtTokenType tokenType)
        {
            string secret = options.Secret;
            if (secret == null)
            {
                throw new OsharpException("创建JwtToken时Secret为空，请在OSharp:Jwt:Secret节点中进行配置");
            }
            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            DateTime now = DateTime.UtcNow;
            double minutes = tokenType == JwtTokenType.AccessToken
                ? Math.Abs(options.AccessExpireMins) > 0 ? Math.Abs(options.AccessExpireMins) : 5 :
                Math.Abs(options.RefreshExpireMins) > 0 ? Math.Abs(options.RefreshExpireMins) : 604800;
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Audience = options.Audience,
                Issuer = options.Issuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = now.AddMinutes(minutes)
            };
            SecurityToken token = _tokenHandler.CreateToken(descriptor);
            return _tokenHandler.WriteToken(token);
        }
    }
}