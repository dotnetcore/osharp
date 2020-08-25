// -----------------------------------------------------------------------
//  <copyright file="ServiceProviderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-28 14:54</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Identity
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 获取在<see cref="OnlineUser"/>线用户信息
        /// </summary>
        public static async Task<OnlineUser> GetOnlineUser(this IServiceProvider provider)
        {
            ClaimsPrincipal principal = provider.GetService<IPrincipal>() as ClaimsPrincipal;
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }

            string userName = principal.Identity.Name;
            IOnlineUserProvider onlineUserProvider = provider.GetService<IOnlineUserProvider>();
            if (onlineUserProvider == null)
            {
                return null;
            }

            OnlineUser onlineUser = await onlineUserProvider.GetOrCreate(userName);
            return onlineUser;
        }
    }
}