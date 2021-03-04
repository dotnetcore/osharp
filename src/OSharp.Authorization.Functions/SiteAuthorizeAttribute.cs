// -----------------------------------------------------------------------
//  <copyright file="OsharpAuthorizeAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-04 14:30</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;


namespace OSharp.Authorization
{
    /// <summary>
    /// OSharp站点授权，应用<see cref="FunctionRequirement.OsharpPolicy"/>授权策略和<see cref="IdentityConstants.ApplicationScheme"/>+<see cref="JwtBearerDefaults.AuthenticationScheme"/>授权计划
    /// </summary>
    public class SiteAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 初始化一个<see cref="SiteAuthorizeAttribute"/>类型的新实例
        /// </summary>
        public SiteAuthorizeAttribute()
        {
            Policy = FunctionRequirement.OsharpPolicy;
            AuthenticationSchemes = $"{IdentityConstants.ApplicationScheme},{JwtBearerDefaults.AuthenticationScheme}";
        }
    }
}