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
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using OSharp.Core.Options;
using OSharp.Exceptions;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// JwtBearer服务实现
    /// </summary>
    public class JwtBearerService : IJwtBearerService
    {
        private readonly IServiceProvider _provider;
        private static readonly JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        /// <summary>
        /// 初始化一个<see cref="JwtBearerService"/>类型的新实例
        /// </summary>
        public JwtBearerService(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="claims">要加入Token的声明信息</param>
        /// <returns></returns>
        public string CreateRefreshToken(IList<Claim> claims)
        {
            string clientId = Guid.NewGuid().ToString();
            claims.Add(new Claim("clientId", clientId));
            OsharpOptions options = _provider.GetOSharpOptions();
            string token = CreateToken(claims, options.Jwt, JwtTokenType.RefreshToken);






            return token;
        }

        /// <summary>
        /// 使用RefreshToken创建AccessToken
        /// </summary>
        /// <param name="claims">要加入Token的声明信息</param>
        /// <param name="refreshToken">刷新Token</param>
        /// <returns></returns>
        public string CreateAccessToken(IList<Claim> claims, string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            ClaimsPrincipal principal = ValidateToken(refreshToken);
            if (principal == null)
            {
                return null;
            }

            OsharpOptions options = _provider.GetOSharpOptions();
            return CreateToken(claims, options.Jwt, JwtTokenType.AccessToken);
        }

        /// <summary>
        /// 验证指定的Token有效性
        /// </summary>
        /// <param name="token">指定Token</param>
        /// <returns></returns>
        public ClaimsPrincipal ValidateToken(string token)
        {
            if (token == null)
            {
                return null;
            }

            try
            {
                JwtBearerHandler jwtBearerHandler = _provider.GetService<JwtBearerHandler>();
                TokenValidationParameters parameters = jwtBearerHandler.Options.TokenValidationParameters.Clone();
                ClaimsPrincipal principal = TokenHandler.ValidateToken(token, parameters, out SecurityToken t);
                return principal;
            }
            catch (Exception ex)
            {
                ILogger logger = _provider.GetLogger<JwtBearerService>();
                logger.LogError(ex, $"RefreshToken验证失败：{ex.Message}");
                return null;
            }
        }

        private static string CreateToken(IEnumerable<Claim> claims, JwtOptions options, JwtTokenType tokenType)
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
            SecurityToken token = TokenHandler.CreateToken(descriptor);
            return TokenHandler.WriteToken(token);
        }
    }
}