// -----------------------------------------------------------------------
//  <copyright file="ResetPasswordDto.cs" company="OSharp开源团队">
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
    /// 重置密码DTO
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 重置密码校验标识，由邮箱、手机等发送
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 获取或设置 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// 获取或设置 确认密码
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "新密码与确认密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
}