// -----------------------------------------------------------------------
//  <copyright file="UserClaimBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-01-31 19:16</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using OSharp.Entity;


namespace OSharp.Identity.Entities
{
    /// <summary>
    /// 用户声明基类
    /// </summary>
    /// <typeparam name="TKey">用户声明编号类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    [TableNamePrefix("Identity")]
    public abstract class UserClaimBase<TKey, TUserKey> : EntityBase<TKey>
        where TUserKey : IEquatable<TUserKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public TUserKey UserId { get; set; }

        /// <summary>
        /// 获取或设置 声明类型
        /// </summary>
        [Required]
        [DisplayName("声明类型")]
        public string ClaimType { get; set; }

        /// <summary>
        /// 获取或设置 声明值
        /// </summary>
        [DisplayName("声明值")]
        public string ClaimValue { get; set; }

        /// <summary>
        /// 使用类型和值创建一个声明对象
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        /// <summary>
        /// 使用一个声明对象初始化
        /// </summary>
        /// <param name="other">声明对象</param>
        public virtual void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }
}