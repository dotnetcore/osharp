// -----------------------------------------------------------------------
//  <copyright file="Extensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-19 10:33</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Security.JwtBearer
{
    /// <summary>
    /// JwtBearer相关扩展方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 应用OSharp-JWT身份认证
        /// </summary>
        public static IApplicationBuilder UseOsharpJwt(this IApplicationBuilder app)
        {
            IServiceProvider provider = app.ApplicationServices;
            IConfiguration configuration = provider.GetService<IConfiguration>();




            return app;
        }
    }
}