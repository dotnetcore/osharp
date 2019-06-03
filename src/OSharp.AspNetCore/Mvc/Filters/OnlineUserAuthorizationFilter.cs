// -----------------------------------------------------------------------
//  <copyright file="OnlineUserAuthorizationFilter.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-01 22:17</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Dependency;
using OSharp.Identity;


namespace OSharp.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 在线用户信息过滤器
    /// </summary>
    public class OnlineUserAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that on completion indicates the filter has executed.
        /// </returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ClaimsPrincipal principal = context.HttpContext.User;
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            IServiceProvider provider = context.HttpContext.RequestServices;
            if (identity != null && identity.IsAuthenticated)
            {
                // 由在线缓存获取用户信息赋给Identity
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

                foreach (KeyValuePair<string, string> pair in onlineUser.ExtendData)
                {
                    identity.AddClaim(new Claim(pair.Key, pair.Value));
                }
            }

            ScopedDictionary dict = provider.GetService<ScopedDictionary>();
            dict.Identity = identity;
        }
    }
}