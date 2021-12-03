// -----------------------------------------------------------------------
//  <copyright file="CustomAuthorizationMiddlewareResultHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-12-15 2:58</last-date>
// -----------------------------------------------------------------------

#if NET5_0
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;


namespace OSharp.Authorization
{
    /// <summary>
    /// 自定义授权中间件结果处理器
    /// </summary>
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        /// <summary>
        /// Evaluates the authorization requirement and processes the authorization result.
        /// </summary>
        /// <param name="next">
        /// The next middleware in the application pipeline. Implementations may not invoke this if the authorization did not succeed.
        /// </param>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" />.</param>
        /// <param name="policy">The <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> for the resource.</param>
        /// <param name="authorizeResult">The result of authorization.</param>
        public async Task HandleAsync(RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            throw new NotImplementedException();
        }
    }
}


#endif