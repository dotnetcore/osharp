// -----------------------------------------------------------------------
//  <copyright file="FunctionRequirement.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-11 14:07</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;


namespace OSharp.Authorization
{
    /// <summary>
    /// 功能点授权需求
    /// </summary>
    public class FunctionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Osharp授权策略名称
        /// </summary>
        public const string OsharpPolicy = "OsharpPolicy";
    }
}