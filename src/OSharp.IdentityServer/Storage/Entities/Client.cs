// -----------------------------------------------------------------------
//  <copyright file="Client.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 23:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using IdentityServer4.Models;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 实体类：客户端
    /// </summary>
    [Description("客户端")]
    [TableNamePrefix("Id4")]
    public class Client : EntityBase<int>
    {
        #region 基本属性

        /// <summary>
        /// 获取或设置 否启用客户端。默认为true。
        /// </summary>
        [DisplayName("是否启用")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 获取或设置 客户端的唯一ID
        /// </summary>
        [DisplayName("客户端码"), StringLength(200), Required]
        public string ClientId { get; set; }

        /// <summary>
        /// 获取或设置 客户端机密列表 - 访问令牌端点的凭据。
        /// </summary>
        [DisplayName("客户端密钥列表")]
        public virtual ICollection<ClientSecret> ClientSecrets { get; set; }

        /// <summary>
        /// 获取或设置 指定此客户端是否需要密钥才能从令牌端点请求令牌，默认为true
        /// </summary>
        [DisplayName("需要客户端密钥")]
        public bool RequireClientSecret { get; set; } = true;

        /// <summary>
        /// 获取或设置 指定允许客户端使用的授权类型。
        /// </summary>
        [DisplayName("协议类型"), StringLength(200), Required]
        public string ProtocolType { get; set; } = "oidc";

        /// <summary>
        /// 获取或设置 指定使用基于授权代码的授权类型的客户端是否必须发送校验密钥
        /// </summary>
        [DisplayName("需要PKCE")]
        public bool RequirePkce { get; set; }

        /// <summary>
        /// 获取或设置 指定使用PKCE的客户端是否可以使用纯文本代码质询（不推荐 - 默认为false）
        /// </summary>
        [DisplayName("允许纯文本PKCE")]
        public bool AllowPlainTextPkce { get; set; } = false;

        /// <summary>
        /// 获取或设置 需要请求对象
        /// </summary>
        [DisplayName("需要请求对象")]
        public bool RequireRequestObject { get; set; }

        /// <summary>
        /// 获取或设置 指定允许的URI以返回令牌或授权码
        /// </summary>
        [DisplayName("跳转URI列表")]
        public virtual ICollection<ClientRedirectUri> RedirectUris { get; set; }

        /// <summary>
        /// 获取或设置 默认情况下，客户端无权访问任何资源 - 通过添加相应的范围名称来指定允许的资源
        /// </summary>
        [DisplayName("客户端作用域列表")]
        public virtual ICollection<ClientScope> AllowedScopes { get; set; }

        /// <summary>
        /// 获取或设置 指定允许客户端使用的授权类型。使用GrantTypes该类进行常见组合。
        /// </summary>
        [DisplayName("允许的授权类型")]
        public virtual ICollection<ClientGrantType> AllowedGrantTypes { get; set; }

        /// <summary>
        /// 获取或设置 指定此客户端是否可以请求刷新令牌（请求offline_access范围）
        /// </summary>
        [DisplayName("允许离线访问")]
        public bool AllowOfflineAccess { get; set; }

        /// <summary>
        /// 获取或设置 指定是否允许此客户端通过浏览器接收访问令牌。这对于强化允许多种响应类型的流是有用的（例如，通过禁止应该使用code id_token来添加令牌响应类型并因此将令牌泄漏到浏览器的混合流客户端。
        /// </summary>
        [DisplayName("允许浏览器接收访问令牌")]
        public bool AllowAccessTokensViaBrowser { get; set; }

        /// <summary>
        /// 获取或设置 字典根据需要保存任何自定义客户端特定值。
        /// </summary>
        [DisplayName("客户端属性列表")]
        public virtual ICollection<ClientProperty> Properties { get; set; }

        #endregion

        #region 确认页面

        /// <summary>
        /// 获取或设置 客户端显示名称（用于记录和同意屏幕）
        /// </summary>
        [DisplayName("客户端名称"), StringLength(200)]
        public string ClientName { get; set; }

        /// <summary>
        /// 获取或设置 描述
        /// </summary>
        [DisplayName("描述"), StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置 指定是否需要同意屏幕。默认为true。
        /// </summary>
        [DisplayName("需要同意")]
        public bool RequireConsent { get; set; } = true;

        /// <summary>
        /// 获取或设置 指定用户是否可以选择存储同意决策。默认为true。
        /// </summary>
        [DisplayName("是否记住同意")]
        public bool AllowRememberConsent { get; set; } = true;

        /// <summary>
        /// 获取或设置 用户同意的生命周期，以秒为单位。默认为null（无到期）。
        /// </summary>
        [DisplayName("同意的生命周期")]
        public int? ConsentLifetime { get; set; } = null;

        /// <summary>
        /// 获取或设置 有关客户端的更多信息的URI（在同意屏幕上使用）
        /// </summary>
        [DisplayName("客户端Uri"), StringLength(2000)]
        public string ClientUri { get; set; }

        /// <summary>
        /// 获取或设置 URI到客户端徽标（在同意屏幕上使用）
        /// </summary>
        [DisplayName("LogoUri"), StringLength(2000)]
        public string LogoUri { get; set; }

        #endregion

        #region 认证/注销

        /// <summary>
        /// 获取或设置 指定在注销后重定向到的允许URI。
        /// </summary>
        [DisplayName("注销跳转Uri")]
        public virtual ICollection<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }

        /// <summary>
        /// 获取或设置 指定客户端的注销URI，以用于基于HTTP的前端通道注销。
        /// </summary>
        [DisplayName("前端注销Uri"), StringLength(2000)]
        public string FrontChannelLogoutUri { get; set; }

        /// <summary>
        /// 获取或设置 指定是否应将用户的会话ID发送到FrontChannelLogoutUri。默认为true。
        /// </summary>
        [DisplayName("需要前端会话Id")]
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;

        /// <summary>
        /// 获取或设置 指定客户端的注销URI，以用于基于HTTP的反向通道注销。
        /// </summary>
        [DisplayName("后端注销Uri"), StringLength(2000)]
        public string BackChannelLogoutUri { get; set; }

        /// <summary>
        /// 获取或设置 指定是否应在请求中将用户的会话ID发送到BackChannelLogoutUri。默认为true。
        /// </summary>
        [DisplayName("需要后端会话Id")]
        public bool BackChannelLogoutSessionRequired { get; set; } = true;

        /// <summary>
        /// 获取或设置 指定此客户端是否可以仅使用本地帐户或外部IdP。默认为true。
        /// </summary>
        [DisplayName("允许本地登录")]
        public bool EnableLocalLogin { get; set; } = true;

        /// <summary>
        /// 获取或设置 指定可以与此客户端一起使用的外部IdP（如果列表为空，则允许所有IdP）。默认为空。
        /// </summary>
        [DisplayName("客户端IdP限制")]
        public virtual ICollection<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }

        /// <summary>
        /// 获取或设置 自上次用户进行身份验证以来的最长持续时间（以秒为单位）。默认为null。您可以调整会话令牌的生命周期，以控制在使用Web应用程序时，用户需要重新输入凭据的时间和频率，而不是进行静默身份验证。
        /// </summary>
        [DisplayName("最长持续时间")]
        public int? UserSsoLifetime { get; set; }

        #endregion

        #region Token

        /// <summary>
        /// 获取或设置 允许的身份令牌签名算法
        /// </summary>
        [DisplayName("允许的身份令牌签名算法"), StringLength(100)]
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }

        /// <summary>
        /// 获取或设置 身份令牌的生命周期，以秒为单位（默认为300秒/ 5分钟）
        /// </summary>
        [DisplayName("身份令牌的生命周期")]
        public int IdentityTokenLifetime { get; set; } = 300;

        /// <summary>
        /// 获取或设置 访问令牌的生命周期，以秒为单位（默认为3600秒/ 1小时）
        /// </summary>
        [DisplayName("访问令牌的生命周期")]
        public int AccessTokenLifetime { get; set; } = 3600;

        /// <summary>
        /// 获取或设置 授权代码的生命周期，以秒为单位（默认为300秒/ 5分钟）
        /// </summary>
        [DisplayName("授权代码的生命周期")]
        public int AuthorizationCodeLifetime { get; set; } = 300;

        /// <summary>
        /// 获取或设置 刷新令牌的最长生命周期，以秒为单位 默认为2592000秒/ 30天
        /// </summary>
        [DisplayName("刷新令牌的最长生命周期")]
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        /// <summary>
        /// 获取或设置 滑动刷新令牌的生命周期，以秒为单位。默认为1296000秒/ 15天
        /// </summary>
        [DisplayName("滑动刷新令牌的生命周期")]
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;


        /// <summary>
        /// 获取或设置 ReUse 刷新令牌时，刷新令牌句柄将保持不变。
        /// OneTime刷新令牌时将更新刷新令牌句柄。这是默认值。
        /// </summary>
        [DisplayName("刷新令牌用法类型")]
        public TokenUsage RefreshTokenUsage { get; set; } = TokenUsage.OneTimeOnly;

        /// <summary>
        /// 获取或设置 Absolute 刷新令牌将在固定时间点到期（由AbsoluteRefreshTokenLifetime指定）。
        /// Sliding刷新令牌时，将刷新刷新令牌的生命周期（按SlidingRefreshTokenLifetime中指定的数量）。生命周期不会超过AbsoluteRefreshTokenLifetime。
        /// </summary>
        [DisplayName("刷新令牌过期类型")]
        public TokenExpiration RefreshTokenExpiration { get; set; } = TokenExpiration.Absolute;

        /// <summary>
        /// 获取或设置一个值，该值指示是否应在刷新令牌请求上更新访问令牌（及其声明）。
        /// </summary>
        [DisplayName("是否刷新时更新访问令牌声明")]
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        /// <summary>
        /// 获取或设置 访问令牌类型，默认为Jwt
        /// </summary>
        [DisplayName("访问令牌类型")]
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;

        /// <summary>
        /// 获取或设置 指定JWT访问令牌是否应具有嵌入的唯一ID（通过jti声明）。
        /// </summary>
        [DisplayName("包含JwtId")]
        public bool IncludeJwtId { get; set; }

        /// <summary>
        /// 获取或设置 如果指定，将由默认CORS策略服务实现（内存和EF）用于为JavaScript客户端构建CORS策略。
        /// </summary>
        [DisplayName("允许跨域来源")]
        public virtual ICollection<ClientCorsOrigin> AllowedCorsOrigins { get; set; }

        /// <summary>
        /// 获取或设置 允许客户端的设置声明（将包含在访问令牌中）。
        /// </summary>
        [DisplayName("客户端声明")]
        public virtual ICollection<ClientClaim> Claims { get; set; }

        /// <summary>
        /// 获取或设置 允许发送客户端声明。如果设置，将为每个流发送客户端声明。如果不是，仅用于客户端凭证流（默认为false）
        /// </summary>
        [DisplayName("允许发送客户端声明")]
        public bool AlwaysSendClientClaims { get; set; }

        /// <summary>
        /// 获取或设置 在请求id token和access token时，如果用户声明始终将其添加到id token而不是请求客户端使用userinfo endpoint。默认值为false。
        /// </summary>
        [DisplayName("总是在Token中包含用户信息")]
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        /// <summary>
        /// 获取或设置 客户端声明前缀。 如果设置，将以前缀为前缀客户端声明类型。默认为client_。目的是确保它们不会意外地与用户声明冲突。
        /// </summary>
        [DisplayName("客户端声明前缀"), StringLength(200)]
        public string ClientClaimsPrefix { get; set; } = "client_";

        /// <summary>
        /// 获取或设置 对于此客户端的用户，在成对的subjectId生成中使用的salt值。
        /// </summary>
        [DisplayName("成对主题Salt值"), StringLength(200)]
        public string PairWiseSubjectSalt { get; set; }

        #endregion

        #region 设备流程

        /// <summary>
        /// 获取或设置 指定用于客户端的用户代码的类型。否则使用默认值。
        /// </summary>
        [DisplayName("用户代码类型"), StringLength(100)]
        public string UserCodeType { get; set; }

        /// <summary>
        /// 获取或设置 设备代码的生命周期，以秒为单位（默认为300秒/ 5分钟）
        /// </summary>
        [DisplayName("设备代码的生命周期")]
        public int DeviceCodeLifetime { get; set; } = 300;

        #endregion

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime? Updated { get; set; }

        /// <summary>
        /// 获取或设置 最后访问时间
        /// </summary>
        [DisplayName("最后访问时间")]
        public DateTime? LastAccessed { get; set; }

        /// <summary>
        /// 获取或设置 禁止编辑
        /// </summary>
        [DisplayName("禁止编辑")]
        public bool NonEditable { get; set; }
    }
}