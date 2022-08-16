﻿// -----------------------------------------------------------------------
//  <copyright file="OnlineUser.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 0:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using OSharp.Identity.JwtBearer;


namespace OSharp.Identity
{
    /// <summary>
    /// 在线用户信息
    /// </summary>
    public class OnlineUser
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 用户Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 用户头像
        /// </summary>
        public string HeadImg { get; set; }

        /// <summary>
        /// 获取或设置 是否管理
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 获取或设置 下次请求是否刷新AccessToken
        /// </summary>
        public bool IsRefreshNext { get; set; }

        /// <summary>
        /// 获取或设置 用户角色
        /// </summary>
        public string[] Roles { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 获取或设置 客户端刷新Token
        /// </summary>
        public IDictionary<string, RefreshToken> RefreshTokens { get; set; }
        
        /// <summary>
        /// 获取 扩展数据字典
        /// </summary>
        public IDictionary<string, string> ExtendData { get; } = new Dictionary<string, string>();
    }
}
