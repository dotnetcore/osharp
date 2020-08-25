// -----------------------------------------------------------------------
//  <copyright file="ConfirmEmailDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;


namespace OSharp.Hosting.Identity.Dtos
{
    /// <summary>
    /// 确认邮箱DTO
    /// </summary>
    public class ConfirmEmailDto
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 邮件码
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}