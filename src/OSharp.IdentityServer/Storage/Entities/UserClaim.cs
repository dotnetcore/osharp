// -----------------------------------------------------------------------
//  <copyright file="UserClaim.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-18 23:55</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Entity;


namespace OSharp.IdentityServer.Storage.Entities
{
    /// <summary>
    /// 用户声明基类
    /// </summary>
    public abstract class UserClaim : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 声明类型
        /// </summary>
        [DisplayName("声明类型"), StringLength(200), Required]
        public string Type { get; set; }
    }
}