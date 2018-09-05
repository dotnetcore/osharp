/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

using OSharp.Identity.OAuth2.Alipay;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Alipay authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AlipayAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="AlipayAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Alipay authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAlipay([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddAlipay(AlipayAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="AlipayAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Alipay authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAlipay(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<AlipayAuthenticationOptions> configuration)
        {
            return builder.AddAlipay(AlipayAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="AlipayAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Alipay authentication capabilities.
        /// </summary>
        /// <example>
        /// <code>
        /// .AddAlipay（options  =>
        /// {
        ///     options.ClientId = configuration["Authentication:Alipay:AppId"];
        ///     options.ClientSecret = configuration["Authentication:Alipay:MerchantPrivateKey"];
        ///     options.AlipayPublicKey = configuration["Authentication:Alipay:AlipayPublicKey"];
        /// }
        /// </code>
        /// </example>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Alipay options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAlipay(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<AlipayAuthenticationOptions> configuration)
        {
            return builder.AddAlipay(scheme, AlipayAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="AlipayAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Alipay authentication capabilities.
        /// </summary>
        /// <example>
        /// <code>
        /// .AddAlipay（options  =>
        /// {
        ///     options.ClientId = configuration["Authentication:Alipay:AppId"];
        ///     options.ClientSecret = configuration["Authentication:Alipay:MerchantPrivateKey"];
        ///     options.AlipayPublicKey = configuration["Authentication:Alipay:AlipayPublicKey"];
        /// }
        /// </code>
        /// </example>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Alipay options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAlipay(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<AlipayAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<AlipayAuthenticationOptions, AlipayAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}