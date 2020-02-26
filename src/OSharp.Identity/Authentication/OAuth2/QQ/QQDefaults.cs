// -----------------------------------------------------------------------
//  <copyright file="QQDefaults.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-21 17:00</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Identity.OAuth2.QQ
{
    //http://wiki.connect.qq.com/%E5%87%86%E5%A4%87%E5%B7%A5%E4%BD%9C_oauth2-0 QQ互联 oauth2.0文档
    /// <summary>
    /// QQ认证使用的默认值。
    /// </summary>
    public static class QQDefaults
    {
        /// <summary>
        /// 权限标记
        /// </summary>
        public const string AuthenticationScheme = "QQ";

        /// <summary>
        /// 显示名称
        /// </summary>
        public static readonly string DisplayName = "QQ";

        /// <summary>
        /// 获取Authorization Code
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";

        /// <summary>
        /// 通过Authorization Code获取Access Token
        /// </summary>
        public static readonly string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        /// <summary>
        ///通过获取的Access Token，得到对应用户身份的OpenID
        /// </summary>
        public static readonly string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";

        /// <summary>
        ///获取到Access Token和OpenID后，可通过调用OpenAPI来获取或修改用户个人信息
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://graph.qq.com/user/get_user_info";
    }
}