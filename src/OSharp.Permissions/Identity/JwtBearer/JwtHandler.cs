// -----------------------------------------------------------------------
//  <copyright file="JwtHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-26 0:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// Jwt辅助操作类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 生成JwtToken
        /// </summary>
        public static string CreateToken(Claim[] claims, OsharpOptions options)
        {
            JwtOptions jwtOptions = options.Jwt;
            string secret = jwtOptions.Secret;
            if (secret == null)
            {
                throw new OsharpException("创建JwtToken时Secret为空");
            }
            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            DateTime now = DateTime.UtcNow;
            double days = Math.Abs(jwtOptions.AccessExpireMins) > 0 ? Math.Abs(jwtOptions.AccessExpireMins) : 7;
            DateTime expires = now.AddDays(days);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = expires
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}