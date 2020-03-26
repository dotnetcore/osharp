// -----------------------------------------------------------------------
//  <copyright file="UserBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-04 21:29</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Audits;
using OSharp.Entity;


namespace OSharp.Identity.Entities
{
    /// <summary>
    /// 用户信息基类
    /// </summary>
    /// <typeparam name="TKey">用户编号类型</typeparam>
    [TableNamePrefix("Identity")]
    public abstract class UserBase<TKey> : EntityBase<TKey>, ICreatedTime, ILockable, ISoftDeletable
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 初始化一个<see cref="UserBase{TUserKey}"/>类型的新实例
        /// </summary>
        protected UserBase()
        {
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        [Required, DisplayName("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 标准化的用户名
        /// </summary>
        [Required, DisplayName("标准化的用户名"), AuditIgnore]
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        [DisplayName("用户昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        [DisplayName("电子邮箱"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 标准化的电子邮箱
        /// </summary>
        [DisplayName("标准化的电子邮箱"), DataType(DataType.EmailAddress), AuditIgnore]
        public string NormalizeEmail { get; set; }

        /// <summary>
        /// 获取或设置 表示用户是否已确认其电子邮件地址的标志
        /// </summary>
        [DisplayName("电子邮箱确认")]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 获取或设置 密码哈希值
        /// </summary>
        [DisplayName("密码哈希值"), AuditIgnore]
        public string PasswordHash { get; set; }

        /// <summary>
        /// 获取或设置 用户头像
        /// </summary>
        [DisplayName("用户头像")]
        public string HeadImg { get; set; }

        /// <summary>
        /// 获取或设置 每当用户凭据发生变化（密码更改、登录删除）时必须更改的随机值。
        /// </summary>
        [DisplayName("安全标识"), AuditIgnore]
        public string SecurityStamp { get; set; }

        /// <summary>
        /// 获取或设置 一个随机值，必须在用户持续存储时更改。
        /// </summary>
        [DisplayName("版本标识"), AuditIgnore]
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 获取或设置 手机号码
        /// </summary>
        [DisplayName("手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置 手机号码是否已确认
        /// </summary>
        [DisplayName("手机号码确定")]
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 获取或设置 一个标志，指示是否为该用户启用了双因子身份验证。
        /// </summary>
        [DisplayName("双因子身份验证")]
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 获取或设置 当任何用户锁定结束时，UTC的日期和时间。
        /// </summary>
        [DisplayName("锁定时间")]
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// 获取或设置 指示用户是否可以被锁定的标志。
        /// </summary>
        [DisplayName("是否登录锁")]
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 获取或设置 当前用户失败的登录尝试次数。
        /// </summary>
        [DisplayName("登录失败次数")]
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 获取或设置 是否系统用户
        /// </summary>
        [DisplayName("是否系统用户")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定当前信息
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 数据逻辑删除时间，为null表示正常数据，有值表示已逻辑删除，同时删除时间每次不同也能保证索引唯一性
        /// </summary>
        public DateTime? DeletedTime { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return UserName;
        }
    }
}