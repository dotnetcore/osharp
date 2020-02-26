// -----------------------------------------------------------------------
//  <copyright file="UserInputDtoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 13:32</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.Identity.Dtos
{
    /// <summary>
    /// 用户信息输入DTO基类
    /// </summary>
    public abstract class UserInputDtoBase<TUserKey> : IInputDto<TUserKey>
    {
        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        [DisplayName("编号")]
        public TUserKey Id { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        [Required, DisplayName("用户名")]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        [DisplayName("用户昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        [Required, DisplayName("电子邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 表示用户是否已确认其电子邮件地址的标志
        /// </summary>
        [DisplayName("电子邮箱确认")]
        public bool EmailConfirmed { get; set; }

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
        /// 获取或设置 是否启用登录锁
        /// </summary>
        [DisplayName("是否登录锁")]
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }
    }
}