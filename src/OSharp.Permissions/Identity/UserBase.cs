// -----------------------------------------------------------------------
//  <copyright file="UserBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-04 21:29</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.Identity
{
    /// <summary>
    /// 用户信息基类
    /// </summary>
    /// <typeparam name="TUserKey"></typeparam>
    public abstract class UserBase<TUserKey> : EntityBase<TUserKey>, ICreatedTime
        where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 初始化一个<see cref="UserBase"/>类型的新实例
        /// </summary>
        protected UserBase()
        {
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 标准化的用户名
        /// </summary>
        [Required]
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 标准化的电子邮箱
        /// </summary>
        [Required]
        public string NormalizeEmail { get; set; }

        /// <summary>
        /// 获取或设置 表示用户是否已确认其电子邮件地址的标志
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 获取或设置 密码哈希值
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 获取或设置 每当用户凭据发生变化（密码更改、登录删除）时必须更改的随机值。
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// 获取或设置 一个随机值，必须在用户持续存储时更改。
        /// </summary>
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 获取或设置 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置 手机号码是否已确认
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 获取或设置 一个标志，指示是否为该用户启用了双因素身份验证。
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 获取或设置 当任何用户锁定结束时，UTC的日期和时间。
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// 获取或设置 指示用户是否可以被锁定的标志。
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 获取或设置 当前用户失败的登录尝试次数。
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return UserName;
        }

    }
}