// -----------------------------------------------------------------------
//  <copyright file="ProfileEditInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-24 13:05</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Entities;
using OSharp.Entity;
using OSharp.Mapping;
using System.ComponentModel.DataAnnotations;


namespace OSharp.Hosting.Identity.Dtos
{
    /// <summary>
    /// 输入DTO：用户资料编辑
    /// </summary>
    [MapTo(typeof(User))]
    public class ProfileEditDto : IInputDto<int>
    {
        /// <summary>
        /// 获取或设置 主键，唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        [Required]
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 头像
        /// </summary>
        public string HeadImg { get; set; }
    }
}