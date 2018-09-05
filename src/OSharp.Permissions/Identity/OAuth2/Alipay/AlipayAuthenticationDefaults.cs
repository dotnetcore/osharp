/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace OSharp.Identity.OAuth2.Alipay
{
    /// <summary>
    /// Default values for Alipay authentication.
    /// </summary>
    public static class AlipayAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Alipay";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Alipay";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-alipay";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Alipay";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        //public const string AuthorizationEndpoint = "https://openauth.alipaydev.com/oauth2/publicAppAuthorize.htm";//沙箱
        public const string AuthorizationEndpoint = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "alipay.system.oauth.token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "alipay.user.userinfo.share";

        //alipay.user.info.auth(用户登陆授权)
        //alipay.system.oauth.token(换取授权访问令牌)
        //alipay.user.userinfo.share(支付宝钱包用户信息共享)

        /// <summary>
        /// 支付宝默认网关
        /// </summary>
        //public const string Gateway = "https://openapi.alipaydev.com/gateway.do";//沙箱
        public const string GatewayUrl = "https://openapi.alipay.com/gateway.do";

        /// <summary>
        /// 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        /// </summary>
        public const string AlipayPublicKey = "";

        ///// <summary>
        ///// 商户应用私钥，您的原始格式RSA2私钥
        ///// </summary>

        //public const string MerchantPrivateKey = "";

        ///// <summary>
        ///// 商户应用公钥，您的原始格式RSA公钥
        ///// </summary>

        //public const string MerchantPublicKey = "";

        /// <summary>
        /// 签名方式
        /// </summary>

        public const string SignType = "RSA2";

        /// <summary>
        /// 编码格式
        /// </summary>

        public const string CharSet = "UTF-8";

        /// <summary>
        /// 版本
        /// </summary>

        public const string Version = "1.0";

        /// <summary>
        /// 数据格式
        /// </summary>

        public const string Format = "JSON";

        /// <summary>
        /// 是否从文件读取公私钥 如果为true ，那么公私钥应该配置为密钥文件路径
        /// </summary>

        public const bool IsKeyFromFile = false;
    }
}