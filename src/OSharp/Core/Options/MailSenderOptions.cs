// -----------------------------------------------------------------------
//  <copyright file="MailSenderOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-08 3:06</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options
{
    /// <summary>
    /// 邮件发送选项
    /// </summary>
    public class MailSenderOptions
    {
        /// <summary>
        /// 获取或设置 邮件发送服务器
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 获取或设置 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获取或设置 是否SSL
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// 获取或设置 发送方显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取或设置 发送方用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 发送方密码
        /// </summary>
        public string Password { get; set; }
    }
}