// -----------------------------------------------------------------------
//  <copyright file="DefaultEmailSender.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-08 2:47</last-date>
// -----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Net
{
    /// <summary>
    /// 默认邮件发送者
    /// </summary>
    public class DefaultEmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly OsharpOptions _options;

        /// <summary>
        /// 初始化一个<see cref="DefaultEmailSender"/>类型的新实例
        /// </summary>
        public DefaultEmailSender(IServiceProvider provider)
        {
            _logger = provider.GetLogger<DefaultEmailSender>();
            _options = provider.GetOSharpOptions();
        }

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="email">接收人Email</param>
        /// <param name="subject">Email标题</param>
        /// <param name="body">Email内容</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            MailSenderOptions mailSender = _options.MailSender;
            if (mailSender == null || mailSender.Host == null || mailSender.Host.Contains("请替换"))
            {
                throw new OsharpException("邮件发送选项不存在，请在appsetting.json配置OSharp:MailSender节点");
            }

            string host = mailSender.Host,
                displayName = mailSender.DisplayName,
                userName = mailSender.UserName,
                password = mailSender.Password;
            bool enableSsl = mailSender.EnableSsl;
            int port = mailSender.Port;
            if (port == 0)
            {
                port = enableSsl ? 465 : 25;
            }

            SmtpClient client = new SmtpClient(host, port)
            {
                UseDefaultCredentials = true,
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(userName, password)
            };

            string fromEmail = userName.Contains("@") ? userName : "{0}@{1}".FormatWith(userName, client.Host.Replace("smtp.", ""));
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail, displayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(email);
            await client.SendMailAsync(mail);
            _logger.LogDebug($"发送邮件到“{mail.To.ExpandAndToString()}”，标题：{mail.Subject}");
        }
    }
}