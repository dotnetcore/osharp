/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static OSharp.Identity.OAuth2.Baidu.BaiduAuthenticationConstants;

namespace OSharp.Identity.OAuth2.Baidu
{
    /// <summary>
    /// Defines a set of options used by <see cref="BaiduAuthenticationHandler"/>.
    /// </summary>
    public class BaiduAuthenticationOptions : OAuthOptions
    {
        public BaiduAuthenticationOptions()
        {
            ClaimsIssuer = BaiduAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(BaiduAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = BaiduAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BaiduAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BaiduAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("basic");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            //"1"表示男，"0"表示女。
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex", ClaimValueTypes.Integer);

            ClaimActions.MapJsonKey(Claims.UserId, "userid");
            ClaimActions.MapJsonKey(Claims.UserName, "username");
            ClaimActions.MapJsonKey(Claims.RealName, "realname");
            ClaimActions.MapJsonKey(Claims.Portrait, "portrait");
            ClaimActions.MapJsonKey(Claims.UserDetail, "userdetail");
            ClaimActions.MapJsonKey(Claims.Birthday, "birthday");
            ClaimActions.MapJsonKey(Claims.Marriage, "marriage");
            ClaimActions.MapJsonKey(Claims.Blood, "blood");
            ClaimActions.MapJsonKey(Claims.Figure, "figure");
            ClaimActions.MapJsonKey(Claims.Constellation, "constellation");
            ClaimActions.MapJsonKey(Claims.Education, "education");
            ClaimActions.MapJsonKey(Claims.Trade, "trade");
            ClaimActions.MapJsonKey(Claims.Job, "job");
        }
    }
}