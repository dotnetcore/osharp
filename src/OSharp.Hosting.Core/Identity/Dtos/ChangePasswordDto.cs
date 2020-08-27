// -----------------------------------------------------------------------
//  <copyright file="ChangePasswordDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Identity.Dtos
{
    /// <summary>
    /// 修改密码DTO
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// 获取或设置 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 获取或设置 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 获取或设置 新密码确认
        /// </summary>
        public string ConfirmNewPassword { get; set; }
    }
}