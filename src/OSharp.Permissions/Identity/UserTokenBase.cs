// -----------------------------------------------------------------------
//  <copyright file="UserTokenBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-05 0:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;


namespace OSharp.Identity
{
    /// <summary>
    /// 表示用户的身份验证令牌的基类
    /// </summary>
    /// <typeparam name="TUserKey">用户编号</typeparam>
    public abstract class UserTokenBase<TUserKey> : EntityBase<Guid>
        where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 获取或设置 当前用户标识的登录提供者
        /// </summary>
        [DisplayName("登录提供者")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// 获取或设置 令牌名称
        /// </summary>
        [DisplayName("令牌名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 令牌值
        /// </summary>
        [DisplayName("令牌值")]
        public string Value { get; set; }
    }
}