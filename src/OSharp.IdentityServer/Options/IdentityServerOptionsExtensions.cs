// -----------------------------------------------------------------------
//  <copyright file="Extensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-21 20:12</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;


namespace OSharp.IdentityServer.Options
{
    public static class IdentityServerOptionsExtensions
    {
        /// <summary>
        /// 获取<see cref="IdentityServerOptions"/>
        /// </summary>
        public static IdentityServerOptions GetIdentityServerOptions(this IConfiguration configuration)
        {
            return configuration?.GetInstance("OSharp:IdentityServer", new IdentityServerOptions());
        }

        /// <summary>
        /// 获取<see cref="IdentityServerOptions"/>
        /// </summary>
        public static IdentityServerOptions GetIdentityServerOptions(this IServiceProvider provider)
        {
            return provider.GetService<IConfiguration>()?.GetIdentityServerOptions();
        }
    }
}