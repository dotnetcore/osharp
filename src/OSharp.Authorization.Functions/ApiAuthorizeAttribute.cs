// -----------------------------------------------------------------------
//  <copyright file="OsharpApiAuthorizeAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-04 14:27</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace OSharp.Authorization
{
    /// <summary>
    /// OSharp Api授权，应用<see cref="FunctionRequirement.OsharpPolicy"/>授权策略和<see cref="JwtBearerDefaults.AuthenticationScheme"/>授权计划
    /// </summary>
    public class ApiAuthorizeAttribute : SiteAuthorizeAttribute
    {
        /// <summary>
        /// 初始化一个<see cref="ApiAuthorizeAttribute"/>类型的新实例
        /// </summary>
        public ApiAuthorizeAttribute()
        {
            Policy = FunctionRequirement.OsharpPolicy;
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}