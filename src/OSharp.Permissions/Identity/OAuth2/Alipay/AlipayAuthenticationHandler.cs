/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Request;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OSharp.Identity.OAuth2.Alipay
{
    public class AlipayAuthenticationHandler : OAuthHandler<AlipayAuthenticationOptions>
    {
        private DefaultAopClient _alipayClient;

        public AlipayAuthenticationHandler(
            [NotNull] IOptionsMonitor<AlipayAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="properties"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity, [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var requestUser = new AlipayUserInfoShareRequest();
            var userinfoShareResponse = _alipayClient.Execute(requestUser, tokens.AccessToken);
            if (userinfoShareResponse.IsError)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                    /* Status: */ userinfoShareResponse.Code,
                    /* Headers: */ userinfoShareResponse.Msg,
                    /* Body: */ userinfoShareResponse.Body);

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }
            else
            {
                var payload = JObject.FromObject(userinfoShareResponse);
                var principal = new ClaimsPrincipal(identity);
                var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
                context.RunClaimActions(payload);
                await Options.Events.CreatingTicket(context);
                return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            }
        }

        /// <summary>
        /// 使用auth_code换取access_token及用户userId
        /// </summary>
        /// <param name="code"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] string code, [NotNull] string redirectUri)
        {
            try
            {
                var alipayRequest = new AlipaySystemOauthTokenRequest
                {
                    Code = code,
                    GrantType = "authorization_code"
                    //GetApiName()
                };

                var alipayResponse = await _alipayClient.ExecuteAsync(alipayRequest);
                if (alipayResponse.IsError)
                {
                    Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ alipayResponse.Code,
                                /* Headers: */ alipayResponse.Msg,
                                /* Body: */ alipayResponse.Body);
                    return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
                }
                else
                {
                    var payload = JObject.FromObject(alipayResponse);
                    return OAuthTokenResponse.Success(payload);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }
        }

        /// <summary>
        /// 拼接授权页面url
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["app_id"] = Options.ClientId,
                ["scope"] = FormatScope(),
                ["redirect_uri"] = redirectUri,
                ["state"] = Options.StateDataFormat.Protect(properties)
            });
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var query = Request.Query;

            var error = query["error"];
            if (!StringValues.IsNullOrEmpty(error))
            {
                var failureMessage = new StringBuilder();
                failureMessage.Append(error);
                var errorDescription = query["error_description"];
                if (!StringValues.IsNullOrEmpty(errorDescription))
                {
                    failureMessage.Append(";Description=").Append(errorDescription);
                }
                var errorUri = query["error_uri"];
                if (!StringValues.IsNullOrEmpty(errorUri))
                {
                    failureMessage.Append(";Uri=").Append(errorUri);
                }

                return HandleRequestResult.Fail(failureMessage.ToString());
            }

            var code = query["auth_code"];
            var state = query["state"];

            AuthenticationProperties properties = Options.StateDataFormat.Unprotect(state);
            if (properties == null)
            {
                return HandleRequestResult.Fail("The oauth state was missing or invalid.");
            }

            // OAuth2 10.12 CSRF
            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.");
            }

            if (StringValues.IsNullOrEmpty(code))
            {
                return HandleRequestResult.Fail("Code was not found.");
            }

            var tokens = await ExchangeCodeAsync(code, BuildRedirectUri(Options.CallbackPath));

            if (tokens.Error != null)
            {
                return HandleRequestResult.Fail(tokens.Error);
            }

            if (string.IsNullOrEmpty(tokens.AccessToken))
            {
                return HandleRequestResult.Fail("Failed to retrieve access token.");
            }

            var identity = new ClaimsIdentity(ClaimsIssuer);

            if (Options.SaveTokens)
            {
                var authTokens = new List<AuthenticationToken>
                {
                    new AuthenticationToken { Name = "access_token", Value = tokens.AccessToken }
                };
                if (!string.IsNullOrEmpty(tokens.RefreshToken))
                {
                    authTokens.Add(new AuthenticationToken { Name = "refresh_token", Value = tokens.RefreshToken });
                }

                if (!string.IsNullOrEmpty(tokens.TokenType))
                {
                    authTokens.Add(new AuthenticationToken { Name = "token_type", Value = tokens.TokenType });
                }

                if (!string.IsNullOrEmpty(tokens.ExpiresIn))
                {
                    if (int.TryParse(tokens.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                    {
                        // https://www.w3.org/TR/xmlschema-2/#dateTime
                        // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
                        var expiresAt = Clock.UtcNow + TimeSpan.FromSeconds(value);
                        authTokens.Add(new AuthenticationToken
                        {
                            Name = "expires_at",
                            Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                        });
                    }
                }

                properties.StoreTokens(authTokens);
            }

            var ticket = await CreateTicketAsync(identity, properties, tokens);
            return ticket != null
                ? HandleRequestResult.Success(ticket)
                : HandleRequestResult.Fail("Failed to retrieve user information from remote server.");
        }

        protected override Task InitializeHandlerAsync()
        {
            if (_alipayClient == null)
            {
                _alipayClient = new DefaultAopClient(Options.GatewayUrl,
                    Options.ClientId,
                    Options.ClientSecret,
                    Options.Format,
                    Options.Version,
                    Options.SignType,
                    Options.AlipayPublicKey,
                    Options.CharSet,
                    Options.IsKeyFromFile);
            }
            return Task.CompletedTask;
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);
    }
}