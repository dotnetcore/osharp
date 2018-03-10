// -----------------------------------------------------------------------
//  <copyright file="UserLoginBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-05 12:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.Identity
{
    /// <summary>
    /// 表示用户的登录及其关联提供程序信息基类
    /// </summary>
    /// <typeparam name="TUserKey">用户编号</typeparam>
    public abstract class UserLoginBase<TUserKey> : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 登录的登录提供程序（例如facebook, google）。
        /// </summary>
        [DisplayName("登录的登录提供程序")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// 获取或设置 此登录的提供者唯一标识符。
        /// </summary>
        [DisplayName("此登录的提供者唯一标识符")]
        public string ProviderKey { get; set; }

        /// <summary>
        /// 获取或设置 登录提供者在UI上显示的友好名称
        /// </summary>
        [DisplayName("显示的友好名称")]
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// 获取或设置 所属用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public TUserKey UserId { get; set; }
    }
}