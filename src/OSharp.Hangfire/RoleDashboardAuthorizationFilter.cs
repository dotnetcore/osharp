// -----------------------------------------------------------------------
//  <copyright file="RoleDashboardAuthorizationFilter.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-21 18:32</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Security.Claims;

using Hangfire.Dashboard;

using Microsoft.AspNetCore.Http;

using OSharp.Identity;


namespace OSharp.Hangfire
{
    /// <summary>
    /// Dashboard角色权限过滤器
    /// </summary>
    public class RoleDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string[] _roles;

        /// <summary>
        /// 初始化一个<see cref="RoleDashboardAuthorizationFilter"/>类型的新实例
        /// </summary>
        public RoleDashboardAuthorizationFilter(string[] roles)
        {
            _roles = roles;
        }

        public bool Authorize(DashboardContext context)
        {
            HttpContext httpContext = context.GetHttpContext();
            ClaimsPrincipal principal = httpContext.User;
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return false;
            }

            string[] roles = identity.GetRoles();
            return _roles.Intersect(roles).Any();
        }
    }
}