// -----------------------------------------------------------------------
//  <copyright file="UserLoginInfoEx.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-21 23:47</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Identity;


namespace OSharp.Identity.OAuth2
{
    /// <summary>
    /// 第三方用户登录信息
    /// </summary>
    public class UserLoginInfoEx : UserLoginInfo
    {
        /// <summary>
        /// 初始化一个<see cref="UserLoginInfoEx"/>类型的新实例
        /// </summary>
        public UserLoginInfoEx()
            : base(null, null, null)
        { }

        /// <summary>
        /// Creates a new instance of <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" />
        /// </summary>
        /// <param name="loginProvider">The provider associated with this login information.</param>
        /// <param name="providerKey">The unique identifier for this user provided by the login provider.</param>
        /// <param name="displayName">The display name for this user provided by the login provider.</param>
        public UserLoginInfoEx(string loginProvider, string providerKey, string displayName)
            : base(loginProvider, providerKey, displayName)
        { }

        /// <summary>
        /// 获取或设置 头像URL
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 获取或设置 注册IP
        /// </summary>
        public string RegisterIp { get; set; }

        /// <summary>
        /// 获取或设置 登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        public string Password { get; set; }
    }
}