// -----------------------------------------------------------------------
//  <copyright file="OsharpCookieAuthenticationEvents.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-02 19:15</last-date>
// -----------------------------------------------------------------------

using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Authentication.Cookies
{
    /// <summary>
    /// 自定义 CookieAuthenticationEvents
    /// </summary>
    public class OsharpCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        /// <summary>
        /// Cookie验证通过时，从OnlineUser缓存或数据库查找用户的最新信息附加到有效的 ClaimIdentity 上
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            ClaimsPrincipal user = context.Principal;
            ClaimsIdentity identity = user.Identity as ClaimsIdentity;

            IUserClaimsProvider accessClaimsProvider = context.HttpContext.RequestServices.GetService<IUserClaimsProvider>();
            return accessClaimsProvider.RefreshIdentity(identity);
        }
    }
}