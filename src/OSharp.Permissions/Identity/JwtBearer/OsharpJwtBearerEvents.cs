// -----------------------------------------------------------------------
//  <copyright file="OsharpJwtBearerEvents.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-15 0:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using OSharp.Collections;
using OSharp.Dependency;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// 自定义JwtBearerEvents
    /// </summary>
    public class OsharpJwtBearerEvents : JwtBearerEvents
    {
        /// <summary>
        /// Token验证通过时，从OnlineUser缓存或数据库查找用户的最新信息附加到有效的 ClaimPrincipal 上
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task TokenValidated(TokenValidatedContext context)
        {
            ClaimsPrincipal user = context.Principal;
            ClaimsIdentity identity = user.Identity as ClaimsIdentity;
            IServiceProvider provider = context.HttpContext.RequestServices;
            if (identity != null && identity.IsAuthenticated)
            {
                // 由在线缓存获取用户信息并赋到 Identity
                IOnlineUserProvider onlineUserProvider = provider.GetService<IOnlineUserProvider>();
                OnlineUser onlineUser = await onlineUserProvider.GetOrCreate(identity.Name);
                if (onlineUser == null)
                {
                    return;
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

                ScopedDictionary dict = provider.GetService<ScopedDictionary>();
                dict.Identity = identity;
            }
        }

        /// <summary>
        /// 在接收消息时触发，这里定义接收SignalR的token的逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task MessageReceived(MessageReceivedContext context)
        {
            string token = context.Request.Query["access_token"];
            string path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(token) && path.Contains("hub"))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }

            return Task.CompletedTask;
        }
    }
}