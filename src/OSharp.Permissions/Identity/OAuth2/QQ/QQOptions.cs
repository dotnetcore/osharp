// -----------------------------------------------------------------------
//  <copyright file="QQOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-21 16:58</last-date>
// -----------------------------------------------------------------------

using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;


namespace OSharp.Identity.OAuth2.QQ
{
    /// <summary>
    /// QQ身份认证选项
    /// </summary>
    public class QQOptions : OAuthOptions
    {
        /// <summary>
        /// 初始化一个<see cref="QQOptions"/>类型的新实例
        /// </summary>
        public QQOptions()
        {
            CallbackPath = new PathString("/signin-qq");
            AuthorizationEndpoint = QQDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQDefaults.TokenEndpoint;
            UserInformationEndpoint = QQDefaults.UserInformationEndpoint;
            OpenIdEndpoint = QQDefaults.OpenIdEndpoint;

            //StateDataFormat = 
            //Scope 表示用户授权时向用户显示的可进行授权的列表。
            Scope.Add("get_user_info"); //默认只请求对get_user_info进行授权

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey("urn:qq:figure", "figureurl_qq_1");
        }

        /// <summary>
        /// OpenId请求的终结点
        /// </summary>
        public string OpenIdEndpoint { get; }

        /// <summary>
        /// QQ互联 APP ID https://connect.qq.com
        /// </summary>
        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        /// <summary>
        /// QQ互联 APP Key https://connect.qq.com
        /// </summary>
        public string AppKey
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }
    }
}