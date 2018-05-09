// -----------------------------------------------------------------------
//  <copyright file="SendMailDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-10 3:12</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;


namespace OSharp.Demo.Identity.Dtos
{
    public class SendMailDto
    {
        /// <summary>
        /// 获取或设置 Email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 验证码
        /// </summary>
        public string VerifyCode { get; set; }
    }
}