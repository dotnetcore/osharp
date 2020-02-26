// -----------------------------------------------------------------------
//  <copyright file="OsharpCookieAuthenticationEvents.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-16 19:00</last-date>
// -----------------------------------------------------------------------

using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Authentication.JwtBearer;


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

            IAccessClaimsProvider accessClaimsProvider = context.HttpContext.RequestServices.GetService<IAccessClaimsProvider>();
            return accessClaimsProvider.RefreshIdentity(identity);
        }
    }
}