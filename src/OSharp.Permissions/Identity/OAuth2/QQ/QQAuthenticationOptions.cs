/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static OSharp.Identity.OAuth2.QQ.QQAuthenticationConstants;

namespace OSharp.Identity.OAuth2.QQ
{
    /// <summary>
    /// Defines a set of options used by <see cref="QQAuthenticationHandler"/>.
    /// </summary>
    public class QQAuthenticationOptions : OAuthOptions
    {
        public QQAuthenticationOptions()
        {
            ClaimsIssuer = QQAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(QQAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = QQAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQAuthenticationDefaults.TokenEndpoint;
            UserIdentificationEndpoint = QQAuthenticationDefaults.UserIdentificationEndpoint;
            UserInformationEndpoint = QQAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("get_user_info");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");

            ClaimActions.MapJsonKey(Claims.NickName, "nickname");
            ClaimActions.MapJsonKey(Claims.PictureUrl, "figureurl");
            ClaimActions.MapJsonKey(Claims.PictureMediumUrl, "figureurl_1");
            ClaimActions.MapJsonKey(Claims.PictureFullUrl, "figureurl_2");
            ClaimActions.MapJsonKey(Claims.AvatarUrl, "figureurl_qq_1");
            ClaimActions.MapJsonKey(Claims.AvatarFullUrl, "figureurl_qq_2");
            ClaimActions.MapJsonKey(Claims.IsYellowVip, "is_yellow_vip");
            ClaimActions.MapJsonKey(Claims.Vip, "vip");
            ClaimActions.MapJsonKey(Claims.YellowVipLevel, "yellow_vip_level");
            ClaimActions.MapJsonKey(Claims.Level, "level");
            ClaimActions.MapJsonKey(Claims.IsYellowYearVip, "is_yellow_year_vip");
        }

        /// <summary>
        /// Gets or sets the URL of the user identification endpoint (aka "OpenID endpoint").
        /// </summary>
        public string UserIdentificationEndpoint { get; set; }
    }
}