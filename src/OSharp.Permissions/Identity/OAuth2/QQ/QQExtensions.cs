// -----------------------------------------------------------------------
//  <copyright file="QQExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-21 17:10</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Authentication;

using OSharp.Identity.OAuth2.QQ;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// QQ身份认证扩展方法
    /// </summary>
    public static class QQExtensions
    {
        /// <summary>
        /// 添加QQ身份认证
        /// </summary>
        /// <param name="builder">身份认证构建器</param>
        /// <returns>身份认证构建器</returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder)
            => builder.AddQQ(QQDefaults.AuthenticationScheme,
                _ =>
                { });

        /// <summary>
        /// 添加QQ身份认证
        /// </summary>
        /// <param name="builder">身份认证构建器</param>
        /// <param name="configureOptions">QQ身份认证选项</param>
        /// <returns>身份认证构建器</returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder, Action<QQOptions> configureOptions)
            => builder.AddQQ(QQDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// 添加QQ身份认证
        /// </summary>
        /// <param name="builder">身份认证构建器</param>
        /// <param name="authenticationScheme">QQ身份认证标识</param>
        /// <param name="configureOptions">QQ身份认证选项</param>
        /// <returns>身份认证构建器</returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder, string authenticationScheme, Action<QQOptions> configureOptions)
            => builder.AddQQ(authenticationScheme, QQDefaults.DisplayName, configureOptions);

        /// <summary>
        /// 添加QQ身份认证
        /// </summary>
        /// <param name="builder">身份认证构建器</param>
        /// <param name="authenticationScheme">QQ身份认证标识</param>
        /// <param name="displayName">QQ身份认证显示名称</param>
        /// <param name="configureOptions">QQ身份认证选项</param>
        /// <returns>身份认证构建器</returns>
        public static AuthenticationBuilder AddQQ(this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<QQOptions> configureOptions)
            => builder.AddOAuth<QQOptions, QQHandler>(authenticationScheme, displayName, configureOptions);
    }
}