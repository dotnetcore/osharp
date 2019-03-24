// -----------------------------------------------------------------------
//  <copyright file="UserLoginBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-05 12:02</last-date>
// -----------------------------------------------------------------------

using OSharp.Entity;
using System;
using System.ComponentModel;


namespace OSharp.Identity
{
    /// <summary>
    /// 表示用户的登录及其关联提供程序信息基类
    /// </summary>
    /// <typeparam name="TUserKey">用户编号</typeparam>
    public abstract class UserLoginBase<TUserKey> : EntityBase<Guid>, ICreatedTime
    {
        /// <summary>
        /// 获取或设置 登录的登录提供程序（例如facebook, google, qq）。
        /// </summary>
        [DisplayName("登录的登录提供程序")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// 获取或设置 第三方登录用户的唯一标识，即用户编号
        /// </summary>
        [DisplayName("第三方用户的唯一标识")]
        public string ProviderKey { get; set; }

        /// <summary>
        /// 获取或设置 第三方登录用户昵称
        /// </summary>
        [DisplayName("第三方用户昵称")]
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// 获取或设置 头像
        /// </summary>
        [DisplayName("第三方用户头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 所属用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public TUserKey UserId { get; set; }
    }
}