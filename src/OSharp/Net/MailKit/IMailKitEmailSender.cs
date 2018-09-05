﻿using System.Net.Mail;

namespace OSharp.Net
{
    /// <summary>
    /// 用于通过SMTP发送电子邮件
    /// </summary>
    public interface IMailKitEmailSender : IEmailSender
    {
        /// <summary>
        /// 创建和配置新的<see cref="SmtpClient"/>对象来发送电子邮件
        /// </summary>
        /// <returns>
        /// 一个准备发送电子邮件的<see cref="SmtpClient"/>对象
        /// </returns>
        SmtpClient BuildMailKitClient();
    }
}
